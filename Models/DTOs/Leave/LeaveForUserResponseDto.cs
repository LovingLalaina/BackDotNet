using System.Text.Json.Serialization;
using back_dotnet.Models.Domain;

namespace back_dotnet.Models.DTOs.Leave;

public class LeaveForUserResponseDto
{
    [JsonPropertyName("id_leave")]
    public Guid Id { get; set; }

    [JsonPropertyName("date_period")]
    public string DatePeriod { get; set; } = null!;

    public string Type { get; set; } = null!;

    [JsonPropertyName("solde_for_type")]
    public decimal SoldeForType { get; set; }

    public decimal Duration { get; set; }

    public LeaveStatus Status { get; set; }

    public string Description { get; set; } = null!;
}