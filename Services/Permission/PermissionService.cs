using AutoMapper;
using back_dotnet.Exceptions;
using back_dotnet.Models.DTOs.Permission;

namespace back_dotnet.Services.Permission
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public PermissionService(IPermissionRepository permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<PermissionResponseDto?> CreateAsync(CreateOrUpdatePermissionDto requestPermission)
        {
            try
            {
                var addedPermission = await _permissionRepository.CreateAsync(requestPermission);
                return _mapper.Map<PermissionResponseDto>(addedPermission);
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

        public async Task<PermissionResponseDto?> DeleteAsync(Guid id)
        {
            try
            {
                var permissionDeleted = await _permissionRepository.DeleteAsync(id);
                if (permissionDeleted == null) return null;
                return _mapper.Map<PermissionResponseDto>(permissionDeleted);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PermissionResponseDto>?> GetAllAsync()
        {
            try
            {
                var permissionsModel = await _permissionRepository.GetAllAsync();
                return _mapper.Map<List<PermissionResponseDto>>(permissionsModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PermissionResponseDto?> GetByIdAsync(Guid id)
        {
            var permissionModel = await _permissionRepository.GetByIdAsync(id);
            if (permissionModel == null) return null;
            return _mapper.Map<PermissionResponseDto>(permissionModel);
        }

        public async Task<PermissionResponseDto?> UpdateAsync(Guid id, CreateOrUpdatePermissionDto updateRequestPermission)
        {
            try
            {
                var updatedPermission = await _permissionRepository.UpdateAsync(id, updateRequestPermission);
                return _mapper.Map<PermissionResponseDto>(updatedPermission);
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
    }
}
