using AutoMapper;
using back_dotnet.Exceptions;
using back_dotnet.Models.DTOs.Department;
using back_dotnet.Services;
using back_dotnet.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("department")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService departmentService;
    private readonly IMapper Mapper;
    public DepartmentController(IDepartmentService departmentService, IMapper mapper)
    {
        this.Mapper = mapper;
        this.departmentService = departmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDepartment([FromQuery] bool role, [FromQuery] bool posts)
    {
        try
        {
            var query = GetQueryParameter();

            return Ok(await this.departmentService.GetAllDepartments(query));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex);
        }
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetDepartmentById([FromRoute] Guid id, [FromQuery] bool role, [FromQuery] bool posts)
    {
        try
        {
            var query = GetQueryParameter();

            var department = await this.departmentService.GetDepartmentById(id, query);
            if (department == null)
            {
                return NotFound("Le department n'existe pas");
            }

            return Ok(department);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDto createDepartmentDto)
    {
        if (!TryValidateModel(createDepartmentDto))
        {
            var ErrorValidationResponse = ValidationResponse.GetResponseValidation(ModelState);

            return StatusCode(StatusCodes.Status422UnprocessableEntity, new { status = StatusCodes.Status422UnprocessableEntity, error = ErrorValidationResponse });
        }

        try
        {
            var department = await departmentService.CreateDepartment(createDepartmentDto);

            return CreatedAtAction(nameof(GetDepartmentById), new { id = department.Id }, department);
        }
        catch (Exception ex)
        {
            var innerException = ex as HttpException;
            if (innerException is not null)
            {
                return StatusCode(innerException.Status, new { status = innerException.Status, error = innerException.Message });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> UpdateDepartment([FromRoute] Guid id, [FromBody] UpdateDepartmentDto updateDepartmentDto)
    {
        var departmentResult = await this.departmentService.GetDepartmentById(id);
        if (departmentResult == null)
        {
            return StatusCode(StatusCodes.Status404NotFound, new { status = StatusCodes.Status404NotFound, error = "Le department n'existe pas" });
        }

        if (!TryValidateModel(updateDepartmentDto))
        {
            var ErrorValidationResponse = ValidationResponse.GetResponseValidation(ModelState);

            return StatusCode(StatusCodes.Status422UnprocessableEntity, new { status = StatusCodes.Status422UnprocessableEntity, error = ErrorValidationResponse });
        }

        try
        {
            var department = await departmentService.UpdateDepartment(id, updateDepartmentDto);

            return Ok(department);
        }
        catch (Exception ex)
        {
            var innerException = ex as HttpException;
            if (innerException is not null)
            {
                return StatusCode(innerException.Status, new { status = innerException.Status, error = innerException.Message });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteDepartment([FromRoute] Guid id)
    {
        try
        {
            var department = await departmentService.DeleteDepartment(id);

            return StatusCode(StatusCodes.Status200OK, new { message = "Le département a été supprimé avec succés" });
        }
        catch (Exception ex)
        {
            var innerException = ex as HttpException;
            if (innerException is not null)
            {
                return StatusCode(innerException.Status, new { status = innerException.Status, error = innerException.Message });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    private List<string> GetQueryParameter()
    {
        return HttpContext
                        .Request
                        .Query
                        .ToDictionary(q => q.Key, q => q.Value)
                        .Where(q => (q.Key != string.Empty && q.Value == string.Empty) || (Convert.ToBoolean(q.Value) == true))
                        .ToDictionary(q => q.Key, q => q.Value)
                        .Keys
                        .ToList();
    }

    [HttpGet("anonymous")]
    public async Task<IActionResult> GetWithAnonymous()
    {
        try
        {
            return Ok(await departmentService.GetAllDepartmentWithAnonymous(GetQueryParameter()));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex);
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult> SearchDepartments([FromQuery] SearchDepartmentDto searchDepartmentDto)
    {
        if (string.IsNullOrWhiteSpace(searchDepartmentDto.Search))
        {
            return BadRequest("Recherche vide");
        }

        try
        {
            var departments = await departmentService.SearchDepartmentAsync(searchDepartmentDto);
            return Ok(departments);
        }
        catch (System.Exception)
        {
            return StatusCode(500, "An error occurred while processing your request.");

        }
    }
}