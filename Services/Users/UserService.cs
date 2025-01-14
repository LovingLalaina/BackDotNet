using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using back_dotnet.Exceptions;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Files;
using back_dotnet.Models.DTOs.Users;
using back_dotnet.Repositories.Leave;
using back_dotnet.Repositories.Users;
using back_dotnet.Services.Auth;
using back_dotnet.Services.Files;
using back_dotnet.Services.Password;
using BCrypt.Net;

namespace back_dotnet.Services.Users
{
  public class UserService : IUserService
  {
    private const int BCRYPT_SALT = 10;
    
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;
    private readonly IFileService fileService;
    private readonly IPostService postService;
    private readonly IAuthService authService;
    private readonly IPasswordService passwordService;

    private readonly ILeaveRepository leaveRepository;

    public UserService(IUserRepository userRepository, IMapper mapper, IFileService fileService, IPostService postService, IAuthService authService, IPasswordService passwordService, ILeaveRepository leaveRepository )
    {
      this.userRepository = userRepository;
      this.mapper = mapper;
      this.fileService = fileService;
      this.postService = postService;
      this.authService = authService;
      this.passwordService = passwordService;
      this.leaveRepository = leaveRepository;
    }

    public async Task<GetUserDto?> CreateAsync(CreateUserDto createUserDto)
    {
        
      if(await UserWithThisEmailAsync(createUserDto.Email) == true){
        throw new HttpException(409, "Email non disponible");
      }
      var file = await fileService.CreateAsync(
        new UploadImageDto{
          File = createUserDto.File
        }
      );
      if(file != null){
        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
        var password = passwordService.GeneratePassword(); 
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(BCRYPT_SALT));
        var post = postService.GetPostById(createUserDto.Post);
        if(post == null) throw new HttpException(404, "Auucun poste correspondant");
        var user = new User{
          Firstname = Regex.Replace(createUserDto.Firstname.ToString().Trim(), @"\s+", " ").ToUpper(),
          Lastname = ti.ToTitleCase(Regex.Replace(createUserDto.Lastname.ToString().Trim(), @"\s+", " ")),
          BirthDate = createUserDto.BirthDate,
          Email = createUserDto.Email,
          Password = hashedPassword,
          IdFile = file.Id,
          IdPost = createUserDto.Post
        };
        var createdUser = await userRepository.Create(mapper.Map<User>(user));
        await authService.SendNotificationPassword( user.Firstname, user.Email, password );
        await leaveRepository.AffectLeave( user.Uuid );
        if(createdUser is not null) return createdUser;
        return null;
      }
      else throw new HttpException(StatusCodes.Status500InternalServerError, "Une erreur est survenue lors du traitement du fichier");
    }

    public async Task<List<GetUserDto>?> GetAllAsync()
    {
      return await userRepository.GetAll();
    }

    public async Task<GetUserDto?> GetByIdAsync(Guid uuid)
    {
      return await userRepository.GetById(uuid);
    }

    public async Task<GetUserDto?> UpdateAsync(Guid uuid, UpdateUserDto updateUserDto)
    {
      try
      {
        var user = await GetByIdAsync(uuid);
        if(user == null) {
          throw new HttpException(StatusCodes.Status404NotFound, "L'utilisateur est introuvable");
        }
        else{
          var file = user.IdFileNavigation.Id;
          if (updateUserDto.File != null){
            var newFile = await fileService.UpdateAsync(user.IdFileNavigation.Id, new UploadImageDto { File = updateUserDto.File});
            file = newFile.Id;
          }
          await postService.GetPostById(updateUserDto.Post);
          var updatedUser = await userRepository.Update(uuid, updateUserDto, file);
          return mapper.Map<GetUserDto>(updatedUser);  
        } 
      }
      catch (System.Exception)
      {
          throw new HttpException(400, "Une erreur est survenue lors de la modification de l'utilisateur. Vérifiez les champs.");
      }
    }


    public async Task<GetUserDto?> DeleteAsync(Guid uuid)
    {
      var user = await GetByIdAsync(uuid);
      if(user != null) 
      {
        await leaveRepository.RemoveLeaveAuth( uuid );
        return await userRepository.Delete(user.Uuid);
      }
      else return null;
    }


    public async Task<bool> UserWithThisEmailAsync(string email)
    {
      var user = await userRepository.UserWithThisEmail(email);
      if(user != null && user.Uuid != Guid.Empty) return true;
      return false;
    }

    public async Task<List<GetUserDto>?> GetAllUsersByDepartmentAsync(string department)
    {
      return await userRepository.GetAllUsersByDepartmentAsync(department);
    }

    public async Task<List<GetUserDto>> SearchUsersAsync(SearchUserDto searchUserDto)
    {
      var users = await userRepository.SearchUsersAsync(searchUserDto.Search);
      return users;
    }
  }
}