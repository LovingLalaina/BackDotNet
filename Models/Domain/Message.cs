
namespace back_dotnet.Models.Domain;

public partial class Message
{
    public Guid Id { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public Guid IdUser { get; set; }

    public virtual User User { get; set; } = null!;

    public Guid IdDiscussion { get; set; }
    
    public virtual Discussion Discussion { get; set; } = null!;

    public virtual ICollection<MessageState> MessageStates { get; set; } = new List<MessageState>();
}
