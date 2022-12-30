namespace Infrastructure.Services;

public static class UnixTimeHandler
{
    public static int UnixTimeToMonth(ulong unixTime)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        
        dateTime = dateTime.AddMilliseconds(unixTime).ToUniversalTime();

        var month = dateTime.Month;

        return month;
    }

    public static int UnixTimeToYear(ulong unixTime)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            
        dateTime = dateTime.AddMilliseconds(unixTime).ToUniversalTime();

        var year = dateTime.Year;
        
        return year;
    }
    
    public static DateTime UnixTimeToDateTime(ulong unixTime)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            
        dateTime = dateTime.AddMilliseconds(unixTime).ToUniversalTime();
    
        return dateTime;
    }
}
