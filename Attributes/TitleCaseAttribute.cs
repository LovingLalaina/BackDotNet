using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace back_dotnet.Attributes;

public class TitleCaseAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        try
        {
            validationContext?
            .ObjectType?
            .GetProperty(validationContext.MemberName)?.SetValue(validationContext.ObjectInstance, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(value.ToString().Trim(), @"\s+", " ")), null);
        }
        catch (Exception)
        {
        }

        return null;
    }
}