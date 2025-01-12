using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[assembly: RegisterGenericComponentType(typeof(TestEvent))]

public partial class TestEventDispatcher : EventDispatcher<TestEvent>
{

}