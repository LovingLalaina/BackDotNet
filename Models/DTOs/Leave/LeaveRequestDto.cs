
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace back_dotnet.Models.DTOs.Leave;

public class LeaveRequestDto
{
    [Required(ErrorMessage = "L'identifiant de l'utilisateur demandeur n'est pas fourni")]
    [JsonPropertyName("id_user")]
    public Guid IdUser { get; set; }

    [Required(ErrorMessage = "L'identifiant du type de congé n'est pas fourni")]
    [JsonPropertyName("id_leave_type")]
    public Guid IdLeaveType { get; set; }

    [Required(ErrorMessage = "La date de début du congé est requise")]
    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "La date de fin du congé est requise")]
    [JsonPropertyName("end_date")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "La description du congé est requise")]
    [Length(1, 255, ErrorMessage = "La description est trop longue")]
    public string Description { get; set; } = null!;

    //UTIL PLUS TARD DANS LeaveService.CheckLeaveRequest()
    [BindNever]
    [JsonIgnore]
    public decimal? ActualSolde { get; set; }

    public bool ContainsDateOrTypeChanges(Domain.Leave oldLeaveRequest)
    {
        return IdLeaveType != oldLeaveRequest.IdLeaveType || StartDate != oldLeaveRequest.StartDate || EndDate != oldLeaveRequest.EndDate;
    }
}