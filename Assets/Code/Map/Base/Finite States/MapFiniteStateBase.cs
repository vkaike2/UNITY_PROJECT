using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapFiniteStateBase
{
    public abstract BaseMap.FiniteState State { get; }

    public abstract void Start(BaseMap map);
    public abstract void OnExitState();
    public abstract void EnterState();
    public abstract void Update();
}
