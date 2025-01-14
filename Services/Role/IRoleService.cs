using back_dotnet.Models.DTOs;
using back_dotnet.Models.DTOs.Role;

namespace back_dotnet
{
    public interface IRoleService
    {
        Task<RoleResponseDto?> CreateAsync(CreateRoleDto requestRole);
        Task<List<RoleResponseDto>?> GetAllAsync();
        Task<RoleResponseDto?> GetByIdAsync(Guid id);
        Task<RoleResponseDto?> UpdateAsync(Guid id, UpdateRoleDto role);
        Task<RoleResponseDto?> DeleteAsync(Guid id);
        Task<List<RoleResponseDto>> SearchRoleAsync(SearchRoleDto searchRoleDto);
    }
}
