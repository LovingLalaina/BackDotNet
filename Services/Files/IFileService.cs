using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_dotnet.Models.DTOs.Files;

namespace back_dotnet.Services.Files
{
    public interface IFileService
    {
        Task<List<GetFileDto>?> GetAllAsync();
        Task<GetFileDto?> GetByIdAsync(Guid uuid);
        Task<GetFileDto?> CreateAsync(UploadImageDto fileDto);
        Task<GetFileDto?> DeleteAsync(Guid uuis);
        Task<GetFileDto?> UpdateAsync(Guid id, UploadImageDto fileDto);
    }
}