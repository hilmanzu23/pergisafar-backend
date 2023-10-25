using System.Text.Json;

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
                error = errorMessage
            }
        };
    }
}

public class ErrorMessageItem
{
    public string error { get; set; }
}

public class ErrorDto
{
    public int code { get; set; }
    public List<ErrorMessageItem> errorMessage { get; set; }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}