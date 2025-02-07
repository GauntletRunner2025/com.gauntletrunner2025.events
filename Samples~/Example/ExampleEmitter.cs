using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public partial class ExampleEmitter : SystemBase {
    public int count = 10;

    protected override void OnUpdate() {

        //Wait 10 frames before emitting an ExampleEvent

        if (count-- > 0) {
            return;
        } else {
            Enabled = false;
        }

        using EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

        Debug.Log($"[{this.GetType().Name}] Emitting ExampleEvent");

        var e = ecb.CreateEntity();
        ecb.AddComponent(e, new Request() { Value = new ComponentType(typeof(ExampleEvent)) });
        ecb.AddComponent(e, new ExampleEvent { Value = "Hello, World!" });

        ecb.Playback(EntityManager);

    }
}