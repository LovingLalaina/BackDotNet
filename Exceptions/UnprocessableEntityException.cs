using back_dotnet.ErrorsHandler;

namespace back_dotnet.Exceptions
{
    //NOUVELLE CLASSE POUR GERER LA VALIDATION DE CheckIfPasswordContainPersonalInformation de ResetPassword
    public class UnprocessableEntityException : HttpException
    {
        public List<ValidationError> constraints { get; set; }
        public UnprocessableEntityException(int status, List<ValidationError> constraints) : base(status,"")
        {
            this.constraints = constraints;
        }
    }
}