namespace TriaDemo.RestApi.Extensions;

public static class DateTimeExtensions
{
    public static DateTime? ToUtcDateTimeOrNull(this DateTime? dateTime)
    {
        if (!dateTime.HasValue)
        {
            return null;
        }

        return DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc);
    }
}