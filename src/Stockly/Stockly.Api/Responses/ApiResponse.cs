namespace Stockly.Api.Responses;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public object? Errors { get; set; }
    public DateTime TimeStamp { get; set; }

    public static ApiResponse<T> Success(T data, string message = "Request successfully")
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data,
            Errors = null,
            TimeStamp = DateTime.UtcNow
        };
    }

    public static ApiResponse<T> Failure(object errors, string message = "Request failed")
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Data = default,
            Errors = errors,
            TimeStamp = DateTime.UtcNow
        };
    }
}
