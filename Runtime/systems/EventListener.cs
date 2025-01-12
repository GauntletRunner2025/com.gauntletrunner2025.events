using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(EventListenersGroup))]
public abstract partial class EventListener : SystemBase where T : unmanaged, IComponentData
{
    EntityQuery EventsQuery;
    EntityQuery RequestsQuery;

    public struct Handled : IComponentData { }
    public struct ListenerFlag : IComponentData { }

    public abstract bool OnEvent(T data);

    protected sealed override void OnCreate()
    {
        //Listen for requsts for the event we care about
        RequestsQuery = new EntityQueryBuilder(Allocator.TempJob)
            .WithAll<EventDispatcher<T>.Request>()
            .WithAll<EventListeners>()
            .Build(EntityManager);

        //Listen for the actual events
        EventsQuery = new EntityQueryBuilder(Allocator.TempJob)
            .WithAll<T>()
            .WithAll<EventListeners>()
            .WithNone<Handled>()
            .Build(EntityManager);
    }

    void ListenForEventRequests()
    {
        using var entities = RequestsQuery.ToEntityArray(Allocator.TempJob);
        foreach (var entity in entities)
        {
            Debug.Log($"[{this.GetType().Name}] heard a request for the event type its interested in");
            var buffer = EntityManager.GetBuffer<EventListeners>(entity);
            buffer.Add(new EventListeners { Value = ComponentType.ReadWrite<ListenerFlag>() });
        }
    }
    void ListenForActualEvents()
    {

        using var entities = EventsQuery.ToEntityArray(Allocator.TempJob);
        foreach (var entity in entities)
        {
            T data = EntityManager.GetComponentData<T>(entity);
            var buffer = EntityManager.GetBuffer<EventListeners>(entity);

            //OnEvent will return true if the event was received successfully
            //if it returns false, the event will persist until the next frame
            //This could be because the event is missing a required component
            if (!OnEvent(data))
                continue;

            Debug.Log($"[{this.GetType().Name}] consumed the event");

            //Mark the event as handled so we personally dont try and consume it again
            EntityManager.AddComponentData(entity, new Handled());

            //Go through the buffer and remove the listener flag that corresponds to this system
            //This is different from the Handled component.
            //THe listner buffer is how the Dispatcher knows all listeners have been satisfied, because they removed their entry in the buffer
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i].Value != ComponentType.ReadWrite<T>())
                    continue;

                //If this listener flag is ours then remove it
                buffer.RemoveAt(i);
                break;
            }

            if (buffer.Length == 0)
            {
                //The event is completely consumer 
                //Destroy the event entity
                EntityManager.DestroyEntity(entity);
            }
        }
    }
    protected override void OnUpdate()
    {
        ListenForEventRequests();

        ListenForActualEvents();
    }
}