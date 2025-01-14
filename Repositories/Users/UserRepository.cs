using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using back_dotnet.Exceptions;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Users;
using Microsoft.EntityFrameworkCore;

namespace back_dotnet.Repositories.Users
{
  public class UserRepository : IUserRepository
  {
    private readonly HairunSiContext context;
    private readonly IMapper mapper;

    public UserRepository(HairunSiContext context, IMapper mapper)
    {
      this.context = context;
      this.mapper = mapper;
    }

    public async Task<GetUserDto?> Create(User user)
    {
      try
      {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return mapper.Map<GetUserDto>(user);
      }
      catch (System.Exception)
      { 
        throw new HttpException(500, "Une erreur s'est produite lors de la création");
      }
    }

    public async Task<List<GetUserDto>?> GetAll()
    {
      var users = await context.Users.OrderByDescending(x => x.CreatedAt)
      .Include(x => x.IdFileNavigation)
      .Include(x => x.IdPostNavigation.IdDepartmentNavigation.IdRoleNavigation.PermissionRoles).ThenInclude(x => x.IdPermissionNavigation)
      .ToListAsync();
      return mapper.Map<List<GetUserDto>>(users);
    }

    public async Task<GetUserDto?> UserWithThisEmail(string email)
    {
      return mapper.Map<GetUserDto>(
        await context.Users
        .Include(x => x.IdPostNavigation)
        .Include(x => x.IdFileNavigation)
        .FirstOrDefaultAsync(user => user.Email == email)
      );
    }

    public async Task<GetUserDto?> GetUserWithMail(string email)
    {
      return mapper.Map<GetUserDto>(
        await context.Users
        .Include(x => x.IdPostNavigation)
        .Include(x => x.IdPostNavigation.IdDepartmentNavigation.IdRoleNavigation.PermissionRoles).ThenInclude(x => x.IdPermissionNavigation)
        .FirstOrDefaultAsync(user => user.Email == email)
      );
    }

    public async Task<GetUserDto?> GetById(Guid uuid)
    {
      return mapper.Map<GetUserDto>(await context.Users
      .Include(x => x.IdPostNavigation.IdDepartmentNavigation.IdRoleNavigation.PermissionRoles)
        .ThenInclude(x => x.IdPermissionNavigation)
      .Include(x => x.IdFileNavigation).FirstOrDefaultAsync(x => x.Uuid == uuid));
    }

    public async Task<User?> FindUserByUuidAsync(string uuid)
    {
      if (!Guid.TryParse(uuid, out Guid uuidGuid))
      {
        return null;
      }

      return await context.Users
          .Where(u => u.Uuid == uuidGuid)
          .Select(u => new User
          {
            Email = u.Email,
            Lastname = u.Lastname,
            Firstname = u.Firstname,
            Password = u.Password,
            Uuid = u.Uuid
          })
          .FirstOrDefaultAsync();
    }

    public async Task<User?> Update(Guid uuid, UpdateUserDto updateUser, Guid file)
    {
      var _user = await context.Users
      .Include(x => x.IdPostNavigation)
      .Include(x => x.IdFileNavigation)
      .SingleAsync(user => user.Uuid == uuid);
      if (_user == null) return null;
      var ti = CultureInfo.CurrentCulture.TextInfo;
      _user.Firstname = Regex.Replace(updateUser.Firstname.ToString().Trim(), @"\s+", " ").ToUpper();
      _user.Lastname = ti.ToTitleCase(Regex.Replace(updateUser.Lastname.ToString(), @"\s+", " "));
      _user.BirthDate = updateUser.BirthDate;
      _user.IdPost = updateUser.Post;
      _user.IdFile = file;
      _user.UpdatedAt = DateTime.Now;
      await context.SaveChangesAsync();
      return _user;
    }

    public async Task<GetUserDto?> Delete(Guid uuid)
    {
      try
      {
        var _user = await context.Users.SingleAsync(x => x.Uuid == uuid);
        context.Remove(_user);
        await context.SaveChangesAsync();

        return mapper.Map<GetUserDto>(_user);
      }
      catch (System.Exception)
      {
        throw new HttpException(500, "La suppression a échoué");
      }
    }

    public async Task UpdateUserAsync(Guid userId, string newPassword)
    {
      var user = new User
      {
        Uuid = userId,
        Password = newPassword
      };

      context.Users.Attach(user);
      context.Entry(user).Property(u => u.Password).IsModified = true;

      await context.SaveChangesAsync();
    }
    public async Task<List<GetUserDto>?> GetAllUsersByDepartmentAsync(string departementDonnee)
    {
      try
      {
        var users = await context.Users.OrderByDescending(x => x.CreatedAt)
          .Include(x => x.IdFileNavigation)
          .Include(x => x.IdPostNavigation.IdDepartmentNavigation.IdRoleNavigation.PermissionRoles).ThenInclude(x => x.IdPermissionNavigation)
          .Where(x => x.IdPostNavigation.IdDepartmentNavigation.Id == Guid.Parse(departementDonnee))
          .ToListAsync();
        return mapper.Map<List<GetUserDto>>(users);
      }
      catch( FormatException )
      {
        throw new HttpException( 400, "Filtre de departement invalide");
      }


    }

    public async Task<List<GetUserDto>?> SearchUsersAsync(string search)
    {
      IQueryable<User> queryUsers = context.Users.OrderByDescending(x => x.CreatedAt)
      .Include(x => x.IdFileNavigation)
      .Include(x => x.IdPostNavigation.IdDepartmentNavigation.IdRoleNavigation.PermissionRoles).ThenInclude(x => x.IdPermissionNavigation)
      .AsQueryable();
      search = search.ToLower();
      queryUsers = queryUsers.Where(u => u.Firstname.ToLower().Contains(search) ||
                                    u.Lastname.ToLower().Contains(search) ||
                                    u.Matricule.ToLower().Contains(search));
      List<User> users = await queryUsers.ToListAsync();
      return mapper.Map<List<GetUserDto>>(users);
    }
  }
}