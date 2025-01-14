using back_dotnet.Models.DTOs.LeaveAuth;

namespace back_dotnet.Services.LeaveAuth;

public interface ILeaveAuthService
{
    public Task<List<LeaveAuthorizationResponseDto>> GetAllLeavesAuthorizationForUser(Guid id, SearchLeaveAuthDto searchLeaveAuthDto);

    public Task<List<DatePeriodResponseDto>> GetAllDatePeriodForUser(Guid id);
    
    public Task<List<LeaveTypeDto>> GetAllLeaveType();
    
    public Task<AssignedLeaveAuthResponse> AssignLeaveAuth(Guid id, List<LeaveTypeDto> leaveTypes);
}