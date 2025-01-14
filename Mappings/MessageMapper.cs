using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Chat;

namespace back_dotnet.Mappings;

public static class MessageMapper
{
    public static GetMessageDto MapMessage(Message message, Guid idUserReader)
    {
        return new GetMessageDto
        {
            Id = message.Id,
            IdUser = message.IdUser,
            Title = message.User.Firstname + ' ' + message.User.Lastname,
            Content = message.Content,
            CreatedAt = message.CreatedAt,
            IsRead = IsMessageReadFor( message, idUserReader),
            Profil = message.User.IdFileNavigation
        };
    }

    private static bool IsMessageReadFor(Message message, Guid idUserReader)
    {
        List<MessageState> messageStates = message.MessageStates.ToList();
        MessageState? messageState = messageStates
        .SingleOrDefault( messageState => messageState.IdUser == idUserReader );
        if( messageState == null )  throw new Exception( $"L'état du message ({message.Id}) pour l'utilisateur ({idUserReader}) n'a pas été trouvé");
        return messageState.IsRead;
    }
}
