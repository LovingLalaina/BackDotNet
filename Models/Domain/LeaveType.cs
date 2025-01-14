using System.ComponentModel.DataAnnotations;

namespace back_dotnet.Models.Domain;

public partial class LeaveType
{
    [Key]
    public Guid Id { get; set; }

    public string Designation { get; set; } = null!;

    public Boolean IsCumulable { get; set; }

    public decimal SoldeEachYear { get; set; }

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<LeaveAuthorization> LeavesAuthorization { get; set; } = new List<LeaveAuthorization>();

    public static decimal MonthBalance = 2.5M;
}