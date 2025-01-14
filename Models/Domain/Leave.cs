using System.ComponentModel.DataAnnotations;

namespace back_dotnet.Models.Domain;

public partial class Leave
{
    [Key]
    public Guid Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public LeaveStatus Status { get; set; }

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // UN CONGE EST ASSOCIE AVEC UN ET UN SEUL UTILISATEUR
    public Guid IdUser { get; set; }
    public User User { get; set; } = null!;

    // UN CONGE POSSEDE UN TYPE (Payé, maladie, maternité,...)
    public Guid IdLeaveType { get; set; }
    public LeaveType Type { get; set; } = null!;
}