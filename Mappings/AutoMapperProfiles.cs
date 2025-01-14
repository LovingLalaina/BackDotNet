using AutoMapper;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs;
using back_dotnet.Models.DTOs.Chat;
using back_dotnet.Models.DTOs.Department;
using back_dotnet.Models.DTOs.Files;
using back_dotnet.Models.DTOs.Leave;
using back_dotnet.Models.DTOs.LeaveAuth;
using back_dotnet.Models.DTOs.Permission;
using back_dotnet.Models.DTOs.Post;
using back_dotnet.Models.DTOs.Role;
using back_dotnet.Models.DTOs.Users;
using back_dotnet.Utils;
using File = back_dotnet.Models.Domain.File;

namespace back_dotnet.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<DepartmentDto, Department>().ReverseMap();
            CreateMap<RoleDepartmentDto, Role>().ReverseMap();

            //User
            CreateMap<User, GetUserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();


            CreateMap<Permission, UserPostDepartmentRolePermissionDto>().ReverseMap();
            //File
            CreateMap<File, GetFileDto>().ReverseMap();
            CreateMap<File, CreateFileDto>().ReverseMap();

            //Role
            CreateMap<Role, CreateRoleDto>().ReverseMap();
            CreateMap<Role, UpdateRoleDto>().ReverseMap();
            CreateMap<Role, UserPostDepartmentRoleDto>()
              .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.PermissionRoles.Select(pr => new UserPostDepartmentRolePermissionDto
              {
                  Id = pr.IdPermissionNavigation.Id,
                  Name = pr.IdPermissionNavigation.Name,
                  CreatedAt = pr.IdPermissionNavigation.CreatedAt,
                  UpdatedAt = pr.IdPermissionNavigation.UpdatedAt
              })));
            CreateMap<Role, RoleResponseDto>()
              .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.PermissionRoles.Select(pr => new PermissionResponseOnRoleDto
              {
                  Id = pr.IdPermissionNavigation.Id,
                  Name = pr.IdPermissionNavigation.Name,
                  CreatedAt = pr.IdPermissionNavigation.CreatedAt,
                  UpdatedAt = pr.IdPermissionNavigation.UpdatedAt
              })))
              .ForMember(dest => dest.Departments, opt => opt.MapFrom(src => src.Departments.Select(rd => new DepartmentResponseOnRoleDto
              {
                  Id = rd.Id,
                  Name = rd.Name,
                  CreatedAt = rd.CreatedAt,
                  UpdatedAt = rd.UpdatedAt
              })));

            //Post
            CreateMap<PostDto, Post>().ReverseMap();
            CreateMap<UserPostDto, Post>().ReverseMap();
            CreateMap<PostDepartmentDto, Department>().ReverseMap();
            CreateMap<UserPostDepartmentDto, Department>().ReverseMap();
            CreateMap<Post, SearchPostDto>()
            .ForMember( dto => dto.Department, opt => opt.MapFrom( domain => new DepartmentForPostDto(domain.IdDepartmentNavigation)) )
            .ReverseMap();

            //Permission
            CreateMap<Permission, CreateOrUpdatePermissionDto>().ReverseMap();
            CreateMap<Permission, PermissionResponseDto>()
               .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.PermissionRoles.Select(pr => new RoleResponseOnPermissionDto
               {
                   Id = pr.IdRoleNavigation.Id,
                   Name = pr.IdRoleNavigation.Name,
                   CreatedAt = pr.IdRoleNavigation.CreatedAt,
                   UpdatedAt = pr.IdRoleNavigation.UpdatedAt
               })));

            //Leave
            CreateMap<Leave, LeaveForAdminResponseDto>()
                .ForMember(dto => dto.Duration, opt => opt.MapFrom(domain => DateUtils.GetDurationBetween(domain.StartDate, domain.EndDate)))
                .ForMember(dto => dto.Matricule, traitement => traitement.MapFrom(leaveDomain => leaveDomain.User.Matricule))
                .ForMember(dto => dto.Lastname, traitement => traitement.MapFrom(leaveDomain => leaveDomain.User.Lastname))
                .ForMember(dto => dto.Type, traitement => traitement.MapFrom(leaveDomain => leaveDomain.Type.Designation))
                .ForMember(dto => dto.Solde, traitement => traitement.MapFrom(leaveDomain => GetSoldeFor(leaveDomain)));
            
            CreateMap<LeaveRequestDto, Leave>()
                .ForMember( domain => domain.Status, traitement => traitement.MapFrom( dto => LeaveStatus.PendingApproval))
                .ForMember( domain => domain.StartDate, traitement => traitement.MapFrom(dto => dto.StartDate.ToUniversalTime()))
                .ForMember( domain => domain.EndDate, traitement => traitement.MapFrom(dto => dto.EndDate.ToUniversalTime()));
            
            CreateMap<LeaveRequestDto, ResponseAfterLeaveRequest>()
                .ForMember( response => response.SoldeAfter, traitement => traitement.MapFrom( request => GetSoldeAfterRequest(request)));

            CreateMap<Leave, LeaveForUserResponseDto>()
                .ForMember( dto => dto.DatePeriod, traitement => traitement.MapFrom(domain => DateUtils.GetDatePeriod(domain.StartDate,domain.EndDate)))
                .ForMember(dto => dto.Type, traitement => traitement.MapFrom(leaveDomain => leaveDomain.Type.Designation))
                .ForMember(dto => dto.SoldeForType, traitement => traitement.MapFrom(leaveDomain => GetSoldeFor(leaveDomain)))
                .ForMember(dto => dto.Duration, traitement => traitement.MapFrom(leaveDomain => DateUtils.GetDurationBetween(leaveDomain.StartDate, leaveDomain.EndDate)));
            CreateMap<LeaveAuthorization, LeaveAuthorizationResponseDto>()
                .ForMember( dto => dto.Designation, opt => opt.MapFrom( domain => domain.LeaveType.Designation))
                .ForMember( dto => dto.IdLeaveType, opt => opt.MapFrom( domain => domain.LeaveType.Id))
                .ForMember( dto => dto.Description, opt => opt.MapFrom( domain => domain.LeaveType.Description));
            CreateMap<LeaveAuthorization, DatePeriodResponseDto>()
                .ForMember( dto => dto.DatePeriod, opt => opt.MapFrom( domain => DateUtils.GetDatePeriod(domain.StartValidity,domain.EndValidity)));
            CreateMap<Leave, PatchLeaveRequest>();  
            CreateMap<LeaveType, LeaveTypeDto>();

            //Chat
            CreateMap<Discussion, GetDiscussionDto>()
            .ForMember( dto => dto.Title, opt => opt.MapFrom( (domain, dto, _, context) => 
                GetDiscussionTitle(domain, GetIdUserReader(context))))
            .ForMember( dto => dto.Messages, opt => opt.MapFrom( (domain, dto, _, context) =>
                GetMessagesForDiscussion(domain, GetIdUserReader(context))))
            .ReverseMap();

            CreateMap<SendMessageDto, Message>()
            .ForMember(domain => domain.IdUser, opt => opt.MapFrom(dto => dto.IdUserSender))
            .ReverseMap();

            CreateMap<CreateGroupDto, Discussion>()
            .ForMember(domain => domain.ParticipantNumber, opt => opt.MapFrom(dto => dto.IdsParticipants.Count))
            .ReverseMap();
        }

        private Guid GetIdUserReader(ResolutionContext context)
        {
            Guid? idUserReader = context.Items["IdUserReader"] as Guid?;
            if( idUserReader == null )
                throw new Exception( "L'identifiant de l'utilisateur auquel fournir la discussion n'est pas fournie pour le mapping" );
            return (Guid) idUserReader;
        }

        private static decimal GetSoldeAfterRequest(LeaveRequestDto leaveRequest)
        {
            decimal? actualSolde = leaveRequest.ActualSolde;
            if( actualSolde == null )
                throw new Exception( "Erreur lors du calcul du nouveau solde");
            return (decimal)actualSolde - DateUtils.GetDurationBetween( leaveRequest.StartDate,leaveRequest.EndDate );
        }

        private static decimal GetSoldeFor(Leave leaveDomain)
        {
            LeaveAuthorization? leaveAuthorization = leaveDomain.User.LeavesAuthorization.SingleOrDefault( leaveAuthorization => leaveAuthorization.IdUser ==leaveDomain.IdUser && leaveAuthorization.IdLeaveType == leaveDomain.IdLeaveType);
            if( leaveAuthorization == null )
                throw new Exception( "Solde du congé non trouvé");
            return leaveAuthorization.Solde;
        }

        private string GetDiscussionTitle(Discussion discussion, Guid idUserReader)
        {
            if(discussion.ParticipantNumber < 2)
                throw new Exception($"Le nombre de participant de la discussion ({discussion.Id})est inférieur à 2");

            if (discussion.ParticipantNumber > 2)
                return discussion.Title;

            User? interlocutor = discussion.UserDiscussions.ToList()
            .Select( userDiscussion => userDiscussion.User)
            .SingleOrDefault( user => user.Uuid != idUserReader);

            if(interlocutor == null)
                throw new Exception($"Aucun interlocuteur trouvé dans la discussion ({discussion.Id}) de l'utilisateur ({idUserReader})");

            return $"{interlocutor.Firstname} {interlocutor.Lastname}";
        }

        private List<GetMessageDto> GetMessagesForDiscussion(Discussion discussion, Guid idUserReader)
        {
            return discussion.Messages
            .OrderBy( message => message.CreatedAt )
            .Select( message => MessageMapper
            .MapMessage( message , idUserReader ) )
            .ToList();
        }
    }
}