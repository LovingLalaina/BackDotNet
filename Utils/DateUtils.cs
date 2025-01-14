
namespace back_dotnet.Utils;
public static class DateUtils
{
    private static string[] FrenchMonth = {
        "Jan", "Fév", "Mar", "Avr", "Mai", "Juin", "Juil", "Aoû", "Sep", "Oct", "Nov", "Déc"
    };

    public static string GetDatePeriod(DateTime startDate, DateTime endDate)
    {
        return ToFrenchFormat(startDate) + " à " + ToFrenchFormat(endDate);
    }

    private static string ToFrenchFormat(DateTime date)
    {
        return $"{date.Day:00} {FrenchMonth[date.Month - 1]} {date.Year}";
    }

    public static decimal GetDurationBetween(DateTime startDate, DateTime endDate)
    {
        decimal difference = (decimal)(endDate - startDate).TotalDays;
        if (difference < 0)
            throw new ArgumentOutOfRangeException(nameof(endDate), "La date de fin d'un congé (" + endDate + ") doit être supérieure à la date de début (" + startDate + ")");
        difference -= CountSundaysBetween(startDate, endDate);
        return Math.Ceiling(difference * 2) / 2;
    }

    private static int CountSundaysBetween(DateTime startDate, DateTime endDate)
    {
        int sundayCount = 0;
        DateTime currentDate = startDate;

        while (currentDate <= endDate)
        {
            if (currentDate.DayOfWeek == DayOfWeek.Sunday)
                ++sundayCount;
            currentDate = currentDate.AddDays(1);
        }

        return sundayCount;
    }
}
