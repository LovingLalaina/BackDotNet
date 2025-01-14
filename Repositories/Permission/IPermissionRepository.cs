using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Permission;

namespace back_dotnet
{
    public interface IPermissionRepository
    {
        Task<List<Permission?>> GetAllAsync();
        Task<Permission?> GetByIdAsync(Guid id);
        Task<Permission?> CreateAsync(CreateOrUpdatePermissionDto permission);
        Task<Permission?> UpdateAsync(Guid id, CreateOrUpdatePermissionDto updatedRequestPermission);
        Task<Permission?> DeleteAsync(Guid id);
    }
}
