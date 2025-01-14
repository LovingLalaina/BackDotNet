namespace back_dotnet.Models.DTOs.Leave;

public class SearchLeaveDateDto
{
    public bool? IsNowSelected { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}