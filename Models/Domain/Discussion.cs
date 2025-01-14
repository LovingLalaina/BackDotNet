
using System.ComponentModel.DataAnnotations.Schema;
using back_dotnet.Models.DTOs.Chat;

namespace back_dotnet.Models.Domain;

public partial class Discussion
{
    public Discussion()
    {
        // Nécessaire pour Entity-Framework Core à cause de
        // l'existance d'un autre constructeur
    }

    public Discussion(StartDiscussionDto startDiscussion)
    {
        Title = startDiscussion.IdUserSender + "/" + startDiscussion.IdUserReceiver;
        CreatedAt = DateTime.Now;
        ParticipantNumber = 2;
    }

    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int ParticipantNumber { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<UserDiscussion> UserDiscussions { get; set; } = new List<UserDiscussion>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    [NotMapped]
    public string Avatar { get; set; } = ""; // Champ ignoré par EF Core
}
