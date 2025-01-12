using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public partial class TestEventListener : EventListener<TestEvent>
{

    public override bool OnEvent(TestEvent data)
    {
        Debug.Log($"[{this.GetType().Name}] heard TestEvent {data.Value}");
        return true;
    }

    protected override void OnCreate()
    {

    }

    protected override void OnStartRunning()
    {

    }

    protected override void OnUpdate()
    {
        using EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
        // foreach (var (item, entity) in SystemAPI
        //     .Query<SomeComponent>()
        //     //.WithAll<>()
        //     //.WithNone<>()
        //     .WithEntityAccess()) {
        //     //Debug.Log($"[{this.GetType().Name}] {entity}");

        // }

        ecb.Playback(EntityManager);

    }

}