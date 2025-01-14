using back_dotnet.ErrorsHandler;
using back_dotnet.Exceptions;
using back_dotnet.Models.DTOs.Permission;
using back_dotnet.Services.Permission;
using back_dotnet.Utils;
using Microsoft.AspNetCore.Mvc;

namespace back_dotnet.Controllers
{
    [Route("permission")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var permissionsResponse = await _permissionService.GetAllAsync();
                if (!permissionsResponse.Any()) return NotFound(new ErrorResponse<string>(StatusCodes.Status404NotFound, "Aucune permission existante"));
                return Ok(permissionsResponse);
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
                var permissionResponse = await _permissionService.GetByIdAsync(id);
                if (permissionResponse == null) return NotFound(new ErrorResponse<string>(StatusCodes.Status404NotFound, "Aucune permission existante avec cet identifiant"));
                return Ok(permissionResponse);
            }
            catch (Exception)
            {
                return HttpHandlerError.InternalServer();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrUpdatePermissionDto requestPermission)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var addedPermission = await _permissionService.CreateAsync(requestPermission);
                    return CreatedAtAction(nameof(Create), addedPermission);
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
                var ErrorValidationResponse = ValidationResponse.GetResponseValidation(ModelState);
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new
                {
                    status = StatusCodes.Status422UnprocessableEntity,
                    error = ErrorValidationResponse
                });
            }
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CreateOrUpdatePermissionDto updateRequestPermission)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updatedPermission = await _permissionService.UpdateAsync(id, updateRequestPermission);
                    if (updatedPermission == null) return NotFound(new ErrorResponse<string>(StatusCodes.Status404NotFound, "La permission n'existe pas"));
                    return Ok(updatedPermission);
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
                var ErrorValidationResponse = ValidationResponse.GetResponseValidation(ModelState);
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new
                {
                    status = StatusCodes.Status422UnprocessableEntity,
                    error = ErrorValidationResponse
                });
            }
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var permissionDeleted = await _permissionService.DeleteAsync(id);
                if (permissionDeleted == null) return NotFound(new ErrorResponse<string>(StatusCodes.Status404NotFound, "Aucune permission existante avec cet identifiant"));
                return StatusCode(StatusCodes.Status200OK, new { message = "Permission supprimée" });
            }
            catch (Exception)
            {
                return HttpHandlerError.InternalServer();
            }
        }
    }
}
