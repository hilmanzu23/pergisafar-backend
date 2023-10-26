public class CustomException : Exception
{
    public int ErrorCode { get; private set; }
    public string ErrorHeader { get; private set; }

    public CustomException(int errorCode, string header, string message) : base(message)
    {
        ErrorHeader = header;
        ErrorCode = errorCode;
    }
}