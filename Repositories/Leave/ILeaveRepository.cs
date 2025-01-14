

using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Leave;

namespace back_dotnet.Repositories.Leave;

public interface ILeaveRepository
{
    public Task<List<LeaveForAdminResponseDto>> GetAllLeavesWithAdmin(List<LeaveStatus> allFilters);

    public Task<List<LeaveForAdminResponseDto>> SearchLeavesWithAdmin(string search);

    public Task<LeaveType?> GetLeaveTypeById(Guid idLeaveType);

    public Task<ResponseAfterLeaveRequest> AddLeaveRequest(LeaveRequestDto leaveRequest);

    public Task<LeaveAuthorization?> GetLeaveAuthorization(Guid idUser, Guid idLeaveType);

    public Task<List<LeaveForUserResponseDto>> GetAllLeavesForUser(Guid idUser, List<LeaveStatus> allFilters);

    public Task<List<LeaveForUserResponseDto>> SearchLeavesForNow(Guid idUser);

    public Task<List<LeaveForUserResponseDto>> SearchLeavesContainedBetween(Guid idUser, DateTime startDate, DateTime endDate);

    public Task<Models.Domain.Leave?> GetLeaveRequestById(Guid idLeave);

    public Task<ResponseAfterLeaveRequest> UpdateLeaveRequest(Models.Domain.Leave newLeaveRequest, LeaveRequestDto newLeaveRequest1);

    public Task<PatchLeaveRequest> PatchLeaveRequest(Models.Domain.Leave leaveRequest, LeaveStatus newStatus);

    public Task RefreshNonScheduledLeave();

    public void RemoveSolde(Models.Domain.Leave leave);

    public Task<decimal> SumLeaveTypeDuration(Guid idUser, Guid idLeaveType);
    
    public Task AffectLeave(Guid idUser);

    public Task RemoveLeaveAuth(Guid idUser);
}