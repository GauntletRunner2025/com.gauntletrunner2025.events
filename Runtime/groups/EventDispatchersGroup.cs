using Unity.Collections;
using Unity.Entities;
using UnityEngine;


[UpdateAfter(typeof(EventListenersGroup))]
public partial class EventDispatchersGroup : ComponentSystemGroup
{
}