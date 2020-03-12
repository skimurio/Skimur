using System.Collections.Generic;

namespace Skimur.Messaging
{
    /// <summary>
    /// Defines the object that exposes events that are ment to be published.
    /// </summary>
    public interface IEventPublisher
    {
        IEnumerable<IEvent> Events { get; }
    }
}
