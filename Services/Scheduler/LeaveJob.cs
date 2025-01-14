

using back_dotnet.Models.Domain;
using back_dotnet.Repositories.Leave;
using Quartz;

namespace back_dotnet.Services.Scheduler;

[DisallowConcurrentExecution]
public class LeaveJob : IJob
{
    private readonly ILeaveRepository _leaveRepository;

    private readonly ILogger<LeaveJob> _logger;

    public LeaveJob( ILeaveRepository leaveRepository, ILogger<LeaveJob> logger)
    {
        _leaveRepository = leaveRepository;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
         try
        {
            string? idLeave = context.JobDetail.JobDataMap.GetString("id_leave_request");
            if(idLeave == null) return;

            Models.Domain.Leave? leaveToUpdate = await _leaveRepository.GetLeaveRequestById( Guid.Parse(idLeave) );

            if( leaveToUpdate == null || leaveToUpdate.StartDate < DateTime.Now )
                return;
            LeaveStatus oldLeaveStatus = leaveToUpdate.Status;
            LeaveStatus? newLeaveStatus = null;

            if( oldLeaveStatus == LeaveStatus.PendingApproval )
                newLeaveStatus = LeaveStatus.Rejected;

            if( oldLeaveStatus == LeaveStatus.Scheduled )
            {
                newLeaveStatus = LeaveStatus.Taken;
                _leaveRepository.RemoveSolde(leaveToUpdate);
            }
            
            if( newLeaveStatus == null)
                return;

            await _leaveRepository.PatchLeaveRequest( leaveToUpdate, (LeaveStatus)newLeaveStatus);
            _logger.LogInformation($"Le congé({leaveToUpdate.Id}) a bien été mis à jour : {oldLeaveStatus} => {newLeaveStatus}");
        }
        catch(Exception unknowknError)
        {
            _logger.LogError( unknowknError, "Une erreur est survenu lors de la mise à jour d'un congé" );
            throw new Exception();
        }
    }
}