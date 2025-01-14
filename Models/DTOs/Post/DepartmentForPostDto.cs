
namespace back_dotnet.Models.DTOs.Post;

public class DepartmentForPostDto
{
    public DepartmentForPostDto(Domain.Department department)
    {
        Id = department.Id.ToString();
        Name = department.Name;
        Role = new RoleForPostDto( department.IdRoleNavigation );
    }

    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public RoleForPostDto Role { get; set; } = null!;
}