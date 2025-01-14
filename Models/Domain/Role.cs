using System;
using System.Collections.Generic;

namespace back_dotnet.Models.Domain;

public partial class Role
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<PermissionRole> PermissionRoles { get; set; } = new List<PermissionRole>();
}
