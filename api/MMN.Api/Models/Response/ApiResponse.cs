using Microsoft.AspNetCore.Mvc;

namespace MMN.Api.Models.Response;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public ProblemDetails ErrorDetails { get; set; }

    public ApiResponse()
    {
        Success = true;
        Message = null;
        Data = default(T);
        ErrorDetails = null;
    }

    public ApiResponse<T> SuccessResponse(T data, string message = null)
    {
        Success = true;
        Message = message;
        Data = data;
        ErrorDetails = null;
        return this;
    }

    public ApiResponse<T> ErrorResponse(string errorMessage, int statusCode = 500, string detail = null, string type = null, string instance = null)
    {
        Success = false;
        Message = errorMessage;
        Data = default(T);
        ErrorDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = errorMessage,
            Detail = detail,
            Type = type,
            Instance = instance
        };
        return this;
    }
}

