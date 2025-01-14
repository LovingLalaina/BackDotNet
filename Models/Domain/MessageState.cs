
namespace back_dotnet.Models.Domain;

public partial class MessageState
{
    public MessageState()
    {
        // Nécessaire pour Entity-Framework Core à cause de
        // l'existance d'un autre constructeur
    }

    public MessageState( Guid idMessage, Guid idUser, bool isRead )
    {
        IdMessage = idMessage;
        IdUser = idUser;
        IsRead = isRead;
        ReadAt = isRead ? DateTime.Now : null;
    }

    public Guid IdMessage { get; set; }

    public Guid IdUser { get; set; }

    public bool IsRead { get; set; } = false;

    public DateTime? ReadAt { get; set; }

    public virtual Message Message { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
