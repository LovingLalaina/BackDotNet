namespace back_dotnet.Models.DTOs.Role
{
    public class RoleResponseDto : MainDto
    {
        public ICollection<PermissionResponseOnRoleDto> Permissions { get; set; } = null;
        public ICollection<DepartmentResponseOnRoleDto> Departments { get; set; } = null;
        public Boolean IsSeed { get; set; } = false;
    }
}
