using Unity.Collections;
using Unity.Entities;

public partial class EventTypeListenerPair : IComponentData {
    public ComponentType EventType;
    public ComponentType ListenerType;
}