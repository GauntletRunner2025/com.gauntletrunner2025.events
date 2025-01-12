using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public partial class Manager : SystemBase
{
    protected override void OnCreate()
    {
        Application.targetFrameRate = 10;
    }

    protected override void OnStartRunning()
    {

    }

    protected override void OnUpdate()
    {
        using EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
        ecb.Playback(EntityManager);

    }
}