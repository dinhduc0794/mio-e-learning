namespace Mio.ELearning.Core.ViewModels;

public class ResultViewModel<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;  
    public T? Data { get; set; }

    public static ResultViewModel<T> Success(T data, string message = "Success")
    {
        return new ResultViewModel<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };
    }

    public static ResultViewModel<T> Failure(string message)
    {
        return new ResultViewModel<T>
        {
            IsSuccess = false,
            Message = message,
            Data = default
        };
    }
}