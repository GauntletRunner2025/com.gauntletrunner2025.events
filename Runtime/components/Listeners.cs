using Unity.Collections;
using Unity.Entities;

public partial struct Listeners : IBufferElementData {
    //This should point to the ExampleEvent.ListenerFlag component
    //This acts like a reference to the subscribers of an event
    //When the buffer is empty, there are no remaining listeners 
    //And the event is complete
    public ComponentType Value;
}