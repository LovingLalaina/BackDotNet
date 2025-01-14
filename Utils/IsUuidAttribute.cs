using System.ComponentModel.DataAnnotations;

namespace back_dotnet.Utils
{
    public class IsUuidAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is not null && Guid.TryParse(value.ToString(), out _))
            {
                return true;
            }

            return false;
        }
    }
}