using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapFiniteStateBase
{
    public abstract Map.FiniteState State { get; }

    public abstract void Start(Map map);
    public abstract void OnExitState();
    public abstract void EnterState();
    public abstract void Update();
}
