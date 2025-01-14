using back_dotnet.Exceptions;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.LeaveAuth;
using back_dotnet.Models.DTOs.Users;
using back_dotnet.Repositories.Leave;
using back_dotnet.Repositories.LeaveAuth;
using back_dotnet.Repositories.Users;


namespace back_dotnet.Services.LeaveAuth;

public class LeaveAuthService : ILeaveAuthService
{
    private readonly ILeaveAuthRepository _leaveAuthRepository;

    private readonly ILeaveRepository _leaveRepository;

    private readonly IUserRepository _userRepository;

    private readonly ILogger<LeaveAuthService> _logger;

    public LeaveAuthService( ILeaveAuthRepository leaveAuthRepository, ILogger<LeaveAuthService> logger, IUserRepository userRepository, ILeaveRepository leaveRepository )
    {
        _leaveAuthRepository = leaveAuthRepository;
        _logger = logger;
        _userRepository = userRepository;
        _leaveRepository = leaveRepository;
    }

    public async Task<List<LeaveAuthorizationResponseDto>> GetAllLeavesAuthorizationForUser(Guid idUser, SearchLeaveAuthDto searchLeaveAuthDto)
    {
        try
        {
            GetUserDto? currentUser = await _userRepository.GetById( idUser );
            if( currentUser == null )
                throw new HttpException( StatusCodes.Status404NotFound, "L'utilisateur (" + idUser + ") n'existe pas" );
                
            return await _leaveAuthRepository.GetLeavesAuthorizationForUser(idUser, searchLeaveAuthDto);
        }
        catch(Exception unknowknError)
        {
            HttpException? knownError = unknowknError as HttpException;
            if( knownError == null )
            {
                _logger.LogError(unknowknError, "Une erreur s'est produite lors de l'obtention de la liste des autorisation de l'utilisateur (" + idUser + ")" );
                throw new Exception();
            }
            throw knownError;
        }
    }

    public async Task<List<DatePeriodResponseDto>> GetAllDatePeriodForUser(Guid idUser)
    {
        try
        {
            GetUserDto? currentUser = await _userRepository.GetById( idUser );
            if( currentUser == null )
                throw new HttpException( StatusCodes.Status404NotFound, "L'utilisateur (" + idUser + ") n'existe pas" );

            return await _leaveAuthRepository.GetAllDatePeriodForUser(idUser);    
        }
        catch(Exception unknowknError)
        {
            HttpException? knownError = unknowknError as HttpException;
            if( knownError == null )
            {
                _logger.LogError(unknowknError, "Une erreur s'est produite lors de l'obtention de la période de date pour l'utilisateur (" + idUser + ")" );
                throw new Exception();
            }
            throw knownError;
        }  
    }

    public async Task<List<LeaveTypeDto>> GetAllLeaveType()
    {
        try
        {
            return await _leaveAuthRepository.GetAllLeaveType();
        }
        catch(Exception unknowknError)
        {
            _logger.LogError(unknowknError, "Une erreur s'est produite lors de l'obtention de la liste de tout les types de congé" );
            throw new Exception();
        }
    }

    public async Task<AssignedLeaveAuthResponse> AssignLeaveAuth(Guid iduser, List<LeaveTypeDto> leaveTypes)
    {
        try
        {
            await CheckAssignationLeaveAuth(iduser, leaveTypes);
            return  await _leaveAuthRepository.AssignLeaveAuth( iduser, leaveTypes);
        }
        catch(Exception unknowknError)
        {
            _logger.LogError(unknowknError, "Une erreur s'est produite lors de l'assignation d'authorisation de congé pour l'ustilisateur (" + iduser + ")" );
            throw new Exception();
        }
    }

    private async Task CheckAssignationLeaveAuth( Guid idUser, List<LeaveTypeDto> leaveTypes)
    {
        if(leaveTypes == null || leaveTypes.Count == 0 )
            throw new HttpException(StatusCodes.Status400BadRequest,"Veuillez fournir au moins un type de congé");

        GetUserDto? requestingUser = await _userRepository.GetById( idUser );
        if( requestingUser == null )
            throw new HttpException( StatusCodes.Status404NotFound, "L'utilisateur à assigner aux types de congés n'existe pas" );
        

        for (int i = leaveTypes.Count - 1; i >= 0; i--)
        {
            LeaveTypeDto actualLeaveType = leaveTypes[i];
            LeaveType? leaveTypeInDatabase = await _leaveRepository.GetLeaveTypeById( actualLeaveType.Id );
            if( leaveTypeInDatabase == null )
                throw new HttpException( StatusCodes.Status404NotFound, "Un des types de congé donné n'existe pas" );

            LeaveAuthorization? leaveAuthWithThisTypeInDatabase = await _leaveAuthRepository.GetLeaveAuth( idUser, actualLeaveType.Id);
            if (leaveAuthWithThisTypeInDatabase != null)
                leaveTypes.RemoveAt(i);
        }
    }
}