using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public partial class DelayedComponentRequestSystem : SystemBase {

    //This system will not run until it is enabled
    //THerefore one of the listeners will be waiting and waiting until this sytem updates
    //ExampleListenerTwo requires a DelayedComponent to be added to the entity
    //That is what this system does, for demonstrating event persistence
    protected override void OnCreate() {
        this.Enabled = false;
    }

    protected override void OnUpdate() {
        using EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
        foreach (var (item, entity) in SystemAPI
            .Query<DelayedComponentRequest>()
            //.WithAll<>()
            .WithNone<DelayedComponent>()
            .WithEntityAccess()) {
            //Debug.Log($"[{this.GetType().Name}] {entity}");

            Debug.Log($"[{this.GetType().Name}] adding DelayedComponent to {entity}");
            ecb.AddComponent(entity, new DelayedComponent());

        }

        ecb.Playback(EntityManager);

    }
}