

using back_dotnet.Models.Domain;
using back_dotnet.Repositories.Leave;
using back_dotnet.Repositories.LeaveAuth;
using Quartz;

namespace back_dotnet.Services.Scheduler;

[DisallowConcurrentExecution]
public class CumulableLeaveJob : IJob
{
    private readonly ILeaveRepository _leaveRepository;

    private readonly ILeaveAuthRepository _leaveAuthRepository;

    private readonly ILogger<LeaveJob> _logger;

    public CumulableLeaveJob(ILeaveRepository leaveRepository, ILogger<LeaveJob> logger, ILeaveAuthRepository leaveAuthRepository)
    {
        _leaveRepository = leaveRepository;
        _logger = logger;
        _leaveAuthRepository = leaveAuthRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            string? idUserString = context.JobDetail.JobDataMap.GetString("id_user");
            string? idLeaveTypeString = context.JobDetail.JobDataMap.GetString("id_leave_type");
            if (idUserString == null || idLeaveTypeString == null) return;
            Guid idUser = Guid.Parse(idUserString);
            Guid idLeaveType = Guid.Parse(idLeaveTypeString);

            LeaveAuthorization? leaveAuth = await _leaveRepository.GetLeaveAuthorization(idUser, idLeaveType);

            if (leaveAuth == null || leaveAuth.EndValidity < DateTime.Now)
                return;

            await _leaveAuthRepository.AddSoldeTo(leaveAuth);

            _logger.LogInformation($"L'authorisation de l'utilisateur ({idUser}) a bien été mis à jour");
        }
        catch (Exception unknowknError)
        {
            _logger.LogError(unknowknError, "Une erreur est survenu lors de la mise à jour d'un congé");
            throw new Exception();
        }
    }
}