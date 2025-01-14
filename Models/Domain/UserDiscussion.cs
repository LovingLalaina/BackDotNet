using System;
using System.Collections.Generic;

namespace back_dotnet.Models.Domain;

public partial class UserDiscussion
{
    public UserDiscussion()
    {
        // Nécessaire pour Entity-Framework Core à cause de
        // l'existance d'un autre constructeur
    }

    public UserDiscussion( Guid idUser, Guid idDiscussion )
    {
        IdUser = idUser;
        IdDiscussion = idDiscussion;
        CreatedAt = DateTime.Now;
    }

    public Guid IdUser { get; set; }

    public virtual User User { get; set; } = null!;

    public Guid IdDiscussion { get; set; }

    public virtual Discussion Discussion { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
