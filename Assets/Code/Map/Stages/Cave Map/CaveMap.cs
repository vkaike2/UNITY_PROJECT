using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CaveMap : Map
{
    protected override void BeforeStart()
    {
        _finiteStates = new List<MapFiniteStateBase>()
        {
            new Idle(),
            new InternalCombat(),
            new Wait()
        };
    }
}