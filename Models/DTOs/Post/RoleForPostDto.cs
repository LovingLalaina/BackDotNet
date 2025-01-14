
namespace back_dotnet.Models.DTOs.Post;

public class RoleForPostDto
{

    public RoleForPostDto(Domain.Role role)
    {
        Id = role.Id.ToString();
        Name = role.Name;
    }

    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;
}