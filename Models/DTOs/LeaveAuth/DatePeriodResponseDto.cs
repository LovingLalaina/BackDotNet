
using System.Text.Json.Serialization;

namespace back_dotnet.Models.DTOs.LeaveAuth;

public class DatePeriodResponseDto
{
    [JsonPropertyName("date_period")]
    public string DatePeriod { get; set; } = null!; 

    [JsonPropertyName("start_validity")]
    public DateTime StartValidity { get; set; }

    [JsonPropertyName("end_validity")]
    public DateTime EndValidity { get; set; }
}
