
using System.Text.Json;
using back_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace back_dotnet.Seeds;

public static class ChatSeeder
{
    private static List<Discussion>? discussions;

    private static List<UserDiscussion>? userDiscussions;

    private static List<Message>? messages;

    private static List<MessageState>? messageStates;

    public static async Task SeedAsync(HairunSiContext context)
    {
        try
        {
            await SeedDiscussions(context);
            await SeedUserDiscussions(context);
            await SeedMessages(context);
            await SeedMessageStates(context);
        }
        catch (Exception ex)
        {
            throw new Exception("Une erreur s'est produite lors de la génération de données pour le module de messagerie", ex);
        }
    }

    private static async Task SeedDiscussions(HairunSiContext context)
    {
        discussions = JsonSerializer.Deserialize<List<Discussion>>(await System.IO.File.ReadAllTextAsync("./Seeds/ChatData/discussion.json"));
        if (discussions == null)
            throw new Exception("Erreur lors de la lecture du fichier discussion.json");

        foreach(Discussion discussionInSeed in discussions)
        {
            Discussion? discussionInDatabase = await context.Discussions.SingleOrDefaultAsync(discussion => discussion.Id == discussionInSeed.Id);
            if (discussionInDatabase != null) continue;
            await context.AddAsync(discussionInSeed);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedUserDiscussions(HairunSiContext context)
    {
        userDiscussions = JsonSerializer.Deserialize<List<UserDiscussion>>(await System.IO.File.ReadAllTextAsync("./Seeds/ChatData/user-discussion.json"));
        if (userDiscussions == null)
            throw new Exception("Erreur lors de la lecture du fichier user-discussion.json");

        foreach(UserDiscussion userDiscussionInSeed in userDiscussions)
        {
            UserDiscussion? userDiscussionInDatabase = await context.UserDiscussions.SingleOrDefaultAsync(userDdiscussion => userDdiscussion.IdUser == userDiscussionInSeed.IdUser && userDdiscussion.IdDiscussion == userDiscussionInSeed.IdDiscussion);
            if (userDiscussionInDatabase != null) continue;
            await context.AddAsync(userDiscussionInSeed);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedMessages(HairunSiContext context)
    {
        messages = JsonSerializer.Deserialize<List<Message>>(await System.IO.File.ReadAllTextAsync("./Seeds/ChatData/message.json"));
        if (messages == null)
            throw new Exception("Erreur lors de la lecture du fichier message.json");

        foreach(Message messageInSeed in messages)
        {
            Message? messageInDatabase = await context.Messages.SingleOrDefaultAsync(message => message.Id == messageInSeed.Id);
            if (messageInDatabase != null) continue;
            await context.AddAsync(messageInSeed);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedMessageStates(HairunSiContext context)
    {
        messageStates = JsonSerializer.Deserialize<List<MessageState>>(await System.IO.File.ReadAllTextAsync("./Seeds/ChatData/message-state.json"));
        if (messageStates == null)
            throw new Exception("Erreur lors de la lecture du fichier message-state.json");

        foreach(MessageState messageStateInSeed in messageStates)
        {
            MessageState? messageStateInDatabase = await context.MessagesState.SingleOrDefaultAsync(messageState => messageState.IdUser == messageStateInSeed.IdUser && messageState.IdMessage == messageStateInSeed.IdMessage);
            if (messageStateInDatabase != null) continue;
            await context.AddAsync(messageStateInSeed);
            await context.SaveChangesAsync();
        }
    }
}
