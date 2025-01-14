using back_dotnet.Utils;
using Microsoft.AspNetCore.Mvc;

namespace back_dotnet.ErrorsHandler
{
    public static class HttpHandlerError
    {
        public static ActionResult InternalServer()
        {
            return new ObjectResult(new ErrorResponse<string>(StatusCodes.Status500InternalServerError, "Internal server error"))
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}
