

namespace back_dotnet.Models.DTOs.Post;

public class SearchPostDto
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DepartmentForPostDto Department { get; set; } = null!;
}