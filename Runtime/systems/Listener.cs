using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public abstract partial class Listener : ListenerBase {

    EntityQuery Query;

    public struct ListenerFlag : IComponentData { }

    public abstract bool OnEvent(EntityManager em, Entity e);

    //Abstract field the deriveer must implement
    public abstract ComponentType EventType { get; }
    public abstract ComponentType HandledFlagType { get; }

    sealed protected override void OnCreate() {
        var e = EntityManager.CreateEntity();
        EntityManager.AddComponentData(e, new EventTypeListenerPair {
            EventType = EventType,
            ListenerType = typeof(ListenerFlag)
        });


        var entityQueryDesc = new EntityQueryDesc {
            All = new ComponentType[] { EventType, typeof(Listeners), typeof(LiveEvent) },
            None = new ComponentType[] { HandledFlagType },
        };

        Query = GetEntityQuery(entityQueryDesc);
    }

    protected override void OnUpdate() {

        using var entities = Query.ToEntityArray(Allocator.TempJob);

        foreach (var entity in entities) {
            if (!OnEvent(EntityManager, entity)) {
                continue;
            } else {
                //Here we add the handled flag to the event so it is no longer processed on subsequent frames
                //This is because the event isn't destroyed right away if some listeners are waiting on something 
                EntityManager.AddComponent(entity, HandledFlagType);

                var buffer = EntityManager.GetBuffer<Listeners>(entity);
                if (buffer.Length == 1) {
                    //We were the last listener, so destroy the event
                    EntityManager.DestroyEntity(entity);
                    continue;
                }

                //Some listeners other than us still remain
                //Remove matching listener
                for (int i = 0; i < buffer.Length; i++) {

                    if (buffer[i].Value != typeof(ListenerFlag))
                        continue;

                    // Debug.Log($"[{this.GetType().Name}] removing listener flag from buffer on {entity}");
                    buffer.RemoveAt(i);
                    break;
                }
            }
        }
    }
}