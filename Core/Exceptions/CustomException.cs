public class CustomException : Exception
{
    public int ErrorCode { get; private set; }

    public CustomException(int errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}