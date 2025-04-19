using FluentValidation;
using FluxConfig.Management.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Domain;
using FluxConfig.Management.Domain.Exceptions.Domain.User;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Domain.Models.User;
using FluxConfig.Management.Domain.Services.Interfaces;

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
    
    private async Task ChangeUserEmailUnsafe(ChangeUserEmailModel chaneEmailModel, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task ChangeUserPassword(ChangeUserPasswordModel changePasswordModel, CancellationToken cancellationToken)
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
    
    private async Task ChangeUserPasswordUnsafe(ChangeUserPasswordModel changePasswordModel, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task ChangeUserUsername(ChangeUserUsernameModel changeUsernameModel, CancellationToken cancellationToken)
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
    
    private async Task ChangeUserUsernameUnsafe(ChangeUserUsernameModel changeUsernameModel, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task ChangeUserRole(UserGlobalRole newRole, long userId, CancellationToken cancellationToken)
    {
        try
        {
            await ChangeUserRoleUnsafe(newRole, userId, cancellationToken);
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
    
    private async Task ChangeUserRoleUnsafe(UserGlobalRole newRole, long userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<UserModel>> GetAllUsers(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}