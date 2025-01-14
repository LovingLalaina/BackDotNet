
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;

namespace back_dotnet.Services.Scheduler;

public class LeaveJobFactory : SimpleJobFactory
{
    private readonly IServiceProvider _provider;

    private readonly ILogger<LeaveJobFactory> _logger;


    public LeaveJobFactory( IServiceProvider provider, ILogger<LeaveJobFactory> logger )
    {
        _provider = provider;
        _logger = logger;
    }

    public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        try
        {
            IJob? newLeaveJob = (IJob?)_provider.GetService(bundle.JobDetail.JobType);
            if(newLeaveJob == null)
                throw new Exception("Planificateur de congé non créé");
            return newLeaveJob;
        }
        catch( Exception ex )
        {
            _logger.LogError( ex , "Une erreur s'est produite lors de la planification de changement de status de congé");
            throw new Exception();
        }
    }
}