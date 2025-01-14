using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using back_dotnet.Exceptions;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Files;
using Microsoft.EntityFrameworkCore;
using File = back_dotnet.Models.Domain.File;

namespace back_dotnet.Repositories.Files
{
  public class FileRepository : IFileRepository
  {
    private readonly HairunSiContext context;

    private readonly IMapper mapper;
    public FileRepository(HairunSiContext context, IMapper mapper)
    {
      this.context = context;
      this.mapper = mapper;
    }

    public async Task<GetFileDto?> Create(File file)
    {
      await context.Files.AddAsync(file); 
      await context.SaveChangesAsync();
      return mapper.Map<GetFileDto>(file);  
    }

    public async Task<GetFileDto?> Delete(File file)
    {
      try
      {
        context.Files.Remove(file);
        await context.SaveChangesAsync();
        
        return mapper.Map<GetFileDto>(file);
      }
      catch (System.Exception)
      {
        
        throw new HttpException(404, "Le fichier est introuvable");
      }
    }

    public async Task<List<GetFileDto>?> GetAll()
    {
      return mapper.Map<List<GetFileDto>>(await context.Files.ToListAsync());
    }

    public async Task<File?> GetById(Guid uuid)
    {
      try
      {
        return await context.Files.FirstOrDefaultAsync(file => file.Id == uuid);
      }
      catch (System.Exception)
      {
        
        throw new HttpException(404, "Fichier introuvable");
      }
    }

    public async Task<GetFileDto?> Update(Guid uuid, string path, string size, string name, string publicId)
    {
      File? file = await GetById(uuid);
      if(file == null) return null;
      file.Path = path;
      file.Name = name;
      file.Size = size;
      file.PublicId = publicId;
      file.UpdatedAt = DateTime.Now;
      await context.SaveChangesAsync();
      return mapper.Map<GetFileDto>(file);
    }
  }
}