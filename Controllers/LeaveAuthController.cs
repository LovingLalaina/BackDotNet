using back_dotnet.Exceptions;
using back_dotnet.Models.DTOs.LeaveAuth;
using back_dotnet.Services.LeaveAuth;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("leaveAuth")]
public class LeaveAuthController : ControllerBase
{
    private readonly ILeaveAuthService _leaveAuthService;
    public LeaveAuthController( ILeaveAuthService leaveAuthService)
    {
        _leaveAuthService = leaveAuthService;
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetAllLeavesAuthorizationForUser([FromRoute] Guid id, [FromQuery] SearchLeaveAuthDto searchLeaveAuthDto)
    {
        try
        {
            return Ok( await _leaveAuthService.GetAllLeavesAuthorizationForUser( id, searchLeaveAuthDto ) );
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
    [Route("/allDatePeriod{id:Guid}")]
    public async Task<IActionResult> GetAllDatePeriodForUser([FromRoute] Guid id)
    {
        try
        {
            return Ok( await _leaveAuthService.GetAllDatePeriodForUser( id ) );
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
    [Route("/type")]
    public async Task<IActionResult> GetAllLeaveType()
    {
        try
        {
            return Ok( await _leaveAuthService.GetAllLeaveType() );
        }
        catch(Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Une erreur interne s'est produite, plus d'information dans le journal de log" } );
        }
    }

    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> AssignLeaveAuth([FromRoute] Guid id, [FromBody] List<LeaveTypeDto> leaveTypes)
    {
        try
        {
            AssignedLeaveAuthResponse reponse = await _leaveAuthService.AssignLeaveAuth(id, leaveTypes);
            return StatusCode(StatusCodes.Status200OK, new { status = StatusCodes.Status200OK, new_user_leave_auth = reponse });
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