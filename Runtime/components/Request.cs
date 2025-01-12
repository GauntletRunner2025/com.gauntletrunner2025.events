using Unity.Entities;

//This marks an event as a request to be processed
public struct Request : IComponentData {
    public ComponentType Value;
}