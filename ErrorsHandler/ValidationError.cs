namespace back_dotnet.ErrorsHandler;

//CLASSE POUR SIMULER Le TryValidateModel SI PASSWORD CONTAINS PERSONAL INFORMATIONS
public class ValidationError
{
    public string? Property { get; set; }
    public Dictionary<string, string>? Constraints { get; set; }
}