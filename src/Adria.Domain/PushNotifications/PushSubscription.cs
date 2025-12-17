    namespace Adria.Domain.PushNotifications;

    public sealed class PushSubscription
    {
        public PushSubscription(Guid userId, string endpoint, string p256dh, string auth, Guid id = default)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            UserId = userId;
            Endpoint = endpoint;
            P256dh = p256dh;
            Auth = auth;
        }

        public Guid Id { get; private init; }
        public Guid UserId { get; private set; }
        public string Endpoint { get; private set; }
        public string P256dh { get; private set; }
        public string Auth { get; private set; }
    }