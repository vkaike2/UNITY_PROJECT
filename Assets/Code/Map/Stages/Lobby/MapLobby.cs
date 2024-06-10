using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLobby : BaseMap
{
    protected override void BeforeStart()
    {
        _finiteStates = new List<MapFiniteStateBase>
        {
            new Wait()
        };
    }
}
