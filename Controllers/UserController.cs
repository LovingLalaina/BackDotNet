using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using back_dotnet.Exceptions;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Users;
using back_dotnet.Repositories.Users;
using back_dotnet.Services.Users;
using back_dotnet.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace back_dotnet.Controllers
{
  [ApiController]
  [Route("user")]
  public class UserController : ControllerBase
  {
    private IUserService userService;

    public UserController(IUserService userService)
    {
      this.userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
      try
      {
        var users = await userService.GetAllAsync();
        if (users == null) return StatusCode(StatusCodes.Status404NotFound, new { status = StatusCodes.Status404NotFound, error = "Aucun utilisateur trouvé" });
        return StatusCode(StatusCodes.Status200OK, users);
      }
      catch (Exception e)
      {
        var error = e as HttpException;
        if (error is not null)
        {
          return StatusCode(error.Status, new { status = error.Status, error = error.Message });
        }
        return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Internal server error" });
      }
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
      try
      {
        var user = await userService.GetByIdAsync(id);
        if (user == null) return StatusCode(StatusCodes.Status404NotFound, new { status = StatusCodes.Status404NotFound, error = "Aucun utilisateur avec cet identifiant" });
        return StatusCode(StatusCodes.Status200OK, user);
      }
      catch (Exception e)
      {
        var error = e as HttpException;
        if (error is not null)
        {
          return StatusCode(error.Status, new { status = error.Status, error = error.Message });
        }
        return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Internal server error" });
      }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromForm] CreateUserDto createUserDto)
    {

      if (!TryValidateModel(createUserDto))
      {
        var ErrorValidationResponse = ValidationResponse.GetResponseValidation(ModelState);

        return StatusCode(StatusCodes.Status422UnprocessableEntity, new { status = StatusCodes.Status422UnprocessableEntity, error = ErrorValidationResponse });
      }
      try
      {
        var user = await userService.CreateAsync(createUserDto);

        if (user is null) return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Utilisateur non enregistré" });
        return StatusCode(StatusCodes.Status201Created, new { status = StatusCodes.Status201Created });
      }
      catch (System.Exception e)
      {
        var error = e as HttpException;
        if (error is not null)
        {
          return StatusCode(error.Status, new { status = error.Status, error = error.Message });
        }
        return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Internal server error" });
      }
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromForm] UpdateUserDto updateUserDto)
    {
      if (!TryValidateModel(updateUserDto))
      {
        var ErrorValidationResponse = ValidationResponse.GetResponseValidation(ModelState);

        return StatusCode(StatusCodes.Status422UnprocessableEntity, new { status = StatusCodes.Status422UnprocessableEntity, error = ErrorValidationResponse });
      }
      try
      {
        var user = await userService.UpdateAsync(id, updateUserDto);

        if (user is null) return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Utilisateur non enregistré" });
        return StatusCode(StatusCodes.Status200OK, user);
      }
      catch (System.Exception e)
      {
        var error = e as HttpException;
        if (error is not null)
        {
          return StatusCode(error.Status, new { status = error.Status, error = error.Message });
        }
        return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Internal server error" });
      }
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
      try
      {
        var user = await userService.DeleteAsync(id);
        if (user != null) return StatusCode(StatusCodes.Status200OK, id);
        return StatusCode(StatusCodes.Status404NotFound, new { status = StatusCodes.Status404NotFound, error = "Utilisateur introuvable" });
      }
      catch (Exception e)
      {
        var error = e as HttpException;
        if (error is not null)
        {
          return StatusCode(error.Status, new { status = error.Status, error = error.Message });
        }
        return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Internal server error" });
      }
    }

    [HttpGet]
    [Route("department")]
    public async Task<IActionResult> GetByDepartement([FromQuery] string department)
    {
      try
      {
        List<GetUserDto>? allUsers;
        if(string.IsNullOrEmpty(department) || department == "department_all")
          allUsers = await userService.GetAllAsync();
        else
          allUsers = await userService.GetAllUsersByDepartmentAsync(department);

        if(allUsers is null)  return StatusCode(StatusCodes.Status404NotFound, new { status = StatusCodes.Status404NotFound, error = "Aucun utilisateur trouvé" });
        return Ok(allUsers);
      }
      catch (Exception e)
      {
        var error = e as HttpException;
        if (error is not null)
        {
          return StatusCode(error.Status, new { status = error.Status, error = error.Message });
        }
        return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Internal server error" });
      }
    }

    [HttpGet("search")]
        public async Task<ActionResult> SearchUsers([FromQuery] SearchUserDto searchUserDto)
        {
            if (string.IsNullOrWhiteSpace(searchUserDto.Search))
            {
                return BadRequest("Le terme de recherche ne peut pas être vide.");
            }

            try
            {
                var users = await userService.SearchUsersAsync(searchUserDto);
                return Ok(users);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur s'est produite lors de la recherche d'utilisateurs");
            }
        }
  }
}