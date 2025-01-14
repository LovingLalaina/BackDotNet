using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Users;

namespace back_dotnet.Repositories.Users
{
    public interface IUserRepository
    {
        Task<List<GetUserDto>?> GetAll();
        Task<GetUserDto?> GetById(Guid uuid);
        Task<GetUserDto?> UserWithThisEmail(string email);
        Task<GetUserDto?> GetUserWithMail(string email);
        Task<GetUserDto?> Create(User user);
        Task<User?> Update(Guid uuid, UpdateUserDto user, Guid file);
        Task<GetUserDto?> Delete(Guid uuid);
        Task UpdateUserAsync(Guid userId, string newPassword);
        Task<User?> FindUserByUuidAsync(string uuid);
        Task<List<GetUserDto>?> GetAllUsersByDepartmentAsync(string department);
        Task<List<GetUserDto>?> SearchUsersAsync(string search);
    }
}