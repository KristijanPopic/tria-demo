namespace TriaDemo.Service.Exceptions;

public sealed class NotUniqueException(string message) : Exception(message);