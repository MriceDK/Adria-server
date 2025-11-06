namespace Adria.Infrastructure.Persistence.Shared;

public sealed class NutriscanDatabaseException(string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public NutriscanDatabaseException()
        : this("An error occurred while accessing the nutriscan database.")
    {
    }

    public NutriscanDatabaseException(Exception innerException)
        : this("An error occurred while accessing the nutriscan database.", innerException)
    {
    }
}

