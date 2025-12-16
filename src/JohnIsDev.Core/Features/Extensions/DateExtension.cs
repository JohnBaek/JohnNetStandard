using System.Diagnostics.CodeAnalysis;

namespace JohnIsDev.Core.Features.Extensions;


/// <summary>
/// Extension for DateTime
/// </summary>
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public static class DateExtension
{
   /// <summary>
   /// 
   /// </summary>
   /// <param name="from"></param>
   /// <param name="to"></param>
   /// <returns></returns>
   /// <exception cref="ArgumentException"></exception>
    public static HashSet<DateTime> GetBusinessDays(this DateTime from, DateTime to)
    {
        // Validate for if it's 'to' is more than 'from'
        if(from < to)
            throw new ArgumentException("The 'from' date must be earlier than or equal to the 'to' date.");
            
        // Initiates a from to current date
        DateTime current = from;
        HashSet<DayOfWeek> noneBusinessDays = [ DayOfWeek.Saturday, DayOfWeek.Sunday ];
        var result = new HashSet<DateTime>();

        // Iterates form and to
        while (current <= to)
        {
            DayOfWeek currentDay = current.DayOfWeek;

            // Checks current day includes it not in Business Day
            if (!noneBusinessDays.Contains(currentDay))
            {
                result.Add(current);                    
            }
                
            // Cursor moves to next Day
            current = current.AddDays(1);
        }
        return result;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="to"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static HashSet<DateTime> GetBusinessDaysForToday(this DateTime to)
    {
        return GetBusinessDays(DateTime.Now, to);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="to"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static int GetBusinessDaysForTodayCount(this DateTime to)
    {
        return GetBusinessDays(DateTime.Now, to).Count;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <param name="businessDays"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static DateTime SubtractBusinessDays(this DateTime date, int businessDays)
    {
        if (businessDays < 0)
            throw new ArgumentException("businessDays must be a non-negative integer.");

        int daysSubtracted = 0;

        while (daysSubtracted < businessDays)
        {
            date = date.AddDays(-1); // 하루씩 감소
            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
            {
                daysSubtracted++;
            }
        }

        return date;
    }
}