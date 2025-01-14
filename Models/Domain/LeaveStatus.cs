namespace back_dotnet.Models.Domain;

public enum LeaveStatus
{
    PendingApproval, //En attente d'approbation == 0
    Scheduled,      //Programmé == 1
    Taken,          //Pris == 2
    Cancelled,      //Annulé == 3
    Rejected,       //Rejeté ou Refusé == 4
    All             //Tous (Utilisé pour afficher n'importe quels congé) == 5
}