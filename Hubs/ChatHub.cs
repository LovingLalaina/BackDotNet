
using back_dotnet.Models.Domain;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace back_dotnet.Hubs;

public class ChatHub :Hub
{
    private readonly HairunSiContext _chatDB;

    public ChatHub( HairunSiContext chatDB){
        _chatDB = chatDB;
    }

    // public async Task JoinChatRoom( UserConnection userConnection)
    // {
    //     await Clients.All.
    //         SendAsync("ReceiveMessage", "admin" , $"{userConnection.Username} has joined");
    // }

    public async Task ConnectUserChat( string idCurrentUser)
    {
        List<UserDiscussion> userDiscussions = await _chatDB.UserDiscussions
        .Where( d => d.IdUser == Guid.Parse( idCurrentUser))
        .ToListAsync();
        foreach (var item in userDiscussions)
        {
        await Groups.AddToGroupAsync( Context.ConnectionId, item.IdDiscussion.ToString());
            
        }

    }

    public async Task SendMessage( string idCurrentUser, string IdDiscussion, string msg)
    {
        List<Message> listeMessages = await _chatDB.Discussions
        .Include(d => d.Messages)
        .Where( d => d.Id.ToString() == IdDiscussion)
        .SelectMany( d => d.Messages).ToListAsync();

        
        await Clients.Group(IdDiscussion)
        .SendAsync( "ReceiveMessage", listeMessages.LastOrDefault());

    }
}