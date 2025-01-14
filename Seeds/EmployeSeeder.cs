using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using back_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace back_dotnet.Seeds
{
    public class EmployeSeeder
    {
        public static async Task SeedEmployeAsync(HairunSiContext context)
        {
            // try
            // {
            //     var jsonData = await System.IO.File.ReadAllTextAsync("seeds-id.json");
            //     var seedIds = JsonSerializer.Deserialize<SeedData>(jsonData);

            //     if (seedIds?.Id == null || !seedIds.Id.Any())
            //     {
            //         throw new Exception("Les données dans le seeds-id.json son introuvables.");
            //     }

            //     var employeId = Guid.Parse(seedIds.Id[1]);

            //     var userRepository = context.Users;
            //     var user = await userRepository.FirstOrDefaultAsync(u => u.Email == "test-simple-user@hairun-technology.com");

            //     if (user == null)
            //     {
            //         var permissionRepository = context.Permissions;
            //         var permissions = new[]
            //         {
            //             await GetOrCreatePermission(permissionRepository, "Accès utilisateur"),
            //             await GetOrCreatePermission(permissionRepository, "Modification utilisateur"),
            //         };

            //         var roleRepository = context.Roles;
            //         var role = await roleRepository.FirstOrDefaultAsync(r => r.Name == "Employé");

            //         if (role == null)
            //         {
            //             role = new Role
            //             {
            //                 Name = "Employé",
            //                 CreatedAt = GetDateTimeNowWithoutTimezone(),
            //                 PermissionRoles = new List<PermissionRole>()
            //             };

            //             foreach (var permission in permissions)
            //             {
            //                 var permissionRole = new PermissionRole
            //                 {
            //                     IdPermission = permission.Id,
            //                     IdRole = role.Id,
            //                     CreatedAt = GetDateTimeNowWithoutTimezone(),
            //                     UpdatedAt = GetDateTimeNowWithoutTimezone()
            //                 };

            //                 role.PermissionRoles.Add(permissionRole);
            //             }

            //             await roleRepository.AddAsync(role);
            //             await context.SaveChangesAsync();
            //         }

            //         var departmentRepository = context.Departments;
            //         var department = await departmentRepository.FirstOrDefaultAsync(d => d.Name == "Production");

            //         if (department == null)
            //         {
            //             department = new Department
            //             {
            //                 Id = Guid.NewGuid(),
            //                 Name = "Production",
            //                 IdRoleNavigation = role,
            //                 CreatedAt = GetDateTimeNowWithoutTimezone(),
            //                 UpdatedAt = GetDateTimeNowWithoutTimezone()
            //             };
            //             await departmentRepository.AddAsync(department);
            //             await context.SaveChangesAsync();
            //         }

            //         var postRepository = context.Posts;
            //         var post = await postRepository.FirstOrDefaultAsync(p => p.Name == "Intégrateur");

            //         if (post == null)
            //         {
            //             post = new Post
            //             {
            //                 Id = Guid.NewGuid(),
            //                 Name = "Intégrateur",
            //                 IdDepartment = department.Id,
            //                 CreatedAt = GetDateTimeNowWithoutTimezone(),
            //                 UpdatedAt = GetDateTimeNowWithoutTimezone()
            //             };
            //             await postRepository.AddAsync(post);
            //             await context.SaveChangesAsync();
            //         }

            //         var fileRepository = context.Files;
            //         var file = await fileRepository.FirstOrDefaultAsync(f => f.Name == "hairun_simple_user");

            //         if (file == null)
            //         {
            //             file = new Models.Domain.File
            //             {
            //                 Id = Guid.NewGuid(),
            //                 Name = "hairun_simple_user",
            //                 Path = "https://res.cloudinary.com/daeuzi9pb/image/upload/v1736163619/uploads/profil_qsh9sh.jpg",
            //                 PublicId = "id_simple_user",
            //                 Size = "160",
            //                 Type = "png"
            //             };
            //             await fileRepository.AddAsync(file);
            //             await context.SaveChangesAsync();
            //         }

            //         var password = BCrypt.Net.BCrypt.HashPassword("SimpleUser@2024*");

            //         var newUser = new User
            //         {
            //             Uuid = employeId,
            //             Firstname = "hairun si user",
            //             Lastname = "hairun si user",
            //             BirthDate = GetDateTimeNowWithoutTimezone(),
            //             Email = "test-simple-user@hairun-technology.com",
            //             Password = password,
            //             IdPost = post.Id,
            //             IdFile = file.Id,
            //             CreatedAt = GetDateTimeNowWithoutTimezone(),
            //             UpdatedAt = GetDateTimeNowWithoutTimezone()
            //         };
            //         await userRepository.AddAsync(newUser);
            //         await context.SaveChangesAsync();

            //     }

            //     var departmentRepositoryAnonym = context.Departments;
            //     var departementAnonyme = await departmentRepositoryAnonym.FirstOrDefaultAsync(da => da.Name == "Anonyme");

            //     if (departementAnonyme == null)
            //     {
            //         var roleRepository = context.Roles;
            //         var role = await roleRepository.FirstOrDefaultAsync(r => r.Name == "Employé");
            //         departementAnonyme = new Department
            //         {
            //             Id = Guid.NewGuid(),
            //             Name = "Anonyme",
            //             IdRoleNavigation = role,
            //             CreatedAt = GetDateTimeNowWithoutTimezone(),
            //             UpdatedAt = GetDateTimeNowWithoutTimezone()
            //         };
            //         await departmentRepositoryAnonym.AddAsync(departementAnonyme);
            //         await context.SaveChangesAsync();
            //     }

            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine($"Une erreur est survenue lors du seeding des données: {ex.Message}");
            //     throw;
            // }
        }

        private static async Task<Permission> GetOrCreatePermission(DbSet<Permission> repository, string permissionName)
        {
            var permission = await repository.FirstOrDefaultAsync(p => p.Name == permissionName);
            if (permission == null)
            {
                permission = new Permission
                {
                    Id = Guid.NewGuid(),
                    Name = permissionName,
                    CreatedAt = GetDateTimeNowWithoutTimezone(),
                    UpdatedAt = GetDateTimeNowWithoutTimezone()
                };
                await repository.AddAsync(permission);
            }
            return permission;
        }

        private static DateTime GetDateTimeNowWithoutTimezone()
        {
            var now = DateTime.UtcNow;
            return DateTime.SpecifyKind(now, DateTimeKind.Unspecified);
        }
        private class SeedData
        {
            [JsonPropertyName("id")]
            public List<string>? Id { get; set; }
        }
    }
}