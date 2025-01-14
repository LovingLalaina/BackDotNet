using back_dotnet.ErrorsHandler;
using back_dotnet.Exceptions;
using back_dotnet.Models.DTOs;
using back_dotnet.Models.DTOs.Role;
using back_dotnet.Utils;
using Microsoft.AspNetCore.Mvc;

namespace back_dotnet.Controllers
{
    [Route("role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _roleService.GetAllAsync());
            }
            catch (Exception)
            {
                return HttpHandlerError.InternalServer();
            }
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var roleResponse = await _roleService.GetByIdAsync(id);
                if (roleResponse == null) return NotFound(new ErrorResponse<string>(StatusCodes.Status404NotFound, "Le rôle n'existe pas"));
                return Ok(roleResponse);
            }
            catch (Exception)
            {
                return HttpHandlerError.InternalServer();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto requestRole)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var addedRole = await _roleService.CreateAsync(requestRole);
                    return CreatedAtAction(nameof(Create), addedRole);
                }
                catch (DuplicateEntryException ex)
                {
                    var errorResponse = new ErrorResponse<string>(StatusCodes.Status409Conflict, ex.Message);
                    return Conflict(errorResponse);
                }
                catch (Exception ex)
                {
                    return HttpHandlerError.InternalServer();
                }
            }
            else
            {
                var errors = ModelState.Values
                                 .SelectMany(v => v.Errors)
                                 .Where(e => !string.IsNullOrEmpty(e.ErrorMessage))
                                 .Select(e => e.ErrorMessage);
                var errorResponse = new ErrorResponse<List<string>>(StatusCodes.Status422UnprocessableEntity, errors.ToList());
                return StatusCode(StatusCodes.Status422UnprocessableEntity, errorResponse);
            }
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateRoleDto updateRequestRole)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updatedRole = await _roleService.UpdateAsync(id, updateRequestRole);
                    if (updatedRole == null) return NotFound(new ErrorResponse<string>(StatusCodes.Status404NotFound, "Le rôle n'existe pas"));
                    return Ok(updatedRole);
                }
                catch (DuplicateEntryException ex)
                {
                    var errorResponse = new ErrorResponse<string>(StatusCodes.Status409Conflict, ex.Message);
                    return Conflict(errorResponse);
                }
                catch (Exception)
                {
                    return HttpHandlerError.InternalServer();
                }
            }
            else
            {
                var errors = ModelState.Values
                                 .SelectMany(v => v.Errors)
                                 .Where(e => !string.IsNullOrEmpty(e.ErrorMessage))
                                 .Select(e => e.ErrorMessage);
                var errorResponse = new ErrorResponse<List<string>>(StatusCodes.Status422UnprocessableEntity, errors.ToList());
                return StatusCode(StatusCodes.Status422UnprocessableEntity, errorResponse);
            }
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                var roleDeleted = await _roleService.DeleteAsync(id);
                if (roleDeleted == null) return NotFound(new ErrorResponse<string>(StatusCodes.Status404NotFound, "Le role à supprimer n'existe pas"));
                return Ok(roleDeleted);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new ErrorResponse<string>(StatusCodes.Status403Forbidden, "Impossible de supprimer le rôle"));
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult> SearchRoles([FromQuery] SearchRoleDto searchRoleDto)
        {
            if (string.IsNullOrWhiteSpace(searchRoleDto.Search))
            {
                return BadRequest("Le terme de recherche ne peut pas être vide.");
            }

            try
            {
                var roles = await _roleService.SearchRoleAsync(searchRoleDto);
                return Ok(roles);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
