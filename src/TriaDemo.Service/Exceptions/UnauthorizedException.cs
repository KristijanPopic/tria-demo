namespace TriaDemo.Service.Exceptions;

public sealed class UnauthorizedException(string message) : Exception(message)
{
    
}