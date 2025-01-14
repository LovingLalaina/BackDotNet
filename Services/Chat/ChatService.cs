
using back_dotnet.Exceptions;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Chat;
using back_dotnet.Models.DTOs.Users;
using back_dotnet.Repositories.Chat;
using back_dotnet.Repositories.Users;
using Microsoft.EntityFrameworkCore.Storage;

namespace back_dotnet.Services.Chat;

public class ChatService : IChatService
{
    private readonly ILogger<ChatService> _logger;

    private readonly IUserRepository _userRepository;

    private readonly IChatRepository _chatRepository;

    public ChatService( ILogger<ChatService> logger, IUserRepository userRepository, IChatRepository chatRepository )
    {
        _logger = logger;
        _userRepository = userRepository;
        _chatRepository = chatRepository;
    }

    public async Task<List<GetDiscussionDto>> GetAllDiscussionsForUser(Guid idUser)
    {
        try
        {
            if( await _userRepository.GetById( idUser ) == null )
                throw new HttpException( StatusCodes.Status404NotFound, "L'utilisateur (" + idUser + ") n'existe pas" );
            return await _chatRepository.GetAllDiscussionsForUser( idUser );
        }
        catch(Exception unknowknError)
        {
            if (unknowknError is not HttpException knownError)
            {
                _logger.LogError(unknowknError, "Une erreur s'est produite lors de l'obtention des discussions de l'utilisateur (id_user=" + idUser + ")");
                throw;
            }
            throw knownError;
        }
    }

    public async Task<(Guid idNewMessage, Guid idDiscussion)> SendMessage( MessageDto messageFromClient )
    {
        //404 SI USER_SENDER, DISCUSSION ou USER_RECEIVER NON TROUVE
        using IDbContextTransaction transaction = await _chatRepository.GetTransactioner();
        try
        {
            Guid idNewMessage = Guid.Empty;
            Guid idDiscussion = Guid.Empty;

            if (messageFromClient is StartDiscussionDto startDiscussion)
            {
                await CheckForStartingDiscussion(startDiscussion);
                (idNewMessage, idDiscussion) = await _chatRepository.SendFirstMessage(startDiscussion);
                await _chatRepository.GenerateMessageState(idNewMessage, startDiscussion);
            }
            else if (messageFromClient is SendMessageDto sendMessage)
            {
                await CheckForExistingDiscussion(sendMessage);
                (idNewMessage, idDiscussion) = await _chatRepository.SendMessageOnExistingDiscussion(sendMessage);
                await _chatRepository.GenerateMessageState(idNewMessage, sendMessage);
            }

            if(idNewMessage == Guid.Empty || idDiscussion == Guid.Empty )
                throw new Exception( $"Le format des données envoyées ne correpsondent pas au format attendu soit {typeof(SendMessageDto).FullName} ou {typeof(StartDiscussionDto).FullName}");
            
            await transaction.CommitAsync();
            return (idNewMessage, idDiscussion);
        }
        catch(Exception unknowknError)
        {
            await transaction.RollbackAsync();
            if (unknowknError is not HttpException knownError)
            {
                _logger.LogError(unknowknError, "Une erreur s'est produite lors de l'envoi de message de l'utilisateur (id=" + messageFromClient.IdUserSender + ")");
                throw;
            }
            throw knownError;
        }
    }

    private async Task CheckForExistingDiscussion(SendMessageDto messageFromClient)
    {
        if( await _userRepository.GetById( messageFromClient.IdUserSender ) == null )
            throw new HttpException( StatusCodes.Status404NotFound, "L'utilisateur qui doit envoyer le message n'existe pas" );
        if( await _chatRepository.GetDiscussionById( messageFromClient.IdDiscussion ) == null )
            throw new HttpException( StatusCodes.Status404NotFound, "La discussion où envoyer le message n'existe pas" );
    }

    private async Task CheckForStartingDiscussion(StartDiscussionDto messageFromClient)
    {
        if( await _userRepository.GetById( messageFromClient.IdUserSender ) == null )
            throw new HttpException( StatusCodes.Status404NotFound, "L'utilisateur qui doit envoyer le message n'existe pas" );
        if( await _userRepository.GetById( messageFromClient.IdUserReceiver ) == null )
            throw new HttpException( StatusCodes.Status404NotFound, "L'utilisateur à qui envoyer le message n'existe pas" );
    }

    public async Task<List<GetUserDto>> SearchUser(Guid idUserSearcher, string search)
    {
        try
        {
            if( await _userRepository.GetById( idUserSearcher ) == null )
                throw new HttpException( StatusCodes.Status404NotFound, "L'utilisateur (" + idUserSearcher + ") n'existe pas" );
            return await _chatRepository.SearchUser( idUserSearcher, search );
        }
        catch(Exception unknowknError)
        {
            if (unknowknError is not HttpException knownError)
            {
                _logger.LogError(unknowknError, "Une erreur s'est produite lors de la recherche d'utilisateur ou de groupe de discussion (id_user=" + idUserSearcher + ")");
                throw;
            }
            throw knownError;
        }
    }
    
    public async Task<Guid> CreateGroup(CreateGroupDto createGroupDto)
    {
        //400 SI LISTE_GUID_USER < 3
        //404 SI UN_DES_USERS NON TROUVE
        using IDbContextTransaction transaction = await _chatRepository.GetTransactioner();
        try
        {
            await CheckForCreatingGroup(createGroupDto);
            Guid idNewDiscussion = await _chatRepository.CreateGroup(createGroupDto);
            await transaction.CommitAsync();
            return idNewDiscussion;
        }
        catch(Exception unknowknError)
        {
            await transaction.RollbackAsync();
            if (unknowknError is not HttpException knownError)
            {
                _logger.LogError(unknowknError, "Une erreur s'est produite lors de la création de groupe de discussion");
                throw;
            }
            throw knownError;
        }
    }

    private async Task CheckForCreatingGroup(CreateGroupDto createGroupDto)
    {
        if( createGroupDto.IdsParticipants.Count < 3 )
            throw new HttpException( StatusCodes.Status400BadRequest, $"Il faut fournir au moins 3 participants. Il y a {createGroupDto.IdsParticipants.Count} participant(s)" );
        
        foreach (Guid idParticipant in createGroupDto.IdsParticipants)
            await CheckUserExistence(idParticipant);
    }

    private async Task CheckUserExistence(Guid idParticipant)
    {
        if( (await _userRepository.FindUserByUuidAsync( idParticipant.ToString() )) == null)
            throw new HttpException( StatusCodes.Status404NotFound, $"L'utilisateur (id_user={idParticipant}) n'a pas été trouvé");
    }

    public async Task ReadMessage(ReadMessageDto readMessageDto)
    {
        //400 SI USER OU DISCUSSION NON TROUVE
        using IDbContextTransaction transaction = await _chatRepository.GetTransactioner();
        try
        {
            await CheckForReadMessage(readMessageDto);
            await _chatRepository.ReadMessage(readMessageDto);
            await transaction.CommitAsync();
        }
        catch(Exception unknowknError)
        {
            await transaction.RollbackAsync();
            if (unknowknError is not HttpException knownError)
            {
                _logger.LogError(unknowknError, "Une erreur s'est produite lors du marquage à lu de la discussion");
                throw;
            }
            throw knownError;
        }
    }
    private async Task CheckForReadMessage(ReadMessageDto readMessageDto)
    {
        await CheckUserExistence(readMessageDto.IdUser);
        if( await _chatRepository.GetDiscussionById( readMessageDto.IdDiscussion ) == null )
            throw new HttpException( StatusCodes.Status404NotFound, "La discussion à marquer comme lue n'existe pas" );
    }

    public async Task AddToGroup(AddGroupDto addGroupDto)
    {
        //400 SI USER OU DISCUSSION NON TROUVE
        //409 SI USER DEJA DANS LA DISCUSSION
        //401 SI LA DISCUSSION EST PRIVEE (Discussion à 2)
        using IDbContextTransaction transaction = await _chatRepository.GetTransactioner();
        try
        {
            await CheckForAddGroup(addGroupDto);
            await _chatRepository.AddToGroup(addGroupDto);
            await transaction.CommitAsync();
        }
        catch(Exception unknowknError)
        {
            await transaction.RollbackAsync();
            if (unknowknError is not HttpException knownError)
            {
                _logger.LogError(unknowknError, "Une erreur s'est produite lors de l'ajout de l'utilisateur à la discussion");
                throw;
            }
            throw knownError;
        }
    }

    private async Task CheckForAddGroup(AddGroupDto addGroupDto)
    {
        await CheckUserExistence(addGroupDto.IdUser);
        Discussion? discussion = await _chatRepository.GetDiscussionById( addGroupDto.IdDiscussion );
        if( discussion == null )
            throw new HttpException( StatusCodes.Status404NotFound, $"La discussion (id_discussion={addGroupDto.IdDiscussion}) où ajouter l'utilisateur n'existe pas" );
        if( discussion.ParticipantNumber <= 2 )
            throw new HttpException( StatusCodes.Status403Forbidden, $"La discussion (id_discussion={addGroupDto.IdDiscussion}) est entre deux personnes. Veuillez sélectionner un discussion de groupe");
        if( await _chatRepository.GetUserDiscussionById( addGroupDto.IdUser, addGroupDto.IdDiscussion ) != null )
            throw new HttpException( StatusCodes.Status409Conflict, $"L'utilisateur (id_user={addGroupDto.IdUser}) est déjà dans la discussion (id_discussion={addGroupDto.IdDiscussion})" );
    }
}