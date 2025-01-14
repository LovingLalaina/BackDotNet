
using AutoMapper;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Leave;
using back_dotnet.Utils;
using Microsoft.EntityFrameworkCore;

namespace back_dotnet.Repositories.Leave;

public class LeaveRepository : ILeaveRepository
{
    private readonly HairunSiContext _dbContext;

    private readonly IMapper _mapper;

    public LeaveRepository(HairunSiContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<LeaveForAdminResponseDto>> GetAllLeavesWithAdmin(List<LeaveStatus> allFilters)
    {
        IQueryable<Models.Domain.Leave> queryLeaves = _dbContext.Leave.OrderByDescending(leave => leave.CreatedAt).Include(leave => leave.Type).Include(leave => leave.User).ThenInclude(leaveType => leaveType.LeavesAuthorization).AsQueryable();

        if (allFilters.Count != 0 && allFilters.First() != LeaveStatus.All)
            queryLeaves = queryLeaves.Where(leave => allFilters.Contains(leave.Status));

        return _mapper.Map<List<LeaveForAdminResponseDto>>(await queryLeaves.ToListAsync());
    }

    public async Task<List<LeaveForAdminResponseDto>> SearchLeavesWithAdmin(string search)
    {
        IQueryable<Models.Domain.Leave> queryLeaves = _dbContext.Leave.OrderByDescending(leave => leave.CreatedAt).Include(leave => leave.Type).Include(leave => leave.User).ThenInclude(leaveType => leaveType.LeavesAuthorization).AsQueryable();
        search = search.ToLower();
        queryLeaves = queryLeaves.Where(leave => leave.User.Lastname.ToLower().Contains(search) ||
                                    leave.User.Matricule.ToLower().Contains(search));
        return _mapper.Map<List<LeaveForAdminResponseDto>>(await queryLeaves.ToListAsync());
    }

    public async Task<LeaveType?> GetLeaveTypeById(Guid idLeaveType)
    {
        return await _dbContext.LeaveType.SingleOrDefaultAsync(leaveType => leaveType.Id == idLeaveType);
    }

    public async Task<LeaveAuthorization?> GetLeaveAuthorization(Guid idUser, Guid idLeaveType)
    {
        return await _dbContext.LeaveAuthorization.SingleOrDefaultAsync(leaveAuthorization => leaveAuthorization.IdUser == idUser && leaveAuthorization.IdLeaveType == idLeaveType);
    }

    public async Task<ResponseAfterLeaveRequest> AddLeaveRequest(LeaveRequestDto leaveRequest)
    {
        Models.Domain.Leave newLeave = _mapper.Map<Models.Domain.Leave>(leaveRequest);
        await _dbContext.Leave.AddAsync(newLeave);
        await _dbContext.SaveChangesAsync();
        ResponseAfterLeaveRequest response = _mapper.Map<ResponseAfterLeaveRequest>(leaveRequest);
        response.IdLeave = newLeave.Id;
        return response;
    }

    public async Task<List<LeaveForUserResponseDto>> GetAllLeavesForUser(Guid idUser, List<LeaveStatus> allFilters)
    {
        IQueryable<Models.Domain.Leave> queryLeaves = _dbContext.Leave.OrderByDescending(leave => leave.CreatedAt).Include(leave => leave.Type).Include(leave => leave.User).ThenInclude(user => user.LeavesAuthorization).AsQueryable();

        if (allFilters.Count == 0 || allFilters.First() == LeaveStatus.All)
            queryLeaves = queryLeaves.Where(leave => leave.IdUser == idUser);
        else
            queryLeaves = queryLeaves.Where(leave => leave.IdUser == idUser && allFilters.Contains(leave.Status));

        return _mapper.Map<List<LeaveForUserResponseDto>>(await queryLeaves.ToListAsync());
    }

    public async Task<List<LeaveForUserResponseDto>> SearchLeavesForNow(Guid idUser)
    {
        IQueryable<Models.Domain.Leave> queryLeaves = _dbContext.Leave.OrderByDescending(leave => leave.CreatedAt).Include(leave => leave.Type).Include(leave => leave.User).ThenInclude(leaveType => leaveType.LeavesAuthorization).AsQueryable();
        DateTime now = DateTime.Now;
        queryLeaves = queryLeaves.Where(leave => leave.IdUser == idUser && leave.StartDate <= now && now <= leave.EndDate);
        return _mapper.Map<List<LeaveForUserResponseDto>>(await queryLeaves.ToListAsync());
    }

    public async Task<List<LeaveForUserResponseDto>> SearchLeavesContainedBetween(Guid idUser, DateTime startDate, DateTime endDate)
    {
        IQueryable<Models.Domain.Leave> queryLeaves = _dbContext.Leave.OrderByDescending(leave => leave.CreatedAt).Include(leave => leave.Type).Include(leave => leave.User).ThenInclude(leaveType => leaveType.LeavesAuthorization).AsQueryable();
        queryLeaves = queryLeaves.Where(leave => leave.IdUser == idUser && leave.StartDate >= startDate && leave.EndDate <= endDate);
        return _mapper.Map<List<LeaveForUserResponseDto>>(await queryLeaves.ToListAsync());
    }

    public async Task<Models.Domain.Leave?> GetLeaveRequestById(Guid idLeave)
    {
        return await _dbContext.Leave.Include(leave => leave.Type).Include(leave => leave.User).ThenInclude(user => user.LeavesAuthorization).SingleOrDefaultAsync(leave => leave.Id == idLeave);
    }

    public async Task<ResponseAfterLeaveRequest> UpdateLeaveRequest(Models.Domain.Leave oldLeaveRequest, LeaveRequestDto newLeaveRequest)
    {
        oldLeaveRequest.Description = newLeaveRequest.Description;
        oldLeaveRequest.IdLeaveType = newLeaveRequest.IdLeaveType;
        oldLeaveRequest.StartDate = newLeaveRequest.StartDate;
        oldLeaveRequest.EndDate = newLeaveRequest.EndDate;
        oldLeaveRequest.UpdatedAt = DateTime.Now;
        await _dbContext.SaveChangesAsync();
        ResponseAfterLeaveRequest response = _mapper.Map<ResponseAfterLeaveRequest>(newLeaveRequest);
        response.IdLeave = oldLeaveRequest.Id;
        return response;
    }

    public async Task<PatchLeaveRequest> PatchLeaveRequest(Models.Domain.Leave leaveRequest, LeaveStatus newStatus)
    {
        leaveRequest.Status = newStatus;
        leaveRequest.UpdatedAt = DateTime.Now;
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<PatchLeaveRequest>(leaveRequest);
    }

    public async Task RefreshNonScheduledLeave()
    {
        await RefreshAllPendingApproval();
        await RefreshAllScheduled();
    }

    private async Task RefreshAllPendingApproval()
    {
        List<Models.Domain.Leave> oldLeaves = await _dbContext.Leave.Include(leave => leave.Type).Include(leave => leave.User).ThenInclude(leaveType => leaveType.LeavesAuthorization).Where(leave => leave.Status == LeaveStatus.PendingApproval && leave.StartDate < DateTime.Now).ToListAsync();

        foreach (var leave in oldLeaves)
            leave.Status = LeaveStatus.Rejected;

        await _dbContext.SaveChangesAsync();
    }

    private async Task RefreshAllScheduled()
    {
        List<Models.Domain.Leave> oldLeaves = await _dbContext.Leave.Include(leave => leave.Type).Include(leave => leave.User).ThenInclude(leaveType => leaveType.LeavesAuthorization).Where(leave => leave.Status == LeaveStatus.Scheduled && leave.StartDate < DateTime.Now).ToListAsync();

        foreach (var leave in oldLeaves)
        {
            leave.Status = LeaveStatus.Taken;
            RemoveSolde(leave);
        }
        await _dbContext.SaveChangesAsync();
    }

    public void RemoveSolde(Models.Domain.Leave leave)
    {
        LeaveAuthorization? userLeaveAuth = _dbContext.LeaveAuthorization.SingleOrDefault(leaveAuth => leaveAuth.IdLeaveType == leave.IdLeaveType && leaveAuth.IdUser == leave.IdUser);
        if (userLeaveAuth == null) return;
        userLeaveAuth.Solde -= DateUtils.GetDurationBetween(leave.StartDate, leave.EndDate);

        //CAS supposément impossible si LeaveService.AddLeaveRequest() GERE BIEN les soldes
        if (userLeaveAuth.Solde < 0) userLeaveAuth.Solde = 0.0M;
        _dbContext.SaveChanges();
    }

    public async Task<decimal> SumLeaveTypeDuration(Guid idUser, Guid idLeaveType)
    {
        return (await _dbContext.Leave.Where(leave => leave.IdUser == idUser && leave.IdLeaveType == idLeaveType && leave.Status == LeaveStatus.PendingApproval).Select(leave => DateUtils.GetDurationBetween(leave.StartDate, leave.EndDate)).ToListAsync()).Sum();
    }

    public async Task AffectLeave(Guid idUser)
    {
        List<LeaveType> leaveTypes = await _dbContext.LeaveType.ToListAsync();

        foreach( LeaveType actualLeaveType in leaveTypes )
        {
            await _dbContext.AddAsync( new LeaveAuthorization()
            {
                IdUser = idUser,
                IdLeaveType = actualLeaveType.Id,
                Solde = actualLeaveType.IsCumulable ? 0 : actualLeaveType.SoldeEachYear,
                StartValidity = DateTime.Now,
                EndValidity = DateTime.Now.AddYears(1),
                CreatedAt = DateTime.Now
            });
        }
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveLeaveAuth(Guid idUser)
    {
        _dbContext.RemoveRange(await _dbContext.LeaveAuthorization.Where( leaveAuth => leaveAuth.IdUser == idUser ).ToListAsync());
        await _dbContext.SaveChangesAsync();
    }
}