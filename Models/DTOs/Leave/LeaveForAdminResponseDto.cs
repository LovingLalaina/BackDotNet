using System.Text.Json.Serialization;
using back_dotnet.Models.Domain;

namespace back_dotnet.Models.DTOs.Leave;

public class LeaveForAdminResponseDto
{
    [JsonPropertyName("id_leave")]
    public Guid Id { get; set; }

    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public DateTime EndDate { get; set; }

    public string Matricule { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public decimal Duration { get; set; }

    public decimal Solde { get; set; }

    public string Type { get; set; } = null!;

    public LeaveStatus Status { get; set; }

    public string Description { get; set; } = null!;
}