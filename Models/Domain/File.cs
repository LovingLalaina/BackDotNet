using System;
using System.Collections.Generic;

namespace back_dotnet.Models.Domain;

public partial class File
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Path { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Size { get; set; }

    public string PublicId { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
