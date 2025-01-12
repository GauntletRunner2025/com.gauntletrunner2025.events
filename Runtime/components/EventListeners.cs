using Unity.Collections;
using Unity.Entities;
using UnityEngine;

//THis holds the Flag value that is per-listener system
public struct EventListeners : IBufferElementData
{
    public ComponentType Value;
}