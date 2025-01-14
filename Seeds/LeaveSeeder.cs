
using System.Text.Json;
using System.Text.Json.Serialization;
using back_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace back_dotnet.Seeds;

public static class LeaveSeeder
{
    private static List<LeaveType>? leaveTypes;

    private static List<Leave>? leaveRequests;

    public static async Task SeedAsync(HairunSiContext context)
    {
        try
        {
            leaveTypes = JsonSerializer.Deserialize<List<LeaveType>>(await System.IO.File.ReadAllTextAsync("./Seeds/LeaveData/leave-type.json"));
            if (leaveTypes == null)
                throw new Exception("Erreur lors de la lecture du fichier leave-type.json");

            foreach (LeaveType leaveTypeInSeed in leaveTypes)
            {
                LeaveType? leaveInDatabase = await context.LeaveType.SingleOrDefaultAsync(leaveType => leaveType.Id == leaveTypeInSeed.Id);
                if (leaveInDatabase != null) continue;
                await context.AddAsync(leaveTypeInSeed);
                await context.SaveChangesAsync();
            }

            SeedData? seedEmployee = JsonSerializer.Deserialize<SeedData>(await System.IO.File.ReadAllTextAsync("seeds-id.json"));
            if (seedEmployee?.Id == null || !seedEmployee.Id.Any())
                throw new Exception("Les données dans le seeds-id.json son introuvables.");

            await SetLeaveAuth(context, Guid.Parse(seedEmployee.Id[0]));
            await SetLeaveAuth(context, Guid.Parse(seedEmployee.Id[1]));
            await SetLeaveRequest(context, Guid.Parse(seedEmployee.Id[1]));

        }
        catch (Exception ex)
        {
            throw new Exception("Une erreur s'est produite lors de la génération de données pour les congés", ex);
        }
    }

    private static async Task SetLeaveAuth(HairunSiContext context, Guid idUser)
    {

        User? user = await context.Users.SingleOrDefaultAsync(user => user.Uuid == idUser);
        if (user == null)
            throw new Exception("l'utilisateur (" + idUser + ") n'a pas été trouvé dans la base de données");
        List<LeaveType> leaveTypes = await context.LeaveType.ToListAsync();
        foreach (LeaveType leaveType in leaveTypes)
        {
            LeaveAuthorization? leaveAuthInDatabase = await context.LeaveAuthorization.SingleOrDefaultAsync(leaveAuth => leaveAuth.IdUser == idUser && leaveAuth.IdLeaveType == leaveType.Id);
            if (leaveAuthInDatabase != null) continue;
            await context.LeaveAuthorization.AddAsync(new LeaveAuthorization()
            {
                Solde = leaveType.IsCumulable ? GenerateSolde(90) : GenerateSolde(leaveType.SoldeEachYear),
                StartValidity = DateTime.Now,
                EndValidity = DateTime.Now.AddYears(1),
                IdUser = idUser,
                IdLeaveType = leaveType.Id
            });
            await context.SaveChangesAsync();
        }
    }

    private static async Task SetLeaveRequest(HairunSiContext context, Guid idUser)
    {
        User? user = await context.Users.SingleOrDefaultAsync(user => user.Uuid == idUser);
        if (user == null)
            throw new Exception("l'utilisateur (" + idUser + ") n'a pas été trouvé dans la base de données");

        leaveRequests = JsonSerializer.Deserialize<List<Leave>>(await System.IO.File.ReadAllTextAsync("./Seeds/LeaveData/leave-request.json"));
        if (leaveRequests == null)
            throw new Exception("Erreur lors de la lecture du fichier leave-type.json");

        foreach (Leave leaveRequestInSeed in leaveRequests)
        {
            Leave? leaveRequestInDatabase = await context.Leave.SingleOrDefaultAsync(leave => leave.Id == leaveRequestInSeed.Id);
            if (leaveRequestInDatabase != null) continue;
            await context.AddAsync(leaveRequestInSeed);
            await context.SaveChangesAsync();
        }
    }
    private static decimal GenerateSolde(decimal maxSolde)
    {
        Random rand = new Random();
        return rand.Next(0, (int)maxSolde) + (rand.Next(0, 2) == 0 ? 0.0M : 0.5M);
    }

    private class SeedData
    {
        [JsonPropertyName("id")]
        public List<string>? Id { get; set; }
    }
}
