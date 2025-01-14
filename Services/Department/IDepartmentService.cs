using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Department;
using back_dotnet.Models.DTOs.Role;

namespace back_dotnet.Services
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDto>> GetAllDepartments(List<string> relations);
        Task<DepartmentDto> GetDepartmentById(Guid Id, List<string>? relations = null);
        Task<DepartmentDto> CreateDepartment(CreateDepartmentDto departmentDto);
        Task<DepartmentDto> UpdateDepartment(Guid Id, UpdateDepartmentDto departmentDto);
        Task<DepartmentDto> DeleteDepartment(Guid Id);
        Task<List<DepartmentDto>> GetAllDepartmentWithAnonymous(List<string> relations);
        Task<List<DepartmentDto>> SearchDepartmentAsync(SearchDepartmentDto search);

        
    }
}