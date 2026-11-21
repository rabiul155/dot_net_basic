namespace Dot_Net_Basic.Helpers;

/// <summary>Thrown when a request breaks a business rule. Converted to HTTP 400.</summary>
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}
