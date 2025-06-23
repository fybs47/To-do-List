using Application.DTOs.Shared;
using Application.DTOs.Users;
using Application.Services.Interfaces;

namespace Application.Services.Implementations;

public class UserService : IUserService
{
    public Task<UserDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResponse<UserDto>> GetAllUsersAsync(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto, Guid currentUserId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task ToggleUserStatusAsync(Guid userId, Guid currentUserId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}