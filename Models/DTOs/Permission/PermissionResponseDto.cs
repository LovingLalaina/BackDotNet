namespace back_dotnet.Models.DTOs.Permission
{
    public class PermissionResponseDto : MainDto
    {
        public ICollection<RoleResponseOnPermissionDto> Roles { get; set; }
    }
}
