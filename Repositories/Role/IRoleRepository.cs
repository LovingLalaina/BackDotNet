using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs;
using back_dotnet.Models.DTOs.Role;

namespace back_dotnet
{
    public interface IRoleRepository
    {
        Task<List<Role?>> GetAllAsync();
        Task<Role?> GetByIdAsync(Guid id);
        Task<Role?> CreateAsync(CreateRoleDto role);
        Task<Role?> UpdateAsync(Guid id, UpdateRoleDto role);
        Task<Role?> DeleteAsync(Guid id);
        Task<List<Role?>> SearchRoleAsync(string search);
    }
}
