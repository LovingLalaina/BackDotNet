using System.ComponentModel.DataAnnotations;

namespace back_dotnet.Attributes
{
    public class UuidCollectionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is ICollection<string> uuids)
            {
                var invalidUuids = uuids.Where(uuid => !IsValidUuid(uuid)).ToList();

                if (invalidUuids.Any())
                {
                    var invalidUuidList = string.Join(", ", invalidUuids);
                    return new ValidationResult("");
                }
            }
            return ValidationResult.Success;
        }

        private bool IsValidUuid(string uuid)
        {
            return Guid.TryParse(uuid, out _);
        }
    }
}