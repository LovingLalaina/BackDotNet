
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.LeaveAuth;

namespace back_dotnet.Repositories.LeaveAuth;

public interface ILeaveAuthRepository
{
    public Task<List<LeaveAuthorizationResponseDto>> GetLeavesAuthorizationForUser(Guid idUser, SearchLeaveAuthDto searchLeaveAuthDto);

    public Task<List<DatePeriodResponseDto>> GetAllDatePeriodForUser(Guid idUser);

    public Task<List<LeaveTypeDto>> GetAllLeaveType();

    public Task<LeaveAuthorization?> GetLeaveAuth(Guid idUser, Guid idLeaveType);

    public Task<AssignedLeaveAuthResponse> AssignLeaveAuth(Guid iduser, List<LeaveTypeDto> leaveTypes);

    public Task ReinitializeNonCumulableAuth(LeaveAuthorization leaveAuth, decimal soldeEachYear);

    public Task AddSoldeTo(LeaveAuthorization leaveAuth);
}