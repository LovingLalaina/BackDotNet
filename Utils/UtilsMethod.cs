using back_dotnet.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace back_dotnet.Utils
{
    public static class UtilsMethod
    {
        public static string CleanAndCapitalize(string input)
        {
            string cleaned = RemoveExtraSpaces(input);
            return CapitalizeString(cleaned);
        }

        public static string RemoveExtraSpaces(string input)
        {
            return Regex.Replace(input.Trim(), @"\s+", " ");
        }

        public static string CleanAndCapitalizeFirstLetter(string input)
        {
            var cleaned = RemoveExtraSpaces(input);
            return char.ToUpper(cleaned[0]) + cleaned.Substring(1).ToLowerInvariant();
        }

        public static string CapitalizeString(string input)
        {
            string[] words = input.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
                }
            }
            return string.Join(" ", words);
        }

        public static bool IsPostgresUniqueViolationError(DbUpdateException ex)
        {
            return ex.InnerException is Npgsql.PostgresException postgresEx && postgresEx.SqlState.StartsWith(TypeOrmErrorCode.DUPLICATED_FIELD);
        }

        public static int ConvertDayToHour( string durationJour )
        {
            try
            {
                return int.Parse(durationJour.TrimEnd('d')) * 24;
            }
            catch( Exception )
            {
                throw new NotSupportedException("La duree n'est pas au format 1d");
            }
        }
        public static bool CheckIfPasswordContainPersonalInformation(User user, string password)
        {
            var keyTest = new Dictionary<string, string>
            {
                { "firstname", user.Firstname },
                { "lastname", user.Lastname },
                { "email", user.Email }
            };

            var cleanedPassword = password.ToLower().Trim().Replace(" ", string.Empty);

            foreach (var key in keyTest.Values)
            {
                if (cleanedPassword.Contains(key.ToLower().Replace(" ", string.Empty)))
                {
                    return true;
                }
            }

            return false;
        }
        
        public static List<LeaveStatus> GetAllFilters(IQueryCollection queries)
        {
            List<LeaveStatus> allFilters = new List<LeaveStatus>();

            if(queries.ContainsKey(LeaveStatus.All.ToString()))
            {
                allFilters.Add( LeaveStatus.All );
                return allFilters;
            }

            if(queries.ContainsKey(LeaveStatus.PendingApproval.ToString()))
                allFilters.Add(LeaveStatus.PendingApproval);

            if(queries.ContainsKey(LeaveStatus.Scheduled.ToString()))
                allFilters.Add(LeaveStatus.Scheduled);

            if(queries.ContainsKey(LeaveStatus.Taken.ToString()))
                allFilters.Add(LeaveStatus.Taken);

            if(queries.ContainsKey(LeaveStatus.Cancelled.ToString()))
                allFilters.Add(LeaveStatus.Cancelled);

            if(queries.ContainsKey(LeaveStatus.Rejected.ToString()))
                allFilters.Add(LeaveStatus.Rejected);
                
            return allFilters;
        }
    }
}
