using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using back_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace back_dotnet.Seeds
{
    public class SuperAdminSeeder
    {
        public static async Task SeedAsync(HairunSiContext context)
        {
            try
            {
                var jsonData = await System.IO.File.ReadAllTextAsync("seeds-id.json");
                var seedIds = JsonSerializer.Deserialize<SeedData>(jsonData);

                if (seedIds?.Id == null || !seedIds.Id.Any())
                {
                    Console.WriteLine($"Data from seeds-id.json: {jsonData}");
                    throw new Exception("Les données dans le seeds-id.json son introuvables.");
                }

                var superAdminId = Guid.Parse(seedIds.Id[0]);

                var userRepository = context.Users;
                var user = await userRepository.FirstOrDefaultAsync(u => u.Email == "test@hairun-technology.com");

                if (user == null)
                {
                    var permissionRepository = context.Permissions;
                    var permissions = new[]
                    {
                    await GetOrCreatePermission(permissionRepository, "Accès tout utilisateur"),
                    await GetOrCreatePermission(permissionRepository, "Création tout utilisateur"),
                    await GetOrCreatePermission(permissionRepository, "Modification tout utilisateur"),
                    await GetOrCreatePermission(permissionRepository, "Suppression tout utilisateur"),
                    await GetOrCreatePermission(permissionRepository, "Validation demande congé")
                };

                    var roleRepository = context.Roles;
                    var role = await roleRepository.FirstOrDefaultAsync(r => r.Name == "Super Admin");

                    if (role == null)
                    {
                        role = new Role
                        {
                            Name = "Super Admin",
                            CreatedAt = GetDateTimeNowWithoutTimezone(),
                            PermissionRoles = new List<PermissionRole>()
                        };

                        foreach (var permission in permissions)
                        {
                            var permissionRole = new PermissionRole
                            {
                                IdPermission = permission.Id,
                                IdRole = role.Id,
                                CreatedAt = GetDateTimeNowWithoutTimezone(),
                                UpdatedAt = GetDateTimeNowWithoutTimezone()
                            };

                            role.PermissionRoles.Add(permissionRole);
                        }

                        await roleRepository.AddAsync(role);
                        await context.SaveChangesAsync();
                    }

                    var departmentRepository = context.Departments;
                    var department = await departmentRepository.FirstOrDefaultAsync(d => d.Name == "Direction");

                    if (department == null)
                    {
                        department = new Department
                        {
                            Id = Guid.NewGuid(),
                            Name = "Direction",
                            IdRoleNavigation = role,
                            CreatedAt = GetDateTimeNowWithoutTimezone(),
                            UpdatedAt = GetDateTimeNowWithoutTimezone()
                        };
                        await departmentRepository.AddAsync(department);
                        await context.SaveChangesAsync();
                    }

                    var postRepository = context.Posts;
                    var post = await postRepository.FirstOrDefaultAsync(p => p.Name == "Directeur");

                    if (post == null)
                    {
                        post = new Post
                        {
                            Id = Guid.NewGuid(),
                            Name = "Directeur",
                            IdDepartment = department.Id,
                            CreatedAt = GetDateTimeNowWithoutTimezone(),
                            UpdatedAt = GetDateTimeNowWithoutTimezone()
                        };
                        await postRepository.AddAsync(post);
                        await context.SaveChangesAsync();
                    }

                    var fileRepository = context.Files;
                    var file = await fileRepository.FirstOrDefaultAsync(f => f.Name == "hairun_admin");

                    if (file == null)
                    {
                        file = new Models.Domain.File
                        {
                            Id = Guid.NewGuid(),
                            Name = "hairun_admin",
                            Path = "https://res.cloudinary.com/daeuzi9pb/image/upload/v1735561237/uploads/johndoe_rseb5e.webp",
                            PublicId = "123456",
                            Size = "160",
                            Type = "png"
                        };
                        await fileRepository.AddAsync(file);
                        await context.SaveChangesAsync();
                    }

                    var password = BCrypt.Net.BCrypt.HashPassword("hairunM@2024*");

                    var newUser = new User
                    {
                        Uuid = superAdminId,
                        Firstname = "hairun",
                        Lastname = "hairun",
                        BirthDate = GetDateTimeNowWithoutTimezone(),
                        Email = "test@hairun-technology.com",
                        Password = password,
                        IdPost = post.Id,
                        IdFile = file.Id,
                        CreatedAt = GetDateTimeNowWithoutTimezone(),
                        UpdatedAt = GetDateTimeNowWithoutTimezone()
                    };

                    await userRepository.AddAsync(newUser);
                    await context.SaveChangesAsync();

                    Console.WriteLine("Super Admin ajout avec succès en utilisant ID de seeds-id.json.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur est survenue lors du seeding des données: {ex.Message}");
                throw;
            }

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
