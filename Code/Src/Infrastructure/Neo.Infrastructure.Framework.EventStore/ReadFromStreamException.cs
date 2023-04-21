namespace Neo.Infrastructure.EventStore;

public class ReadFromStreamException : Exception
{
    public ReadFromStreamException(string message, Exception exception) :
        base(message, exception)
    {

    }
}