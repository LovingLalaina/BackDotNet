
using AutoMapper;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.LeaveAuth;
using back_dotnet.Services.Scheduler;
using Microsoft.EntityFrameworkCore;

namespace back_dotnet.Repositories.LeaveAuth;

public class LeaveAuthRepository : ILeaveAuthRepository
{
    private readonly HairunSiContext _dbContext;

    private readonly IMapper _mapper;

    private readonly LeaveScheduler _leaveScheduler;

    public LeaveAuthRepository(HairunSiContext dbContext, IMapper mapper, LeaveScheduler leaveScheduler)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _leaveScheduler = leaveScheduler;
    }

    public async Task<List<LeaveAuthorizationResponseDto>> GetLeavesAuthorizationForUser(Guid idUser, SearchLeaveAuthDto searchLeaveAuthDto)
    {
        IQueryable<LeaveAuthorization> queryLeavesAuth = _dbContext.LeaveAuthorization.OrderByDescending(leaveAuth => leaveAuth.CreatedAt).Include(leaveAuth => leaveAuth.LeaveType).AsQueryable();

        switch (searchLeaveAuthDto.GetSearchCase())
        {
            //AUCUN FILTRE ==> 0
            case 0: queryLeavesAuth = queryLeavesAuth.Where(leaveAuth => leaveAuth.IdUser == idUser); break;
            //SEULEMENT STATUS ==> 1
            case 1: queryLeavesAuth = queryLeavesAuth.Where(leaveAuth => leaveAuth.IdUser == idUser && leaveAuth.LeaveType.Designation == searchLeaveAuthDto.LeaveType); break;
            //SEULEMENT DATE ==> 2
            case 2: queryLeavesAuth = queryLeavesAuth.Where(leaveAuth => leaveAuth.IdUser == idUser && leaveAuth.StartValidity >= searchLeaveAuthDto.StartDate && leaveAuth.EndValidity >= searchLeaveAuthDto.EndDate); break;
            //TOUT EST DONNE ==> 3
            case 3: queryLeavesAuth = queryLeavesAuth.Where(leaveAuth => leaveAuth.IdUser == idUser && leaveAuth.LeaveType.Designation == searchLeaveAuthDto.LeaveType && leaveAuth.StartValidity >= searchLeaveAuthDto.StartDate && leaveAuth.EndValidity >= searchLeaveAuthDto.EndDate); break;
        }

        return _mapper.Map<List<LeaveAuthorizationResponseDto>>(await queryLeavesAuth.ToListAsync());
    }

    public async Task<List<DatePeriodResponseDto>> GetAllDatePeriodForUser(Guid idUser)
    {
        return _mapper.Map<List<DatePeriodResponseDto>>(await _dbContext.LeaveAuthorization.OrderByDescending(leaveAuth => leaveAuth.CreatedAt).Where(leaveAuth => leaveAuth.IdUser == idUser).ToListAsync());
    }

    public async Task<List<LeaveTypeDto>> GetAllLeaveType()
    {
        return _mapper.Map<List<LeaveTypeDto>>(await _dbContext.LeaveType.OrderByDescending(leaveType => leaveType.CreatedAt).ToListAsync());
    }

    public async Task<LeaveAuthorization?> GetLeaveAuth(Guid idUser, Guid idLeaveType)
    {
        return await _dbContext.LeaveAuthorization.SingleOrDefaultAsync(leaveAuth => leaveAuth.IdUser == idUser && leaveAuth.IdLeaveType == idLeaveType);
    }

    public async Task<AssignedLeaveAuthResponse> AssignLeaveAuth(Guid iduser, List<LeaveTypeDto> leaveTypes)
    {
        List<LeaveAuthorizationResponseDto> newLeavesAuthorization = new List<LeaveAuthorizationResponseDto>();

        foreach (LeaveTypeDto actualLeaveType in leaveTypes)
        {
            LeaveType? leaveType = await _dbContext.LeaveType.SingleOrDefaultAsync(leaveType => leaveType.Id == actualLeaveType.Id);
            if (leaveType == null) continue;   //LeaveAuthService GERE DEJA LA NULLITE DU LeaveType

            LeaveAuthorization newLeaveAuth = new LeaveAuthorization()
            {
                Solde = leaveType.IsCumulable ? 0.0M : leaveType.SoldeEachYear,
                StartValidity = DateTime.Now,
                EndValidity = DateTime.Now.AddYears(1),
                IdUser = iduser,
                IdLeaveType = leaveType.Id
            };
            await _dbContext.LeaveAuthorization.AddAsync(newLeaveAuth);
            await _leaveScheduler.ScheduleLeaveAuth(newLeaveAuth, leaveType);
            await _dbContext.SaveChangesAsync();

            newLeavesAuthorization.Add(_mapper.Map<LeaveAuthorizationResponseDto>(newLeaveAuth));
        }
        return new AssignedLeaveAuthResponse()
        {
            IdUser = iduser,
            LeavesAuthorization = newLeavesAuthorization
        };
    }

    public async Task ReinitializeNonCumulableAuth(LeaveAuthorization leaveAuth, decimal soldeEachYear)
    {
        leaveAuth.Solde = soldeEachYear;
        leaveAuth.StartValidity = leaveAuth.EndValidity.AddDays(1);
        leaveAuth.EndValidity = leaveAuth.StartValidity.AddYears(1);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddSoldeTo(LeaveAuthorization leaveAuth)
    {
        leaveAuth.Solde += LeaveType.MonthBalance;
        if (leaveAuth.Solde > 90)
            leaveAuth.Solde = 90;
        await _dbContext.SaveChangesAsync();
    }
}