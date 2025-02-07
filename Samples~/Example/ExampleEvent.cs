using System;
using System.Security.Principal;
using Unity.Entities;
using UnityEngine;

public partial class ExampleEvent : IComponentData {
    //In the future there can be a middle layer Listener that provides the Exampel component 
    //directyl to listeners

    public string Value;
}

