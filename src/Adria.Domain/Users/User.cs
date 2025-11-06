using Adria.Domain.Subcriptions;

namespace Adria.Domain.Users;

public sealed class User
{
    public User(string name, string job, Subscription subscription, Guid adrianId = default)
    {
        EnsureNameIsNotEmpty(name);
        EnsureJobIsNotEmpty(job);

        AdriaId = adrianId == Guid.Empty ? Guid.NewGuid() : adrianId;
        Name = name;
        Job = job;
        Subscription = subscription;
    }

    public Guid AdriaId { get; private init; }
    public string Name { get; set; }
    public string Job { get; set; }
    public Subscription Subscription { get; set; }


    private static void EnsureNameIsNotEmpty(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
    }

    private static void EnsureJobIsNotEmpty(string job)
    {
        if (string.IsNullOrWhiteSpace(job))
            throw new ArgumentException("Job cannot be null or empty.", nameof(job));
    }
}