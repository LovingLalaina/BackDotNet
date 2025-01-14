
namespace back_dotnet.Models.Domain;

public partial class LeaveAuthorization
{
    public decimal Solde { get; set; }

    public DateTime StartValidity { get; set; }

    public DateTime EndValidity { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // UN SOLDE EST AFFECTE A UN UTILISATEUR POUR UN TYPE DE CONGE ET RENOUVELE APRES EndValidity
    public Guid IdUser { get; set; }
    public User User { get; set; } = null!;

    public Guid IdLeaveType { get; set; }
    public LeaveType LeaveType { get; set; } = null!;
}