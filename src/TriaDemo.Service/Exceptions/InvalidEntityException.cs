namespace TriaDemo.Service.Exceptions;

public sealed class InvalidEntityException(string message) : Exception(message);
