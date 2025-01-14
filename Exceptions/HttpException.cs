namespace back_dotnet.Exceptions
{
    public class HttpException : Exception
    {
        public int Status { get; set; }
        public string error { get; set; } = null!;
        public HttpException(int status, string error) : base(error)
        {
            this.Status = status;
        }
    }
}