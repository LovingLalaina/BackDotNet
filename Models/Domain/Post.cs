using System;
using System.Collections.Generic;

namespace back_dotnet.Models.Domain;

public partial class Post
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid IdDepartment { get; set; }

    public virtual Department IdDepartmentNavigation { get; set; } = null!;
}
