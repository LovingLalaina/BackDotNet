namespace back_dotnet.Models.DTOs.LeaveAuth;

public class SearchLeaveAuthDto
{
    public string? LeaveType { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int GetSearchCase()
    {
        if( LeaveType == null && StartDate == null)
            return 0;
        if(LeaveType != null && StartDate == null)
            return 1;
        if( LeaveType == null && StartDate != null )
        {
            if( EndDate == null )   EndDate = ((DateTime)StartDate).AddYears(1);
            return 2;
        }
        return 3;
    }
}