
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Department;

namespace back_dotnet.Repositories
{
	public interface IDepartmentRepository
	{
		Task<List<Department>> GetAllDepartment(List<string> relations);
		Task<Department?> GetDepartmentById(Guid Id, List<string>? relations);
		Task<Department> CreateDepartment(Department department);
		Task<Department?> UpdateDepartment(Department department);
		Task<Department?> DeleteDepartment(Guid id);
		Task<List<Department>> GetAllDepartmentWithAnonymous(List<string> relations);
		Task<List<Department>> SearchDepartmentAsync(string search);

	}
}