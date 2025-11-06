namespace CityWeathers.Application.Services;

public class Result<TResult> where TResult : class
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public TResult? Value { get; set; }

    private Result(bool isSuccess = true, TResult? value = null, string? errorMessage = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
    }
    
    public static Result<TResult> Success(TResult value) => new Result<TResult>(true, value);
    
    public static Result<TResult> Failure(string errorMessage) => new Result<TResult>(false, null, errorMessage);
}