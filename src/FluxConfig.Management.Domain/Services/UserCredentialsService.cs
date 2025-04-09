using FluentValidation;
using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Domain;
using FluxConfig.Management.Domain.Exceptions.Domain.User;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using FluxConfig.Management.Domain.Hasher;
using FluxConfig.Management.Domain.Mappers.User;
using FluxConfig.Management.Domain.Models.Auth;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Models.User;
using FluxConfig.Management.Domain.Services.Interfaces;
using FluxConfig.Management.Domain.Validators.Auth;

namespace FluxConfig.Management.Domain.Services;

public class UserCredentialsService : IUserCredentialsService
{
    private readonly IUserRepository _userRepository;
    private readonly ISessionsRepository _sessionsRepository;

    public UserCredentialsService(IUserRepository userRepository, ISessionsRepository sessionsRepository)
    {
        _userRepository = userRepository;
        _sessionsRepository = sessionsRepository;
    }

    public async Task RegisterNewUser(UserRegisterModel model, CancellationToken cancellationToken)
    {
        try
        {
            await RegisterNewUserUnsafe(model, cancellationToken);
        }
        catch (ValidationException ex)
        {
            throw new BadRequestException("Invalid request parameters.", ex);
        }
        catch (EntityNotFoundException ex)
        {
            throw new UserNotFoundException(
                message: $"Unable to find user with email: {model.Email} to grant role.",
                invalidEmail: model.Email,
                innerException: ex
            );
        }
        catch (EntityAlreadyExistsException ex)
        {
            throw new UserAlreadyExistsException(
                message: $"User with the same credential: {model.Email} already exists.",
                email: model.Email,
                innerException: ex);
        }
    }


    private async Task RegisterNewUserUnsafe(UserRegisterModel registerModel, CancellationToken cancellationToken)
    {
        var validator = new UserRegisterModelValidator();
        await validator.ValidateAndThrowAsync(registerModel, cancellationToken);

        using var transaction = _userRepository.CreateTransactionScope();

        long createdUserId = await _userRepository.AddUserCredentials(
            entity: registerModel.MapModelToEntity(),
            cancellationToken: cancellationToken
        );

        var grantedRoleIds = await _userRepository.AddUserGlobalRoles(
            entities:
            [
                new UserGlobalRoleEntity
                {
                    Id = -1,
                    UserId = createdUserId,
                    Role = UserGlobalRole.Member
                }
            ],
            cancellationToken: cancellationToken
        );

        transaction.Complete();
    }

    public async Task<SetCookieModel> LoginUser(UserLoginModel model, CancellationToken cancellationToken)
    {
        try
        {
            return await LoginUserUnsafe(
                loginModel: model,
                cancellationToken: cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new UserNotFoundException(
                message: $"User with credentials: {model.Email} could not be found.",
                invalidEmail: model.Email,
                innerException: ex);
        }
    }


    private async Task<SetCookieModel> LoginUserUnsafe(UserLoginModel loginModel, CancellationToken cancellationToken)
    {
        using var transaction = _sessionsRepository.CreateTransactionScope();

        var userEntity = await _userRepository.GetUserByEmail(
            userEmail: loginModel.Email,
            cancellationToken: cancellationToken
        );

        if (!PasswordHasher.Verify(
                password: loginModel.Password,
                hashedPassword: userEntity.Password))
        {
            throw new InvalidUserCredentialsException(
                message: "Invalid credentials provided",
                email: loginModel.Email,
                invalidPassword: loginModel.Password
            );
        }

        IReadOnlyList<UserGlobalRole> userRoles = await GetUserGlobalRoles(
            userId: userEntity.Id,
            cancellationToken: cancellationToken
        );


        string sessionId = GenerateRandomSessionId();
        DateTimeOffset expirationDate = loginModel.RememberUser
            ? DateTimeOffset.UtcNow.AddHours(24)
            : DateTimeOffset.UtcNow.AddHours(2);

        await _sessionsRepository.CreateUserSession(
            entity: new UserSessionEntity
            {
                UserId = userEntity.Id,
                Id = sessionId,
                ExpirationDate = expirationDate
            },
            cancellationToken: cancellationToken
        );

        transaction.Complete();

        return new SetCookieModel(
            User: userEntity.MapEntityToModel(userRoles),
            Session: new SessionModel(
                Id: sessionId,
                UserId: userEntity.Id,
                ExpirationDate: expirationDate
            )
        );
    }

    private async Task<IReadOnlyList<UserGlobalRole>> GetUserGlobalRoles(long userId,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<UserGlobalRoleEntity> userRolesEntities = await _userRepository.GetUserGlobalRoles(
            userId: userId,
            cancellationToken: cancellationToken
        );

        IReadOnlyList<UserGlobalRole> userRoles = userRolesEntities.Count == 0
            ? new[] { UserGlobalRole.Member }
            : userRolesEntities.Select(e => e.Role).Distinct().ToList();

        return userRoles;
    }

    private static string GenerateRandomSessionId()
    {
        Guid sessionGuid = Guid.NewGuid();

        return sessionGuid.ToString();
    }


    public async Task LogoutUser(string sessionId, CancellationToken cancellationToken)
    {
        using var transaction = _sessionsRepository.CreateTransactionScope();

        await _sessionsRepository.DeleteUserSession(
            sessionId: sessionId,
            cancellationToken: cancellationToken
        );

        transaction.Complete();
    }


    public async Task<UserModel> UserCheckAuth(string? sessionId, CancellationToken cancellationToken)
    {
        try
        {
            return await CheckUserAuthUnsafe(sessionId, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new UserUnauthenticatedException(
                message: "Invalid credentials.",
                reason: "Invalid credentials.",
                innerException: ex
            );
        }
    }


    private async Task<UserModel> CheckUserAuthUnsafe(string? sessionId, CancellationToken cancellationToken)
    {
        if (sessionId == null)
        {
            throw new UserUnauthenticatedException(
                message: "Authentication credentials were not provided.",
                reason: "Authentication credentials were not provided."
            );
        }

        using var transaction = _userRepository.CreateTransactionScope();

        UserCredentialsEntity userEntity = await _userRepository.GetUserBySessionId(
            sessionId: sessionId,
            curTime: DateTimeOffset.UtcNow,
            cancellationToken: cancellationToken
        );

        IReadOnlyList<UserGlobalRole> userRoles = await GetUserGlobalRoles(
            userId: userEntity.Id,
            cancellationToken: cancellationToken
        );

        transaction.Complete();

        return userEntity.MapEntityToModel(userRoles);
    }
}