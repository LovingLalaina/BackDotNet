

using AutoMapper;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Chat;
using back_dotnet.Models.DTOs.Files;
using back_dotnet.Models.DTOs.Users;
using back_dotnet.Repositories.Files;
using back_dotnet.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace back_dotnet.Repositories.Chat;

public class ChatRepository : IChatRepository
{
    private readonly HairunSiContext _dbContext;

    private readonly IMapper _mapper;

    private readonly IFileRepository _fileRepository;

    private readonly IUserRepository _userRepository;

    public ChatRepository(HairunSiContext dbContext, IMapper mapper, IFileRepository fileRepository, IUserRepository userRepository)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _fileRepository = fileRepository;
        _userRepository = userRepository;

    }

    public async Task<List<GetDiscussionDto>> GetAllDiscussionsForUser(Guid idUser)
    {
        List<Discussion> discussions = await _dbContext.Discussions
        .Include( discussion => discussion.Messages )
            .ThenInclude( message => message.MessageStates )
            .AsSplitQuery()
        .Include( discussion => discussion.UserDiscussions )
            .ThenInclude( userDiscussion => userDiscussion.User )
                .ThenInclude( user => user.IdFileNavigation )
            .AsSplitQuery()
        .Where( discussion => discussion.UserDiscussions
            .Select( userDiscussion => userDiscussion.IdUser)
            .Contains( idUser ))
        .ToListAsync();

        return _mapper.Map<List<GetDiscussionDto>>(
            await GetAvatarsForDiscussion( discussions, idUser ),
            opt => opt.Items["IdUserReader"] = idUser
        );
    }

    private async Task<List<Discussion>> GetAvatarsForDiscussion(List<Discussion> discussions, Guid idUserReader)
    {
        foreach( Discussion discussion in discussions )
            await SetAvatarFor( discussion, idUserReader);
        return discussions;
    }

    private async Task SetAvatarFor(Discussion discussion, Guid idUserReader)
    {
        if( discussion.ParticipantNumber <= 2)
        {
            string? imagePath = (await _userRepository
            .GetById(Guid.Parse(
            discussion.Title.Split('/')
            .First(id => id != idUserReader.ToString()))))?
            .IdFileNavigation?.Path;

            if( imagePath != null )
                discussion.Avatar = imagePath;
            
        }
    }

    public async Task<(Guid idNewMessage, Guid idDiscussion)> SendFirstMessage(StartDiscussionDto startDiscussion)
    {
        Discussion newDiscussion = new Discussion(startDiscussion);

        Discussion? existingDiscussion = await _dbContext.Discussions
        .SingleOrDefaultAsync( discussion => discussion.Title == newDiscussion.Title
        || discussion.Title == ReverseTitle( newDiscussion.Title ));

        Guid idNewDiscussion = Guid.Empty;
        if(existingDiscussion != null)
            idNewDiscussion = existingDiscussion.Id;
        else
        {
            await _dbContext.AddAsync(newDiscussion);
            await _dbContext.SaveChangesAsync();
            idNewDiscussion = newDiscussion.Id;
            await _dbContext.AddAsync(new UserDiscussion( startDiscussion.IdUserSender, idNewDiscussion));
            await _dbContext.AddAsync(new UserDiscussion( startDiscussion.IdUserReceiver, idNewDiscussion));
            await _dbContext.SaveChangesAsync();
        }

        Message newMessage = _mapper.Map<Message>( new SendMessageDto(startDiscussion, idNewDiscussion) );
        await _dbContext.AddAsync(newMessage);
        await _dbContext.SaveChangesAsync();
        return (newMessage.Id, idNewDiscussion);
    }

    private string ReverseTitle(string title)
    {
        string[] titleArray = title.Split( "/");
        return titleArray[1] + "/" + titleArray[0];
    }

    public async Task<(Guid idNewMessage, Guid idDiscussion)> SendMessageOnExistingDiscussion(SendMessageDto messageFromClient)
    {
        Message newMessage = _mapper.Map<Message>(messageFromClient);
        await _dbContext.AddAsync(newMessage);
        await _dbContext.SaveChangesAsync();
        return (newMessage.Id, newMessage.IdDiscussion);
    }

    public async Task GenerateMessageState(Guid idMessageSend, SendMessageDto messageFromClient)
    {
        if( await _dbContext.Messages.SingleOrDefaultAsync(message => message.Id == idMessageSend) == null )   throw new Exception( "Le message dont l'état doit être géré n'existe pas" );
        
        List<Guid> idUsersInDiscussion = await _dbContext.UserDiscussions
        .Where(userDiscussion => userDiscussion.IdDiscussion == messageFromClient.IdDiscussion)
        .Select(userDiscussion => userDiscussion.IdUser)
        .ToListAsync();

        await _dbContext.MessagesState.AddRangeAsync( idUsersInDiscussion
        .Select(idUser => new MessageState(idMessageSend, idUser, idUser == messageFromClient.IdUserSender))
        .ToList());
        await _dbContext.SaveChangesAsync();
    }

    public async Task GenerateMessageState(Guid idMessageSend, StartDiscussionDto startDiscussionDto)
    {
        if( await _dbContext.Messages.SingleOrDefaultAsync(message => message.Id == idMessageSend) == null )   throw new Exception( "Le message dont l'état doit être géré n'existe pas" );

        await _dbContext.AddAsync( new MessageState( idMessageSend, startDiscussionDto.IdUserSender, true ) );
        await _dbContext.AddAsync( new MessageState( idMessageSend, startDiscussionDto.IdUserReceiver, false ) );
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<GetUserDto>> SearchUser(Guid idUser, string search)
    {
        search = search.ToLower();
        List<User> users = await _dbContext.Users
        .Include( user => user.IdFileNavigation)
        .Where(user => user.Uuid != idUser &&
        (user.Firstname.ToLower().Contains(search) ||
        user.Lastname.ToLower().Contains(search)))
        .OrderBy( user => user.Firstname ).ThenBy( user => user.Lastname )
        .ToListAsync();

        return _mapper.Map<List<GetUserDto>>(users).ToList();
    }
    
    private List<Guid> GetUsersAlreadyTalked(Guid idUser)
    {
        List<Discussion> discussionsOfUser = _dbContext.Discussions
        .Where(discussion => discussion.ParticipantNumber == 2).ToList()
        .Where(discussion => IsOnDiscussion( discussion.Title, idUser)).ToList();

        return discussionsOfUser
        .SelectMany(discussion => discussion.Title.Split('/'))
        .Where(id => id != idUser.ToString())
        .Select(Guid.Parse).ToList();
    }

    private bool IsOnDiscussion( string title , Guid idUser )
    {
        return title.Split('/').Contains(idUser.ToString());
    }
    
    public async Task<Discussion?> GetDiscussionById(Guid idDiscussion)
    {
      return await _dbContext.Discussions.SingleOrDefaultAsync(discussion => discussion.Id == idDiscussion);
    }

    public async Task<IDbContextTransaction> GetTransactioner()
    {
        return await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task<Guid> CreateGroup(CreateGroupDto createGroupDto)
    {
        Discussion newDiscussion = _mapper.Map<Discussion>(createGroupDto);
        await _dbContext.AddAsync(newDiscussion);
        await _dbContext.SaveChangesAsync();

        Guid idNewDiscussion = newDiscussion.Id;

        foreach( Guid idParticipant in createGroupDto.IdsParticipants )
            await _dbContext.AddAsync(new UserDiscussion(idParticipant, idNewDiscussion));

        await _dbContext.SaveChangesAsync();
        return idNewDiscussion;
    }

    public async Task ReadMessage(ReadMessageDto readMessageDto)
    {
        List<MessageState> nonReadMessages = await _dbContext.Messages
        .Where( message => message.IdDiscussion == readMessageDto.IdDiscussion)
        .SelectMany( message => message.MessageStates)
        .Where( messageState => messageState.IdUser == readMessageDto.IdUser && !messageState.IsRead)
        .ToListAsync();

        nonReadMessages.ForEach(messageState =>
        {
            messageState.IsRead = true;
            messageState.ReadAt = DateTime.Now;
        });

        await _dbContext.SaveChangesAsync();
    }

    public async Task<UserDiscussion?> GetUserDiscussionById(Guid idUser, Guid idDiscussion)
    {
        return await _dbContext.UserDiscussions.
        SingleOrDefaultAsync( userDiscussion =>
        userDiscussion.IdUser == idUser &&
        userDiscussion.IdDiscussion == idDiscussion );
    }

    public async Task AddToGroup(AddGroupDto addGroupDto)
    {
        await _dbContext.AddAsync(new UserDiscussion(addGroupDto.IdUser, addGroupDto.IdDiscussion));

        List<Message> messagesInDiscussion = await _dbContext.Discussions
        .Where( discussion => discussion.Id == addGroupDto.IdDiscussion)
        .SelectMany( discussion => discussion.Messages )
        .ToListAsync();

        foreach( Message message in messagesInDiscussion )
            await _dbContext.AddAsync(new MessageState(message.Id, addGroupDto.IdUser, false));
        await _dbContext.SaveChangesAsync();
    }
}