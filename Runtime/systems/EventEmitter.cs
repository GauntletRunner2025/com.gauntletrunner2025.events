using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(EventDispatchersGroup))]
public partial class EventDispatcher<T> : SystemBase where T : unmanaged, IComponentData
{
    //A requester (anyone) can submit a request for its version of the event to be dispatched
    public struct Request : IComponentData
    {
        public T Data;
    }

    protected override void OnUpdate()
    {
        using EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
        foreach (var (item, entity) in SystemAPI
            .Query<Request>()
            .WithEntityAccess())
        {
            Debug.Log($"[{this.GetType().Name}] heard a request to emit its event {item.Data}");

            //Promote the request to an actual event
            ecb.AddComponent(entity, item.Data);
            ecb.RemoveComponent<Request>(entity);
        }

        ecb.Playback(EntityManager);
    }
}