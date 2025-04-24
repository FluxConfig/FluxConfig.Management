using FluentValidation;
using FluxConfig.Management.Domain.Contracts.Dal.Entities;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Domain;
using FluxConfig.Management.Domain.Exceptions.Domain.User;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using FluxConfig.Management.Domain.Hasher;
using FluxConfig.Management.Domain.Mappers.User;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Models.User;
using FluxConfig.Management.Domain.Services.Interfaces;
using FluxConfig.Management.Domain.Validators.User;

namespace FluxConfig.Management.Domain.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task ChangeUserEmail(ChangeUserEmailModel changeEmailModel, CancellationToken cancellationToken)
    {
        try
        {
            await ChangeUserEmailUnsafe(changeEmailModel, cancellationToken);
        }
        catch (ValidationException ex)
        {
            throw new BadRequestException("Invalid request parameters.", ex);
        }
        catch (EntityNotFoundException ex)
        {
            throw new UserNotFoundException(
                message: $"Unable to find user with email: {changeEmailModel.User.Email}.",
                invalidEmail: changeEmailModel.User.Email,
                innerException: ex
            );
        }
        catch (EntityAlreadyExistsException ex)
        {
            throw new UserAlreadyExistsException(
                message: $"User with the same credential: {changeEmailModel.NewEmail} already exists.",
                email: changeEmailModel.NewEmail,
                innerException: ex);
        }
    }

    private async Task ChangeUserEmailUnsafe(ChangeUserEmailModel changeEmailModel, CancellationToken cancellationToken)
    {
        var validator = new ChangeUserEmailModelValidator();
        await validator.ValidateAndThrowAsync(changeEmailModel, cancellationToken);

        using var transaction = _userRepository.CreateTransactionScope();

        await _userRepository.UpdateUserEmail(
            userId: changeEmailModel.User.Id,
            newEmail: changeEmailModel.NewEmail,
            cancellationToken: cancellationToken
        );

        transaction.Complete();
    }

    public async Task ChangeUserPassword(ChangeUserPasswordModel changePasswordModel,
        CancellationToken cancellationToken)
    {
        try
        {
            await ChangeUserPasswordUnsafe(changePasswordModel, cancellationToken);
        }
        catch (ValidationException ex)
        {
            throw new BadRequestException("Invalid request parameters.", ex);
        }
        catch (EntityNotFoundException ex)
        {
            throw new UserNotFoundException(
                message: $"Unable to find user with email: {changePasswordModel.User.Email}.",
                invalidEmail: changePasswordModel.User.Email,
                innerException: ex
            );
        }
    }

    private async Task ChangeUserPasswordUnsafe(ChangeUserPasswordModel changePasswordModel,
        CancellationToken cancellationToken)
    {
        var validator = new ChangeUserPasswordModelValidator();
        await validator.ValidateAndThrowAsync(changePasswordModel, cancellationToken);

        using var transaction = _userRepository.CreateTransactionScope();

        await _userRepository.UpdateUserPassword(
            userId: changePasswordModel.User.Id,
            newHashedPassword: PasswordHasher.Hash(changePasswordModel.NewPassword),
            cancellationToken: cancellationToken
        );

        transaction.Complete();
    }

    public async Task ChangeUserUsername(ChangeUserUsernameModel changeUsernameModel,
        CancellationToken cancellationToken)
    {
        try
        {
            await ChangeUserUsernameUnsafe(changeUsernameModel, cancellationToken);
        }
        catch (ValidationException ex)
        {
            throw new BadRequestException("Invalid request parameters.", ex);
        }
        catch (EntityNotFoundException ex)
        {
            throw new UserNotFoundException(
                message: $"Unable to find user with email: {changeUsernameModel.User.Email}.",
                invalidEmail: changeUsernameModel.User.Email,
                innerException: ex
            );
        }
    }

    private async Task ChangeUserUsernameUnsafe(ChangeUserUsernameModel changeUsernameModel,
        CancellationToken cancellationToken)
    {
        var validator = new ChangeUserUsernameModelValidator();
        await validator.ValidateAndThrowAsync(changeUsernameModel, cancellationToken);

        using var transaction = _userRepository.CreateTransactionScope();

        await _userRepository.UpdateUserUsername(
            userId: changeUsernameModel.User.Id,
            newUsername: changeUsernameModel.NewUsername,
            cancellationToken: cancellationToken
        );

        transaction.Complete();
    }

    public async Task ChangeUserRole(UserGlobalRole newRole, long userId, long adminId,
        CancellationToken cancellationToken)
    {
        try
        {
            await ChangeUserRoleUnsafe(newRole, userId, adminId, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new UserNotFoundException(
                message: $"Unable to find user with id: {userId}.",
                invalidEmail: null,
                id: userId,
                innerException: ex
            );
        }
    }

    private async Task ChangeUserRoleUnsafe(UserGlobalRole newRole, long userId, long adminId,
        CancellationToken cancellationToken)
    {
        if (userId == adminId)
        {
            throw new AdminChangeHisRoleException(
                message: "Unable to change admin role.",
                adminId: adminId);
        }

        using var transaction = _userRepository.CreateTransactionScope();

        await _userRepository.UpdateUserRole(
            userId: userId,
            newRole: newRole,
            cancellationToken: cancellationToken
        );

        transaction.Complete();
    }

    public async Task DeleteUser(long userId, CancellationToken cancellationToken)
    {
        try
        {
            await DeleteUserUnsafe(userId, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new UserNotFoundException(
                message: $"Unable to find user with  id: {userId}.",
                invalidEmail: null,
                id: userId,
                innerException: ex
            );
        }
    }

    private async Task DeleteUserUnsafe(long userId, CancellationToken cancellationToken)
    {
        using var transaction = _userRepository.CreateTransactionScope();

        await _userRepository.DeleteUser(
            userId: userId,
            cancellationToken: cancellationToken
        );

        transaction.Complete();
    }

    public async Task<IReadOnlyList<UserModel>> GetAllUsers(CancellationToken cancellationToken)
    {
        using var transaction = _userRepository.CreateTransactionScope();

        IReadOnlyList<UserCredentialsEntity> entities = await _userRepository.GetAllUsers(cancellationToken);

        transaction.Complete();

        return entities.OrderBy(e => e.Email).MapEntitiesToModels();
    }

    public async Task<UserModel> GetUser(long userId, CancellationToken cancellationToken)
    {
        try
        {
            return await GetUserUnsafe(userId, cancellationToken);
        }
        catch (EntityNotFoundException ex)
        {
            throw new UserNotFoundException(
                message: $"Unable to find user with  id: {userId}.",
                invalidEmail: null,
                id: userId,
                innerException: ex
            );
        }
    }

    private async Task<UserModel> GetUserUnsafe(long userId, CancellationToken cancellationToken)
    {
        using var transaction = _userRepository.CreateTransactionScope();

        var userEntity = await _userRepository.GetUserById(
            userId: userId,
            cancellationToken: cancellationToken
        );

        transaction.Complete();

        return userEntity.MapEntityToModel();
    }
}