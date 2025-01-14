using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs;
using back_dotnet.Models.DTOs.Leave;

namespace back_dotnet.Services.Leave;

public interface ILeaveService
{
    public Task<List<LeaveForAdminResponseDto>> GetAllLeavesWithAdmin(List<LeaveStatus> allFilters);

    public Task<List<LeaveForAdminResponseDto>> SearchLeavesWithAdmin(string search);

    public Task<ResponseAfterLeaveRequest> AddLeaveRequest(LeaveRequestDto leaveRequest);

    public Task<List<LeaveForUserResponseDto>> GetAllLeavesForUser(Guid idUser, List<LeaveStatus> allFilters);
    
    public Task<List<LeaveForUserResponseDto>> SearchLeavesDateForUser(Guid id, SearchLeaveDateDto searchLeaveDateDto);
    
    public Task<ResponseAfterLeaveRequest> UpdateLeaveRequest(Guid id, LeaveRequestDto leaveRequest);

    public Task<PatchLeaveRequest> PatchLeaveRequest(Guid id, LeaveStatus newStatus);
}