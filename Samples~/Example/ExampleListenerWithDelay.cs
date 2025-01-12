using Unity.Entities;
using UnityEngine;

public partial class ExampleListenerWithDelay : Listener
{
    public struct Handled : IComponentData { }

    public override ComponentType EventType => typeof(ExampleEvent);
    public override ComponentType HandledFlagType => typeof(Handled);

    public override bool OnEvent(EntityManager em, Entity e)
    {
        var example = SystemAPI.ManagedAPI.GetComponent<ExampleEvent>(e);

        //Notice we want the event to have a certain component on it
        //It doesn't have it at first so we put a request out for it
        //and we return false. 

        //Eventually some system will hopefullly supply the required component
        //At which point we can process the event and return true instead
        //Until then we will keep returning false

        //We could also create a new entity that points to the event entity
        //thereby providing a different avenue of how to point to the event

        //eg: put it on this (the event) entity!
        //or: put it on the entity that this request points to!

        if (!SystemAPI.HasComponent<DelayedComponent>(e))
        {
            em.AddComponentData(e, new DelayedComponentRequest());
            return false;
        }
        else
        {
            Debug.LogWarning($"[{this.GetType().Name}] {example.Value}");
            return true;
        }
    }
}