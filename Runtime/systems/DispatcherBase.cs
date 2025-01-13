using Unity.Collections;
using Unity.Entities;
using UnityEngine;

partial class Dispatcher : DispatcherBase {
    protected override void OnUpdate() {

        using var ecb = new EntityCommandBuffer(Allocator.TempJob);
        foreach (var (item, eventEntity) in SystemAPI
            .Query<Request>()
            .WithNone<LiveEvent>()
            .WithEntityAccess()) {

            ecb.RemoveComponent<Request>(eventEntity);
            ecb.AddComponent(eventEntity, new LiveEvent());

            //Populate the EventTypeListenerPairs
            ecb.AddBuffer<Listeners>(eventEntity);

            foreach (var (pair, entity2) in SystemAPI
               .Query<EventTypeListenerPair>()
               .WithEntityAccess()) {

                if (pair.EventType != item.Value)
                    continue;

                //Add the listener to the buffer
                // Debug.Log($"[{this.GetType().Name}] adding listener flag to buffer on {eventEntity}");
                ecb.AppendToBuffer(eventEntity, new Listeners { Value = pair.ListenerType });
            }

            // Debug.Log($"[{this.GetType().Name}] dispatching event on {eventEntity}");

        }
        ecb.Playback(EntityManager);
    }
}