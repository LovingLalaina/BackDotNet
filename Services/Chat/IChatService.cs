

using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Chat;
using back_dotnet.Models.DTOs.Users;

namespace back_dotnet.Services.Chat;

public interface IChatService
{
    public Task<List<GetDiscussionDto>> GetAllDiscussionsForUser( Guid idUser );
    
    public Task<(Guid idNewMessage, Guid idDiscussion)> SendMessage( MessageDto messageFromClient );
    
    public Task<List<GetUserDto>> SearchUser(Guid idUserSearcher, string search);
    
    public Task<Guid> CreateGroup(CreateGroupDto createGroupDto);
    
    public Task ReadMessage(ReadMessageDto readMessageDto);
    
    public Task AddToGroup(AddGroupDto addGroupDto);
}