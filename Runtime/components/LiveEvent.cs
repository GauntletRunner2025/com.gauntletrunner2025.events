using Unity.Collections;
using Unity.Entities;

//This marks that an event is not merely a request but it is fully ready to be processed
public partial struct LiveEvent : IComponentData {
}