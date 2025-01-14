using AutoMapper;
using back_dotnet.Exceptions;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Permission;
using back_dotnet.Utils;
using Microsoft.EntityFrameworkCore;

namespace back_dotnet.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly HairunSiContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<PermissionRepository> _logger;

        public PermissionRepository(HairunSiContext dbContext, IMapper mapper, ILogger<PermissionRepository> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Permission?> CreateAsync(CreateOrUpdatePermissionDto requestPermission)
        {
            try
            {
                requestPermission.Name = UtilsMethod.CleanAndCapitalizeFirstLetter(requestPermission.Name);
                var permissionModel = _mapper.Map<Permission>(requestPermission);
                await _dbContext.Permissions.AddAsync(permissionModel);
                await _dbContext.SaveChangesAsync();
                return permissionModel;
            }
            catch (DbUpdateException ex) when (UtilsMethod.IsPostgresUniqueViolationError(ex))
            {
                throw new DuplicateEntryException("Cette permission existe déjà", ex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Permission?> DeleteAsync(Guid id)
        {
            var existingPermission = await GetByIdAsync(id);
            if (existingPermission == null) return null;
            _dbContext.Permissions.Remove(existingPermission);
            await _dbContext.SaveChangesAsync();
            return existingPermission;
        }

        public async Task<List<Permission?>> GetAllAsync()
        {
            var permissionsModel = await _dbContext.Permissions
                    .Include(rp => rp.PermissionRoles)
                    .ThenInclude(r => r.IdRoleNavigation)
                    .ToListAsync();
            return permissionsModel;
        }

        public async Task<Permission?> GetByIdAsync(Guid id)
        {
            var permissionModel = await _dbContext.Permissions
                    .Include(rp => rp.PermissionRoles)
                    .ThenInclude(r => r.IdRoleNavigation).FirstOrDefaultAsync(p => p.Id == id);
            return permissionModel;
        }

        public async Task<Permission?> UpdateAsync(Guid id, CreateOrUpdatePermissionDto updateRequestPermission)
        {
            try
            {
                var permissionModel = await GetByIdAsync(id);
                if (permissionModel == null) return null;
                permissionModel.Name = UtilsMethod.CleanAndCapitalizeFirstLetter(updateRequestPermission.Name);
                await _dbContext.SaveChangesAsync();
                return permissionModel;
            }
            catch (DbUpdateException ex) when (UtilsMethod.IsPostgresUniqueViolationError(ex))
            {
                throw new DuplicateEntryException("Cette permission existe déjà", ex);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
