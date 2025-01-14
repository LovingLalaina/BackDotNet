using AutoMapper;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Department;
using back_dotnet.Models.DTOs.Role;
using back_dotnet.Repositories;

namespace back_dotnet.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly Dictionary<string, string> DepartmentRelation = new()
        {
            {"role","IdRoleNavigation"},
            {"posts","Posts"}
        };

        private readonly IDepartmentRepository DepartmentRepository;
        private readonly IMapper Mapper;

        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            this.Mapper = mapper;
            DepartmentRepository = departmentRepository;
        }

        public async Task<DepartmentDto> CreateDepartment(CreateDepartmentDto departmentDto)
        {
            var department = new Department
            {
                Name = departmentDto.Name,
                IdRole = Guid.Parse(departmentDto.Role)
            };
            var departmentResult = await DepartmentRepository.CreateDepartment(department);

            return Mapper.Map<DepartmentDto>(departmentResult);
        }

        public async Task<DepartmentDto> DeleteDepartment(Guid Id)
        {
            var department = await DepartmentRepository.DeleteDepartment(Id);

            return Mapper.Map<DepartmentDto>(department);
        }

        public async Task<List<DepartmentDto>> GetAllDepartments(List<string> relations)
        {
            var departments = await DepartmentRepository.GetAllDepartment(GetDepartmentRelation(relations));

            return Mapper.Map<List<DepartmentDto>>(departments);
        }
        public async Task<DepartmentDto> GetDepartmentById(Guid Id, List<string>? relations = null)
        {
            var department = await DepartmentRepository.GetDepartmentById(Id, GetDepartmentRelation(relations));

            return Mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDto> UpdateDepartment(Guid id, UpdateDepartmentDto departmentDto)
        {
            var department = new Department
            {
                Id = id,
                Name = departmentDto.Name,
                IdRole = Guid.Parse(departmentDto.Role)
            };

            department = await DepartmentRepository.UpdateDepartment(department);

            return Mapper.Map<DepartmentDto>(department);
        }

        private List<string> GetDepartmentRelation(List<string>? relations)
        {
            var result = new List<string>();

            foreach (var relation in relations ?? [])
            {
                if (DepartmentRelation.ContainsKey(relation))
                {
                    result.Add(DepartmentRelation[relation]);
                }
            }

            return result;
        }

        public async Task<List<DepartmentDto>> GetAllDepartmentWithAnonymous(List<string> relations)
        {
            return Mapper.Map<List<DepartmentDto>>(await DepartmentRepository.GetAllDepartmentWithAnonymous(GetDepartmentRelation(relations)));
        }

        public async Task<List<DepartmentDto>> SearchDepartmentAsync(SearchDepartmentDto searchDepartmentDto)
        {
            var departments = await DepartmentRepository.SearchDepartmentAsync(searchDepartmentDto.Search);

            var departmentDtos = Mapper.Map<List<DepartmentDto>>(departments);

            return departmentDtos;
        }
    }
}