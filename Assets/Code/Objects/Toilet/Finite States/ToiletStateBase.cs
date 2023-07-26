using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToiletStateBase 
{
    public abstract Toilet.FiniteState State { get; }

    public abstract void Start(Toilet toilet);
    public abstract void OnExitState();
    public abstract void EnterState();
    public abstract void Update();
}
