using Unity.Entities;
using UnityEngine;

public partial class ExampleListener : Listener
{
    public struct Handled : IComponentData { }

    public override ComponentType EventType => typeof(ExampleEvent);
    public override ComponentType HandledFlagType => typeof(Handled);

    public override bool OnEvent(EntityManager em, Entity entity)
    {
        var example = SystemAPI.ManagedAPI.GetComponent<ExampleEvent>(entity);
        Debug.LogWarning($"[{this.GetType().Name}] {example.Value}");
        return true;
    }
}
