namespace Dot_Net_Basic.Helpers;

/// <summary>Thrown when a requested record does not exist. Converted to HTTP 404.</summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string entityName, object id)
        : base($"{entityName} with id '{id}' was not found.")
    {
    }
}
