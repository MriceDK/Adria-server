namespace Adria.Domain.Shared.Exceptions;
public sealed class ElementNotFoundException : Exception
{
    public ElementNotFoundException(string message) : base(message)
    {
    }

    public ElementNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public static ElementNotFoundException ForId<T>(Guid id)
        => new($"Element of type {typeof(T).Name} with ID {id} not found.");
}