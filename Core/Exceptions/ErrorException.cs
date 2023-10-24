public class ErrorResponse
{
    public int Code { get; set; }
    public List<ErrorMessageItem> ErrorMessage { get; set; }

    public ErrorResponse(int errorCode, string errorMessage)
    {
        Code = errorCode;
        ErrorMessage = new List<ErrorMessageItem>
        {
            new ErrorMessageItem
            {
                Error = errorMessage
            }
        };
    }
}

public class ErrorMessageItem
{
    public string Error { get; set; }
}