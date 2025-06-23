namespace Application.Helpers;

public static class DateTimeHelper
{
    public static DateTime TruncateToSeconds(this DateTime dateTime)
    {
        return new DateTime(dateTime.Ticks - (dateTime.Ticks % TimeSpan.TicksPerSecond), dateTime.Kind);
    }
}