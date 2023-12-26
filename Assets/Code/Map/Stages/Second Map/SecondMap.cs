using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SecondMap : Map
{
    protected override void BeforeStart()
    {
        _finiteStates = new List<MapFiniteStateBase>()
        {
            new Idle(),
            new Combat(),
            new Wait()
        };
    }
}
