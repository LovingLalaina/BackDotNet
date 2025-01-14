using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_dotnet.Models.DTOs.Files
{
    public class UploadImageDto
    {
        [Required]
        public IFormFile? File { get; set; }
    }
}