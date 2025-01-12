using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public partial class TestEventTrigger : SystemBase
{
    protected override void OnCreate()
    {

    }

    int Countdown = 10;

    protected override void OnStartRunning()
    {

    }

    protected override void OnUpdate()
    {
        if (Countdown > 0)
        {
            Countdown--;
            return;
        }

        Debug.Log($"Test Event Trigger");
        var entity = EntityManager.CreateEntity();
        EntityManager.AddComponentData(entity, new EventDispatcher<TestEvent>.Request
        {
            Data = new TestEvent
            {
                Value = new NativeText("Hello World", Allocator.Temp)
            }
        });

        this.Enabled = false;
    }
}