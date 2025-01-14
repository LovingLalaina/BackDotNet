using back_dotnet.Models.DTOs.Permission;

namespace back_dotnet.Services.Permission
{
    public interface IPermissionService
    {
        Task<PermissionResponseDto?> CreateAsync(CreateOrUpdatePermissionDto requestPermission);
        Task<List<PermissionResponseDto>?> GetAllAsync();
        Task<PermissionResponseDto?> GetByIdAsync(Guid id);
        Task<PermissionResponseDto?> UpdateAsync(Guid id, CreateOrUpdatePermissionDto updateRequestPermission);
        Task<PermissionResponseDto?> DeleteAsync(Guid id);
    }
}
