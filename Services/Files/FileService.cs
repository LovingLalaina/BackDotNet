using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using back_dotnet.Exceptions;
using back_dotnet.Models.DTOs.Files;
using back_dotnet.Repositories.Files;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using File = back_dotnet.Models.Domain.File;

namespace back_dotnet.Services.Files
{
  public class FileService : IFileService
  {
    private readonly IFileRepository fileRepository;
    private readonly IMapper mapper;
    private readonly IConfiguration configuration;
    private Cloudinary cloudinary;

    public FileService(IFileRepository fileRepository, IMapper mapper, IConfiguration configuration)
    {
      this.fileRepository = fileRepository;
      this.mapper = mapper;
      this.configuration = configuration;
      cloudinary = new($"cloudinary://{configuration["Cloudinary:CLOUD_KEY"]}:{configuration["Cloudinary:CLOUD_SECRET"]}@{configuration["Cloudinary:CLOUD_NAME"]}");
    }

    public async Task<GetFileDto?> CreateAsync(UploadImageDto imageDto)
    {
      try
      {
        if(imageDto.File != null && ValidateImageExtension(imageDto) == true && ValidateImageSize(imageDto) == true)
        {
          var uploadResult = await UploadImageToCloudinary(imageDto);

          var image = new CreateFileDto
          {
            Name = $"{DateTime.Now}-{imageDto.File?.FileName}",
            Path = uploadResult.SecureUrl.ToString(),
            Type = GetTypeFile(uploadResult.ResourceType),
            Size = imageDto.File?.Length.ToString(),
            PublicId = uploadResult.PublicId,
          };
          var file = await fileRepository.Create(mapper.Map<File>(image));
          return file;
        }
        else throw new HttpException(406, "Fichier non supporté");
      }
      catch (System.Exception)
      {
        throw new HttpException(406, "Une erreur est survenue lors de la création du fichier");
      }
    }

    public async Task<List<GetFileDto>?> GetAllAsync()
    {
      try
      {
        return mapper.Map<List<GetFileDto>>(await fileRepository.GetAll());
      }
      catch (System.Exception)
      {
        
        throw new HttpException(406, "Une erreur est survenue lors du chargement des fichiers");
      }
    }

    public async Task<GetFileDto?> GetByIdAsync(Guid uuid)
    {
      try
      {
        return mapper.Map<GetFileDto?>(await fileRepository.GetById(uuid));
      }
      catch (System.Exception)
      {
        throw new HttpException(404, "Le fichier est introuvable");
      }
    }

    public async Task<GetFileDto?> DeleteAsync(Guid uuid)
    {
      try
      {
        var file = await GetByIdAsync(uuid);
      
        if(file is not null) 
        {
          
          await DeleteImageFromCloudinary(file.PublicId);
          return await fileRepository.Delete(mapper.Map<File>(file));
        }
        else throw new HttpException(404, "Fichier introuvable");
      }
      catch (System.Exception)
      {
        
        throw new HttpException(404, "Le fichier est introuvable");
      }
    }


    public async Task<GetFileDto?> UpdateAsync(Guid id, UploadImageDto fileDto)
    {
      try
      {
        if(fileDto.File != null && ValidateImageExtension(fileDto) == true && ValidateImageSize(fileDto) == true)
        {
          var file = await GetByIdAsync(id);
          if(file == null) return null;
          await DeleteImageFromCloudinary(file.PublicId);
          var uploadResult = await UploadImageToCloudinary(fileDto);
          var fileUpdated = await fileRepository.Update(
              id, 
              uploadResult.SecureUrl.ToString(), 
              fileDto.File.Length.ToString(), 
              $"{DateTime.Now}-{fileDto.File?.FileName}",
              uploadResult.PublicId
          );
          return fileUpdated;
        }
        else throw new HttpException(400, "Fichier non supporté");
      }
      catch (Exception)
      {
        throw new HttpException(406, "Une erreur est survenue lors de la modification du fichier");
      }
    }

    private async Task<ImageUploadResult> UploadImageToCloudinary(UploadImageDto imageDto)
    {
      cloudinary.Api.Secure = true;
      using var stream = imageDto.File?.OpenReadStream();
      var uploadParams = new ImageUploadParams()
      {
          File = new FileDescription(imageDto.File?.FileName, stream),
          Folder = "uploads",
          UseFilename = true,
          UniqueFilename = true,
          Overwrite = false
      };
      return await cloudinary.UploadAsync(uploadParams);
    }
    
    private async Task<DeletionResult> DeleteImageFromCloudinary(string publicId)
    {
      var id = publicId;
      var deletionParams = new DeletionParams(publicId : publicId)
      {
        Invalidate = true,
        PublicId = publicId,
        ResourceType = ResourceType.Image,
        Type = "upload"
      };
      return await cloudinary.DestroyAsync(deletionParams);

    }

    private static string GetTypeFile(string mimetype)
    {
      var type = "";
      switch (mimetype)
      {
        case "image":
          type = "avatar";
          break;
        case "text":
          type = "document";
          break;
        default:
          break;
      }
      return type;
    }
    
    private static bool ValidateImageExtension(UploadImageDto createImageDto)
    {
      var extensions = new string[] { ".png", ".jpg", ".jpeg", ".svg" };
      if(!extensions.Contains(Path.GetExtension(createImageDto.File?.FileName))) return false;
      return true;
    }

    private static bool ValidateImageSize(UploadImageDto createImageDto)
    {
      if(createImageDto.File?.Length > 500000) return false;
      return true;
    }

  }
}