
using AutoMapper;
using back_dotnet.Exceptions;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs;
using back_dotnet.Models.DTOs.Role;
using back_dotnet.Utils;
using Newtonsoft.Json;

namespace back_dotnet.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public RoleService(IRoleRepository roleRepository, IMapper mapper, IWebHostEnvironment env)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _env = env;
        }
        public async Task<RoleResponseDto?> CreateAsync(CreateRoleDto requestRole)
        {
            try
            {
                var addedRole = await _roleRepository.CreateAsync(requestRole);
                return _mapper.Map<RoleResponseDto>(addedRole);
            }
            catch (DuplicateEntryException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RoleResponseDto?> UpdateAsync(Guid id, UpdateRoleDto updateRequestRole)
        {
            try
            {
                var updatedRole = await _roleRepository.UpdateAsync(id, updateRequestRole);
                return _mapper.Map<RoleResponseDto>(updatedRole);
            }
            catch (DuplicateEntryException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<RoleResponseDto>?> GetAllAsync()
        {
            try
            {
                string filePath = Path.Combine(_env.ContentRootPath, "seeds-id.json");
                List<string> seedsId = new List<string>();
                var roleResponse = _mapper.Map<List<RoleResponseDto>>(await _roleRepository.GetAllAsync());
                if (System.IO.File.Exists(filePath))
                {
                    string jsonString = System.IO.File.ReadAllText("seeds-id.json");
                    SeedsType seeds = JsonConvert.DeserializeObject<SeedsType>(jsonString.ToString());
                    seedsId.AddRange(seeds.Id);
                    foreach (var item in roleResponse)
                    {
                        if (seedsId.Contains(item.Id.ToString()))
                        {
                            item.IsSeed = true;
                        }
                    }
                }
                return roleResponse;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<RoleResponseDto?> GetByIdAsync(Guid id)
        {
            var roleModel = await _roleRepository.GetByIdAsync(id);
            if (roleModel == null) return null;
            return _mapper.Map<RoleResponseDto>(roleModel);
        }
        public async Task<RoleResponseDto?> DeleteAsync(Guid id)
        {
            var roleDeleted = await _roleRepository.DeleteAsync(id);
            if (roleDeleted == null) return null;
            return _mapper.Map<RoleResponseDto>(roleDeleted);
        }

        public async Task<List<RoleResponseDto>> SearchRoleAsync(SearchRoleDto searchRoleDto)
        {
            var roles = await _roleRepository.SearchRoleAsync(searchRoleDto.Search);

            var responseRoleDtos = roles.Select(r => new RoleResponseDto
            {
                Id = r.Id,
                Name = r.Name,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                Departments = r.Departments.Select(d => new DepartmentResponseOnRoleDto
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList(),
                Permissions = r.PermissionRoles.Select(per => new PermissionResponseOnRoleDto
                {
                    Id = per.IdPermissionNavigation.Id,
                    Name = per.IdPermissionNavigation.Name
                }).ToList(),
                IsSeed = false
            }).ToList();

            var data = await System.IO.File.ReadAllTextAsync("seeds-id.json");
            var seeds = JsonConvert.DeserializeObject<SeedsType>(data);

            if (seeds?.Id != null)
            {
                responseRoleDtos.ForEach(item =>
                {
                    item.IsSeed = seeds.Id.Contains(item.Id.ToString());
                });
            }

            return responseRoleDtos;

        }
    }
}
