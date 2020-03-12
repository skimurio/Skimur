namespace Skimur.Messaging.Handling
{
    /// <summary>
    /// Marker interface that makes it easier to discover handlers via relection
    /// </summary>
    public interface IEventHandler { }

    public interface IEventHandler<in T> : IEventHandler
    {
        void Handle(T @event);
    }
}
