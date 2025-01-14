
using AutoMapper;
using back_dotnet.Exceptions;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs;
using back_dotnet.Models.DTOs.Role;
using back_dotnet.Utils;
using Microsoft.EntityFrameworkCore;

namespace back_dotnet
{
    public class RoleRepository : IRoleRepository
    {
        private readonly HairunSiContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleRepository> _logger;
        public RoleRepository(HairunSiContext dbContext, IMapper mapper, ILogger<RoleRepository> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Role?> CreateAsync(CreateRoleDto requestRole)
        {
            try
            {
                requestRole.Name = UtilsMethod.CleanAndCapitalize(requestRole.Name);
                var roleModel = _mapper.Map<Role>(requestRole);
                await _dbContext.Roles.AddAsync(roleModel);
                await _dbContext.SaveChangesAsync();

                foreach (var idPermission in requestRole.Permissions.Distinct().ToList())
                {
                    var permissionExist = await _dbContext.Permissions.FirstOrDefaultAsync(item => item.Id == Guid.Parse(idPermission));
                    if (permissionExist != null)
                    {
                        var permissionRoleDomain = new PermissionRole()
                        {
                            IdRole = roleModel.Id,
                            IdPermission = Guid.Parse(idPermission)
                        };
                        await _dbContext.PermissionRoles.AddAsync(permissionRoleDomain);
                    }
                }
                await _dbContext.SaveChangesAsync();
                return roleModel;
            }
            catch (DbUpdateException ex) when (UtilsMethod.IsPostgresUniqueViolationError(ex))
            {
                throw new DuplicateEntryException("Le rôle existe déjà", ex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Role?>> GetAllAsync()
        {
            var rolesModel = await _dbContext.Roles
                .Include(r => r.PermissionRoles)
                    .ThenInclude(rp => rp.IdPermissionNavigation)
                .Include(r => r.Departments)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return rolesModel;
        }

        public async Task<Role?> DeleteAsync(Guid id)
        {
            var existingRole = await GetByIdAsync(id);
            if (existingRole == null) return null;
            _dbContext.Roles.Remove(existingRole);
            await _dbContext.SaveChangesAsync();
            return existingRole;
        }

        public async Task<Role?> GetByIdAsync(Guid id)
        {
            var roleModel = await _dbContext.Roles
                        .Include(rp => rp.Departments)
                        .Include(rp => rp.PermissionRoles)
                            .ThenInclude(p => p.IdPermissionNavigation)
                        .OrderByDescending(r => r.CreatedAt)
                        .FirstOrDefaultAsync(r => r.Id == id);
            return roleModel;
        }
        public async Task<Role?> UpdateAsync(Guid id, UpdateRoleDto updateRequestRole)
        {
            try
            {
                var roleModel = await _dbContext.Roles
                    .Include(r => r.PermissionRoles)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (roleModel == null) return null;

                roleModel.Name = UtilsMethod.CleanAndCapitalize(updateRequestRole.Name);
                if (updateRequestRole.Permissions.Count > 0)
                {
                    var updatedPermissionIds = updateRequestRole.Permissions.Select(Guid.Parse).Distinct().ToHashSet();
                    var rolesToRemove = roleModel.PermissionRoles
                        .Where(pr => !updatedPermissionIds.Contains(pr.IdPermission))
                        .ToList();

                    foreach (var roleToRemove in rolesToRemove)
                    {
                        roleModel.PermissionRoles.Remove(roleToRemove);
                    }
                    foreach (var permissionId in updatedPermissionIds)
                    {
                        if (!roleModel.PermissionRoles.Any(pr => pr.IdPermission == permissionId))
                        {
                            roleModel.PermissionRoles.Add(new PermissionRole { IdRole = id, IdPermission = permissionId });
                        }
                    }
                }
                await _dbContext.SaveChangesAsync();
                return await GetByIdAsync(id);
            }
            catch (DbUpdateException ex) when (UtilsMethod.IsPostgresUniqueViolationError(ex))
            {
                throw new DuplicateEntryException("Le rôle existe déjà", ex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Role>> SearchRoleAsync(string search)
        {
            var searchLower = search.ToLower();
            var roles = await _dbContext.Roles
                .Include(r => r.PermissionRoles)
                    .ThenInclude(per => per.IdPermissionNavigation)
                .Include(r => r.Departments)
                .Where(r => r.PermissionRoles
                    .Any(per => per.IdPermissionNavigation.Name.ToLower().Contains(searchLower))
                    || r.Name.ToLower().Contains(searchLower))
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return roles;
        }
    }
}
