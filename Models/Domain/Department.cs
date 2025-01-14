using System;
using System.Collections.Generic;

namespace back_dotnet.Models.Domain;

public partial class Department
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid IdRole { get; set; }

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public static implicit operator Guid(Department v)
    {
        throw new NotImplementedException();
    }
}
