namespace back_dotnet.Utils
{
    public class ErrorResponse<T>
    {
        public int Status { get; set; }
        public T Error { get; set; }
        public ErrorResponse(int status, T errors)
        {
            Status = status;
            Error = errors;
        }
    }
}
