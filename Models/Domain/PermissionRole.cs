using System;
using System.Collections.Generic;

namespace back_dotnet.Models.Domain;

public partial class PermissionRole
{
    public Guid IdPermission { get; set; }

    public Guid IdRole { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Permission IdPermissionNavigation { get; set; } = null!;

    public virtual Role IdRoleNavigation { get; set; } = null!;
}
