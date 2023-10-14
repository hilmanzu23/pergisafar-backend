public class ErrorResponse
{
    public int Code { get; set; }
    public string ErrorMessage { get; set; }

    public ErrorResponse(int errorCode, string errorMessage)
    {
        Code = errorCode;
        ErrorMessage = errorMessage;
    }
}