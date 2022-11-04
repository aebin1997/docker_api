using System;
using Microsoft.AspNetCore.Http;

namespace Domain.Error;

public class BaseException : Exception
{
    /// <summary>
    /// 에러 코드
    /// </summary>
    private int _errorCode;

    public int ErrorCode
    {
        get => _errorCode;
    }

    public BaseException(int errorCode, int statusCode) : base()
    {
        _errorCode = errorCode;
    }
}

public class BadRequestException : BaseException
{
    /// <summary>
    /// HTTP Status Code 400
    /// </summary>
    /// <param name="errorCode">에러 코드</param>
    public BadRequestException(int errorCode) : base(errorCode, StatusCodes.Status400BadRequest)
    {
    }
}

public class UnauthorizedException: BaseException
{
    /// <summary>
    /// HTTP Status Code 401
    /// </summary>
    /// <param name="errorCode">에러 코드</param>
    public UnauthorizedException(int errorCode) : base(errorCode, StatusCodes.Status401Unauthorized)
    {
    }
}

public class ForbiddenException : BaseException
{
    /// <summary>
    /// HTTP Status Code 403
    /// </summary>
    /// <param name="errorCode">에러 코드</param>
    public ForbiddenException(int errorCode) : base(errorCode, StatusCodes.Status403Forbidden)
    {
    }
}

public class NotFoundException : BaseException
{
    /// <summary>
    /// HTTP Status Code 404
    /// </summary>
    /// <param name="errorCode">에러 코드</param>
    public NotFoundException(int errorCode) : base(errorCode, StatusCodes.Status404NotFound)
    {
    }
}

public class InternalServerErrorException : BaseException
{
    /// <summary>
    /// HTTP Status Code 500
    /// </summary>
    /// <param name="errorCode">에러 코드</param>
    public InternalServerErrorException(int errorCode) : base(errorCode, StatusCodes.Status500InternalServerError)
    {
    }
}