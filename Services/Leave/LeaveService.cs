using back_dotnet.Exceptions;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Leave;
using back_dotnet.Models.DTOs.Users;
using back_dotnet.Repositories.Leave;
using back_dotnet.Repositories.Users;
using back_dotnet.Services.Scheduler;
using back_dotnet.Utils;


namespace back_dotnet.Services.Leave;

public class LeaveService : ILeaveService
{
    private readonly ILeaveRepository _leaveRepository;

    private readonly IUserRepository _userRepository;

    private readonly ILogger<LeaveService> _logger;

    private readonly LeaveScheduler _leaveScheduler;

    public LeaveService(ILeaveRepository leaveRepository, ILogger<LeaveService> logger, IUserRepository userRepository, LeaveScheduler leaveScheduler)
    {
        _leaveRepository = leaveRepository;
        _logger = logger;
        _userRepository = userRepository;
        _leaveScheduler = leaveScheduler;
    }
    public async Task<List<LeaveForAdminResponseDto>> GetAllLeavesWithAdmin(List<LeaveStatus> allFilters)
    {
        try
        {
            return await _leaveRepository.GetAllLeavesWithAdmin(allFilters);
        }
        catch(Exception unknowknError)
        {
            _logger.LogError(unknowknError, "Une erreur s'est produite lors de l'obtention de la liste des congés pour l'administrateur de congé");
            throw new Exception();
        }
    }

    public async Task<List<LeaveForAdminResponseDto>> SearchLeavesWithAdmin(string search)
    {
        try
        {
            return await _leaveRepository.SearchLeavesWithAdmin(search);
        }
        catch(Exception unknowknError)
        {
            _logger.LogError(unknowknError, "Une erreur s'est produite lors de la recherche de congés pour l'administrateur de congé");
            throw new Exception();
        }
    }

    public async Task<ResponseAfterLeaveRequest> AddLeaveRequest(LeaveRequestDto leaveRequest)
    {
        //404 SI USER OU LEAVE_TYPE NON TROUVE
        //401 SI SOLDE INSUFFISANT OU START_DATE TROIS JOURS PLUS TOT QUE NOW()
        //422 SI DATE NON LOGIQUES : endDate <= startDate
        try
        {
            await CheckLeaveRequest(leaveRequest);
            ResponseAfterLeaveRequest addedLeaveRequest = await _leaveRepository.AddLeaveRequest(leaveRequest);
            await _leaveScheduler.ScheduleLeave(addedLeaveRequest);
            return addedLeaveRequest;
        }
        catch(Exception unknowknError)
        {
            HttpException? knownError = unknowknError as HttpException;
            if( knownError == null )
            {
                _logger.LogError(unknowknError, "Une erreur s'est produite lors de la demande de congés de l'utilisateur (id=" + leaveRequest.IdUser + ")" );
                throw new Exception();
            }
            throw knownError;
        }
    }

    private async Task CheckLeaveRequest( LeaveRequestDto leaveRequest)
    {
        if( leaveRequest.StartDate.CompareTo(DateTime.Now.AddDays(2)) <= 0 )
            throw new HttpException( StatusCodes.Status401Unauthorized, "La demande de congé doit être effectué au moins 3 jours avant" );
            
        GetUserDto? requestingUser = await _userRepository.GetById( leaveRequest.IdUser );
        if( requestingUser == null )
            throw new HttpException( StatusCodes.Status404NotFound, "L'utilisateur qui doit faire la demande n'existe pas" );
        
        LeaveType? leaveType = await _leaveRepository.GetLeaveTypeById( leaveRequest.IdLeaveType );
        if( leaveType == null )
            throw new HttpException( StatusCodes.Status404NotFound, "Le type de congé donné n'existe pas" );
        
        LeaveAuthorization? leaveAuthorization = await _leaveRepository.GetLeaveAuthorization(leaveRequest.IdUser, leaveRequest.IdLeaveType);
        if( leaveAuthorization == null )
            throw new HttpException( StatusCodes.Status500InternalServerError, "L'utilisateur et le type de congé ne sont pas associé" );

        decimal requestedDays;
        try
        {
            requestedDays = DateUtils.GetDurationBetween( leaveRequest.StartDate, leaveRequest.EndDate);
        }
        catch(ArgumentOutOfRangeException dateException)
        {
            throw new HttpException( StatusCodes.Status422UnprocessableEntity, dateException.Message );
        }

        decimal solde = leaveAuthorization.Solde + await _leaveRepository.SumLeaveTypeDuration(leaveRequest.IdUser, leaveRequest.IdLeaveType);
        if (requestedDays > solde)
            throw new HttpException(StatusCodes.Status401Unauthorized, "Votre solde (" + solde + ") pour ce type de congé est insuffisant. Veuillez en choisir un autre type ou diminuer le nombre de jour");
        leaveRequest.ActualSolde = solde;
    }

    public async Task<List<LeaveForUserResponseDto>> GetAllLeavesForUser(Guid idUser, List<LeaveStatus> allFilters)
    {
        try
        {
            GetUserDto? currentUser = await _userRepository.GetById( idUser );
            if( currentUser == null )
                throw new HttpException( StatusCodes.Status404NotFound, "L'utilisateur (" + idUser + ") n'existe pas" );
            return await _leaveRepository.GetAllLeavesForUser(idUser, allFilters);
        }
        catch(Exception unknowknError)
        {
            HttpException? knownError = unknowknError as HttpException;
            if( knownError == null )
            {
                _logger.LogError(unknowknError, "Une erreur s'est produite lors de la demande de congés de l'utilisateur (id=" + idUser + ")," );
                throw new Exception();
            }
            throw knownError;
        }
    }

    public async Task<List<LeaveForUserResponseDto>> SearchLeavesDateForUser(Guid idUser, SearchLeaveDateDto searchLeaveDateDto)
    {
        try
        {
            GetUserDto? currentUser = await _userRepository.GetById( idUser );
            if( currentUser == null )
                throw new HttpException( StatusCodes.Status404NotFound, "L'utilisateur (" + idUser + ") n'existe pas" );
            
            if( searchLeaveDateDto.IsNowSelected is not null && (bool)searchLeaveDateDto.IsNowSelected )
                return await _leaveRepository.SearchLeavesForNow(idUser);
            if( searchLeaveDateDto.StartDate == null || searchLeaveDateDto.EndDate == null )
                throw new HttpException( StatusCodes.Status400BadRequest, "Veuillez fournir les dates pour la recherche ou sélectionner aujourd'hui" );
            return await _leaveRepository.SearchLeavesContainedBetween(idUser, (DateTime)searchLeaveDateDto.StartDate, (DateTime)searchLeaveDateDto.EndDate);
        }
        catch(Exception unknowknError)
        {
            HttpException? knownError = unknowknError as HttpException;
            if( knownError == null )
            {
                _logger.LogError(unknowknError, "Une erreur s'est produite lors de la recherche de congés avec le filtre de date (id_user=" + idUser + ")" );
                throw new Exception();
            }
            throw knownError;
        }
    }

    public async Task<ResponseAfterLeaveRequest> UpdateLeaveRequest(Guid idLeave, LeaveRequestDto newLeaveRequest)
    {
        //404 SI Leave introuvable
        //401 SI Status != PendingApproval
        try
        {
            Models.Domain.Leave? oldLeaveRequest = await _leaveRepository.GetLeaveRequestById(idLeave);
            if( oldLeaveRequest == null)
                throw new HttpException( StatusCodes.Status404NotFound, "Le congé à modifier n'existe pas" );

            bool containImportantChanges = newLeaveRequest.ContainsDateOrTypeChanges(oldLeaveRequest);
            if (containImportantChanges)
            {
                await CheckLeaveRequest(newLeaveRequest);
                if( oldLeaveRequest.Status != LeaveStatus.PendingApproval )
                    throw new HttpException( StatusCodes.Status401Unauthorized, "Modification refusée car le congé n'est plus en attente d'approbation (" + oldLeaveRequest.Status + ")" );
            }

            ResponseAfterLeaveRequest updatedLeaveRequest = await _leaveRepository.UpdateLeaveRequest(oldLeaveRequest, newLeaveRequest);
            if (containImportantChanges)
                await _leaveScheduler.ScheduleLeave(updatedLeaveRequest);
            return updatedLeaveRequest;
        }
        catch(Exception unknowknError)
        {
            HttpException? knownError = unknowknError as HttpException;
            if( knownError == null )
            {
                _logger.LogError(unknowknError, "Une erreur s'est produite lors de la modification du congé (id=" + idLeave + ")," );
                throw new Exception();
            }
            throw knownError;
        }
    }

    public async Task<PatchLeaveRequest> PatchLeaveRequest(Guid idLeave, LeaveStatus newStatus)
    {
        try
        {
            //4O1 SI STATUS != PendingApproval ou Scheduled
            Models.Domain.Leave? leaveRequest = await _leaveRepository.GetLeaveRequestById(idLeave);
            if( leaveRequest == null)
                throw new HttpException( StatusCodes.Status404NotFound, "Le congé à mettre à jour n'existe pas" );

            CheckPatchingLeave(leaveRequest.Status, newStatus);
            
            return await _leaveRepository.PatchLeaveRequest(leaveRequest, newStatus);
        }
        catch(Exception unknowknError)
        {
            HttpException? knownError = unknowknError as HttpException;
            if( knownError == null )
            {
                _logger.LogError(unknowknError, "Une erreur s'est produite lors de la mise à jour du congé (id=" + idLeave + ")," );
                throw new Exception();
            }
            throw knownError;
        }
    }

    private void CheckPatchingLeave( LeaveStatus oldStatus, LeaveStatus newStatus)
    {
        if (!new List<LeaveStatus> { LeaveStatus.Scheduled, LeaveStatus.Cancelled, LeaveStatus.Rejected }.Contains(newStatus))
            throw new HttpException(StatusCodes.Status400BadRequest, "Un congé ne peut etre qu'annulé, refusé ou programmé");

        if (newStatus == LeaveStatus.Cancelled)
            if (oldStatus != LeaveStatus.PendingApproval && oldStatus != LeaveStatus.Scheduled)
                throw new HttpException(StatusCodes.Status401Unauthorized, "Seul un congé en attente d'approbation ou programmé peut être annulé");

        if (newStatus == LeaveStatus.Rejected || newStatus == LeaveStatus.Scheduled)
            if (oldStatus != LeaveStatus.PendingApproval)
                throw new HttpException(StatusCodes.Status401Unauthorized, "Seul un congé en attente d'approbation peut être accepté ou refusé");
    }
}