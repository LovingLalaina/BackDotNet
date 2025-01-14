using back_dotnet.Exceptions;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs;
using back_dotnet.Models.DTOs.Leave;
using back_dotnet.Services.Leave;
using back_dotnet.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("leave")]
public class LeaveController : ControllerBase
{
    private readonly ILeaveService _leaveService;

    public LeaveController( ILeaveService leaveService)
    {
        _leaveService = leaveService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLeavesWithAdmin()
    {
        try
        {
            List<LeaveStatus> allFilters = UtilsMethod.GetAllFilters(Request.Query);
            return Ok( await _leaveService.GetAllLeavesWithAdmin( allFilters ) );
        }
        catch( Exception )
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Une erreur interne s'est produite, plus d'information dans le journal de log" } );
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult> SearchLeavesWithAdmin([FromQuery] SearchLeaveDto searchLeaveDto )
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchLeaveDto.Search))
                return BadRequest(new { status = StatusCodes.Status400BadRequest, error = "Le terme de recherche ne peut pas être vide" });
            return Ok(await _leaveService.SearchLeavesWithAdmin(searchLeaveDto.Search));
        }
        catch( Exception )
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Une erreur interne s'est produite, plus d'information dans le journal de log" } );
        }
    }

    [HttpPost]
    public async Task<ActionResult> AddLeaveRequestWithSimpleUser([FromBody] LeaveRequestDto leaveRequest )
    {
        if (!TryValidateModel(leaveRequest))
            return StatusCode(StatusCodes.Status422UnprocessableEntity, new { status = StatusCodes.Status422UnprocessableEntity, error = ValidationResponse.GetResponseValidation(ModelState) });
        try
        {
            ResponseAfterLeaveRequest reponse = await _leaveService.AddLeaveRequest(leaveRequest);
            return StatusCode(StatusCodes.Status201Created, new { status = StatusCodes.Status201Created, new_leave_request = reponse });
        }
        catch(Exception unknowknException)
        {
            HttpException? knowknException = unknowknException as HttpException;
            if( knowknException == null )
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Une erreur interne s'est produite, plus d'information dans le journal de log" } );
            return StatusCode(knowknException.Status, new { status = knowknException.Status, error = knowknException.Message } );
        }
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetAllLeavesForUser([FromRoute] Guid id)
    {
        try
        {
            List<LeaveStatus> allFilters = UtilsMethod.GetAllFilters(Request.Query);
            
            return Ok( await _leaveService.GetAllLeavesForUser( id, allFilters) );
        }
        catch(Exception unknowknException)
        {
            HttpException? knowknException = unknowknException as HttpException;
            if( knowknException == null )
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Une erreur interne s'est produite, plus d'information dans le journal de log" } );
            return StatusCode(knowknException.Status, new { status = knowknException.Status, error = knowknException.Message } );
        }
    }

    [HttpGet]
    [Route("/search{id:Guid}")]
    public async Task<IActionResult> SearchLeavesDateForUser([FromRoute] Guid id, [FromQuery] SearchLeaveDateDto searchLeaveDateDto)
    {
        try
        {
            return Ok( await _leaveService.SearchLeavesDateForUser( id, searchLeaveDateDto) );
        }
        catch(Exception unknowknException)
        {
            HttpException? knowknException = unknowknException as HttpException;
            if( knowknException == null )
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Une erreur interne s'est produite, plus d'information dans le journal de log" } );
            return StatusCode(knowknException.Status, new { status = knowknException.Status, error = knowknException.Message } );
        }
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> UpdateLeaveRequest([FromRoute] Guid id, [FromBody] LeaveRequestDto leaveRequest)
    {
        if (!TryValidateModel(leaveRequest))
            return StatusCode(StatusCodes.Status422UnprocessableEntity, new { status = StatusCodes.Status422UnprocessableEntity, error = ValidationResponse.GetResponseValidation(ModelState) });
        try
        {
            ResponseAfterLeaveRequest reponse = await _leaveService.UpdateLeaveRequest(id, leaveRequest);
            return StatusCode(StatusCodes.Status200OK, new { status = StatusCodes.Status200OK, updated_leave_request = reponse });
        }
        catch(Exception unknowknException)
        {
            HttpException? knowknException = unknowknException as HttpException;
            if( knowknException == null )
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Une erreur interne s'est produite, plus d'information dans le journal de log" } );
            return StatusCode(knowknException.Status, new { status = knowknException.Status, error = knowknException.Message } );
        }
    }

    [HttpPatch]
    [Route("{id:Guid}")]
    public async Task<IActionResult> PatchLeaveRequest([FromRoute] Guid id, [FromQuery] LeaveStatus newStatus)
    {
        try
        {
            PatchLeaveRequest reponse = await _leaveService.PatchLeaveRequest(id, newStatus);
            return StatusCode(StatusCodes.Status200OK, new { status = StatusCodes.Status200OK, patched_leave_request = reponse });
        }
        catch(Exception unknowknException)
        {
            HttpException? knowknException = unknowknException as HttpException;
            if( knowknException == null )
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Une erreur interne s'est produite, plus d'information dans le journal de log" } );
            return StatusCode(knowknException.Status, new { status = knowknException.Status, error = knowknException.Message } );
        }
    }
}