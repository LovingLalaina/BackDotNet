

using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Chat;
using back_dotnet.Models.DTOs.Users;
using Microsoft.EntityFrameworkCore.Storage;

namespace back_dotnet.Repositories.Chat;

public interface IChatRepository
{
    public Task<List<GetDiscussionDto>> GetAllDiscussionsForUser(Guid idUser);

    public Task<Discussion?> GetDiscussionById(Guid idDiscussion);
    
    public Task<(Guid idNewMessage, Guid idDiscussion)> SendMessageOnExistingDiscussion(SendMessageDto messageFromClient);

    public Task<IDbContextTransaction> GetTransactioner();

    public Task GenerateMessageState(Guid newId, SendMessageDto messageFromClient);

    public Task GenerateMessageState(Guid newId, StartDiscussionDto messageFromClient);

    public Task<(Guid idNewMessage, Guid idDiscussion)> SendFirstMessage(StartDiscussionDto startDiscussion);
    
    public Task<List<GetUserDto>> SearchUser(Guid idUser, string search);
    
    public Task<Guid> CreateGroup(CreateGroupDto createGroupDto);
    
    public Task ReadMessage(ReadMessageDto readMessageDto);
    
    public Task AddToGroup(AddGroupDto addGroupDto);

    public Task<UserDiscussion?> GetUserDiscussionById(Guid idUser, Guid idDiscussion);
}