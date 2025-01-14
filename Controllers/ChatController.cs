using Microsoft.AspNetCore.Mvc;
using back_dotnet.Exceptions;
using back_dotnet.Utils;
using back_dotnet.Services.Chat;
using back_dotnet.Models.DTOs.Chat;

namespace back_dotnet.Controllers;

[ApiController]
[Route("chat")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController( IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetAllDiscussionsForUser( [FromRoute] Guid id )
    {
        try
        {
            return Ok( await _chatService.GetAllDiscussionsForUser( id ) );
        }
        catch(Exception unknowknException)
        {
            if (unknowknException is not HttpException knowknException)   //ERREUR INATTENDU
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Une erreur interne s'est produite, plus d'information dans le journal de log" });
            return StatusCode(knowknException.Status, new { status = knowknException.Status, error = knowknException.Message } );
        }
    }

    [HttpPost]
    [Route("start-discussion")]
    public async Task<IActionResult> StartDiscussion( [FromBody] StartDiscussionDto messageFromClient )
    {
        try
        {
            return await SendMessage( messageFromClient );
        }
        catch(Exception unknowknException)
        {
            if (unknowknException is not HttpException knowknException)   //ERREUR INATTENDU
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Une erreur interne s'est produite, plus d'information dans le journal de log" });
            return StatusCode(knowknException.Status, new { status = knowknException.Status, error = knowknException.Message } );
        }
    }

    private async Task<IActionResult> SendMessage(MessageDto messageFromClient)
    {
        if (!TryValidateModel(messageFromClient))
            return StatusCode(StatusCodes.Status422UnprocessableEntity, new { status = StatusCodes.Status422UnprocessableEntity, error = ValidationResponse.GetResponseValidation(ModelState) });
        
        (Guid idNewMessage, Guid idDiscussion) = await _chatService.SendMessage(messageFromClient);

        return StatusCode(StatusCodes.Status201Created, new { status = StatusCodes.Status201Created, id_new_message = idNewMessage, id_discussion = idDiscussion });
    }

    [HttpPost]
    [Route("send-message")]
    public async Task<IActionResult> SendMessageOnExistingDiscussion( [FromBody] SendMessageDto messageFromClient )
    {
        try
        {
            return await SendMessage( messageFromClient );
        }
        catch(Exception unknowknException)
        {
            if (unknowknException is not HttpException knowknException)   //ERREUR INATTENDU
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Une erreur interne s'est produite, plus d'information dans le journal de log" });
            return StatusCode(knowknException.Status, new { status = knowknException.Status, error = knowknException.Message } );
        }
    }

    [HttpGet]
    [Route("search-user-to-talk/{id:Guid}")]
    public async Task<IActionResult> SearchUser( [FromQuery] string search, [FromRoute] Guid id )
    {
        try
        {
            return Ok( await _chatService.SearchUser( id, search ) );
        }
        catch(Exception unknowknException)
        {
            HttpException? knowknException = unknowknException as HttpException;
            if( knowknException == null )   //ERREUR INATTENDU
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Une erreur interne s'est produite, plus d'information dans le journal de log" } );
            return StatusCode(knowknException.Status, new { status = knowknException.Status, error = knowknException.Message } );
        }
    }

    [HttpPost]
    [Route("create-group")]
    public async Task<IActionResult> CreateGroup( [FromBody] CreateGroupDto createGroupDto )
    {
        try
        {
            if (!TryValidateModel(createGroupDto))
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new { status = StatusCodes.Status422UnprocessableEntity, error = ValidationResponse.GetResponseValidation(ModelState) });
        
            Guid idNewDiscussion = await _chatService.CreateGroup(createGroupDto);
            return StatusCode(StatusCodes.Status201Created, new { status = StatusCodes.Status201Created, id_new_discussion = idNewDiscussion });
        }
        catch(Exception unknowknException)
        {
            if (unknowknException is not HttpException knowknException)   //ERREUR INATTENDU
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Une erreur interne s'est produite, plus d'information dans le journal de log" });
            return StatusCode(knowknException.Status, new { status = knowknException.Status, error = knowknException.Message } );
        }
    }

    [HttpPatch]
    [Route("read-message")]
    public async Task<IActionResult> ReadMessage( [FromBody] ReadMessageDto readMessageDto )
    {
        try
        {
            if (!TryValidateModel(readMessageDto))
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new { status = StatusCodes.Status422UnprocessableEntity, error = ValidationResponse.GetResponseValidation(ModelState) });
            
            await _chatService.ReadMessage(readMessageDto);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch(Exception unknowknException)
        {
            if (unknowknException is not HttpException knowknException)   //ERREUR INATTENDU
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Une erreur interne s'est produite, plus d'information dans le journal de log" });
            return StatusCode(knowknException.Status, new { status = knowknException.Status, error = knowknException.Message } );
        }
    }

    [HttpPost]
    [Route("add-to-group")]
    public async Task<IActionResult> AddToGroup( [FromBody] AddGroupDto addGroupDto )
    {
        try
        {
            if (!TryValidateModel(addGroupDto))
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new { status = StatusCodes.Status422UnprocessableEntity, error = ValidationResponse.GetResponseValidation(ModelState) });
        
            await _chatService.AddToGroup(addGroupDto);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch(Exception unknowknException)
        {
            if (unknowknException is not HttpException knowknException)   //ERREUR INATTENDU
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = "Une erreur interne s'est produite, plus d'information dans le journal de log" });
            return StatusCode(knowknException.Status, new { status = knowknException.Status, error = knowknException.Message } );
        }
    }
}