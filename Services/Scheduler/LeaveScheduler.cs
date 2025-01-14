
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Leave;
using Quartz;

namespace back_dotnet.Services.Scheduler;

public class LeaveScheduler
{
    private readonly IScheduler _scheduler;

    public LeaveScheduler(IScheduler scheduler)
    {
        _scheduler = scheduler;
    }

    public async Task ScheduleLeave(ResponseAfterLeaveRequest leaveToSchedule)
    {
        IJobDetail job = JobBuilder.Create<LeaveJob>()
            .WithIdentity($"leave-{leaveToSchedule.IdLeave}", "leave-module")
            .UsingJobData("id_leave_request", leaveToSchedule.IdLeave)
            .Build();

        DateTime leaveStartDate = leaveToSchedule.StartDate,
        now = DateTime.Now;
        DateTime timeToExecuteLeaveJob = (leaveStartDate <= now) ? now : leaveStartDate;

        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity($"trigger-{leaveToSchedule.IdLeave}", "leave-module")
            .StartAt(timeToExecuteLeaveJob.AddSeconds(10))
            .WithSimpleSchedule(x => x.WithRepeatCount(0))
            .Build();
        await _scheduler.ScheduleJob(job, trigger);
    }

    public async Task ScheduleLeaveAuth(LeaveAuthorization leaveAuth, LeaveType leaveType)
    {
        JobBuilder? leaveAuthJobBuilder = null;
        ITrigger? trigger = null;
        if (!leaveType.IsCumulable)
        {
            leaveAuthJobBuilder = JobBuilder.Create<NonCumulableLeaveJob>();
            trigger = TriggerBuilder.Create()
            .WithIdentity($"trigger-user-{leaveAuth.IdUser}-leave-type-{leaveAuth.IdLeaveType}", "leave-auth-module")
            .StartAt(leaveAuth.EndValidity.AddSeconds(10))
            .WithSimpleSchedule(x => x
            .WithInterval(TimeSpan.FromDays(366))
            .RepeatForever())
            .Build();

        }
        else
        {
            leaveAuthJobBuilder = JobBuilder.Create<CumulableLeaveJob>();
            trigger = TriggerBuilder.Create()
            .WithIdentity($"trigger-user-{leaveAuth.IdUser}-leave-type-{leaveAuth.IdLeaveType}", "leave-auth-module")
            .StartAt(leaveAuth.StartValidity.AddSeconds(10))
            .WithSimpleSchedule(x => x
            .WithInterval(TimeSpan.FromDays(30))
            .RepeatForever())
            .Build();
        }

        IJobDetail job = JobBuilder.Create<NonCumulableLeaveJob>()
            .WithIdentity($"leave-auth-user-{leaveAuth.IdUser}-leave-type-{leaveAuth.IdLeaveType}", "leave-auth-module")
            .UsingJobData("id_user", leaveAuth.IdUser)
            .UsingJobData("id_leave_type", leaveAuth.IdLeaveType)
            .Build();
        await _scheduler.ScheduleJob(job, trigger);
    }
}
