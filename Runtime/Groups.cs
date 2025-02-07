using Unity.Entities;

public partial class EventsGroup : ComponentSystemGroup { }

[UpdateInGroup(typeof(EventsGroup))]
public partial class DispatcherGroup : ComponentSystemGroup { }

[UpdateInGroup(typeof(EventsGroup))]
[UpdateAfter(typeof(DispatcherGroup))]
public partial class ListenerGroup : ComponentSystemGroup { }

[UpdateInGroup(typeof(DispatcherGroup))]
public abstract partial class DispatcherBase : SystemBase { }

[UpdateInGroup(typeof(ListenerGroup))]
public abstract partial class ListenerBase : SystemBase { }
