using back_dotnet.Exceptions;
using back_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace back_dotnet.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HairunSiContext DbContext;
        public DepartmentRepository(HairunSiContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<Department> CreateDepartment(Department department)
        {
            var role = await DbContext.Roles.FirstOrDefaultAsync(r => r.Id == department.IdRole);
            if (role is null)
            {
                throw new HttpException(StatusCodes.Status404NotFound, "Le role n'existe pas");
            }

            try
            {
                var departmentCreated = new Department
                {
                    Name = department.Name,
                    IdRole = department.IdRole,
                    CreatedAt = new DateTime()
                };

                await DbContext.Departments.AddAsync(departmentCreated);
                await DbContext.SaveChangesAsync();

                return departmentCreated;
            }
            catch (DbUpdateException ex)
            {
                PostgresException? innerException = ex.InnerException as PostgresException;

                if (innerException is not null && innerException.SqlState == "23505")
                {
                    throw new HttpException(StatusCodes.Status409Conflict, "Le département existe déja");
                }

                throw;
            }
        }

        public async Task<Department?> DeleteDepartment(Guid id)
        {
            var department = await DbContext.Departments.Include(d => d.Posts).FirstOrDefaultAsync(d => d.Id == id);

            if (department is null)
            {
                throw new HttpException(StatusCodes.Status404NotFound, "Le département à supprimer n'existe pas");
            }

            var departmentAnonymous = await DbContext.Departments.FirstOrDefaultAsync(d => d.Name == "Anonyme");
            if (departmentAnonymous is not null)
            {
                foreach (var post in department.Posts)
                {
                    post.IdDepartment = departmentAnonymous.Id;
                }
            }

            DbContext.Remove(department);
            await DbContext.SaveChangesAsync();

            return department;
        }

        public async Task<List<Department>> GetAllDepartment(List<string> relations)
        {
            var query = DbContext.Departments.AsQueryable();

            foreach (var relation in relations)
            {
                query = query.Include(relation);
            }

            return await query.Where(d => d.Name != "Anonyme")
                              .OrderByDescending(d => d.CreatedAt)
                              .ToListAsync();
        }

        public async Task<Department?> GetDepartmentById(Guid Id, List<string>? relations = null)
        {
            var query = DbContext.Departments.AsQueryable();

            foreach (var relation in relations ?? [])
            {
                query = query.Include(relation);
            }

            return await query.FirstOrDefaultAsync(d => d.Id == Id);
        }

        public async Task<Department?> UpdateDepartment(Department department)
        {
            var role = await DbContext.Roles.FirstOrDefaultAsync(r => r.Id == department.IdRole);
            if (role is null)
            {
                throw new HttpException(StatusCodes.Status404NotFound, "Le role n'existe pas");
            }

            var departmentSaved = await GetDepartmentById(department.Id);
            if (department is null)
            {
                throw new HttpException(StatusCodes.Status404NotFound, "Le département n'existe pas");
            }

            try
            {
                departmentSaved.Name = department.Name;
                departmentSaved.IdRole = department.IdRole;
                departmentSaved.UpdatedAt = DateTime.Now;

                await DbContext.SaveChangesAsync();

                return await GetDepartmentById(departmentSaved.Id);
            }
            catch (DbUpdateException ex)
            {
                PostgresException? innerException = ex.InnerException as PostgresException;

                if (innerException is not null && innerException.SqlState == "23505")
                {
                    throw new HttpException(StatusCodes.Status409Conflict, "Le département existe déja");
                }

                throw;
            }
        }

        public async Task<List<Department>> GetAllDepartmentWithAnonymous(List<string> relations)
        {
            var query = DbContext.Departments.AsQueryable();
            foreach (var relation in relations)
                query = query.Include(relation);
            return await query.OrderByDescending(d => d.CreatedAt)
                              .ToListAsync();
        }

        public async Task<List<Department>> SearchDepartmentAsync(string search)
        {
            var searchLower = search.ToLower();
            var departments = await DbContext.Departments
                .Include(d => d.IdRoleNavigation)
                .Where(d => d.Name.ToLower().Contains(searchLower) || d.IdRoleNavigation.Name.ToLower().Contains(searchLower))
                .Where(d => d.Name != "Anonyme")
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();

            return departments;
        }
    }
}