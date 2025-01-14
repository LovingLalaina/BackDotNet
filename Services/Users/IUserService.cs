using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_dotnet.Models.DTOs.Users;

namespace back_dotnet.Services.Users
{
    public interface IUserService
    {
        Task<List<GetUserDto>?> GetAllAsync();
        Task<GetUserDto?> GetByIdAsync(Guid uuid);
        Task<bool> UserWithThisEmailAsync(string email);
        Task<GetUserDto?> CreateAsync(CreateUserDto createUserDto);
        Task<GetUserDto?> UpdateAsync(Guid uuid, UpdateUserDto updateUserDto);
        Task<GetUserDto?> DeleteAsync(Guid uuid);
        Task<List<GetUserDto>?> GetAllUsersByDepartmentAsync(string department);
        Task<List<GetUserDto>> SearchUsersAsync(SearchUserDto searchUserDto);
    }
}