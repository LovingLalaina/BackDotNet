using System.ComponentModel.DataAnnotations;

namespace back_dotnet.Attributes
{
    public class UuidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || !(value is string))
            {
                return false;
            }

            return Guid.TryParse((string)value, out var _);
        }
    }
}
