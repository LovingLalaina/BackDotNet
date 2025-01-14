using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Files;
using File = back_dotnet.Models.Domain.File;

namespace back_dotnet.Repositories.Files
{
    public interface IFileRepository
    {
        Task<List<GetFileDto>?> GetAll();
        Task<File?> GetById(Guid uuid);
        Task<GetFileDto?> Create(File file);
        Task<GetFileDto?> Delete(File file);
        Task<GetFileDto?> Update(Guid uuid, string path, string size, string name, string publicId);
    }
}