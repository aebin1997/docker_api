namespace Infrastructure.Models.Return;

public class DefaultReturn
{
    public bool IsSuccess { get; set; }

    public int ErrorCode { get; set; }

    public DefaultReturn(bool isSuccess, int errorCode)
    {
        IsSuccess = isSuccess;
        ErrorCode = errorCode;
    }
}