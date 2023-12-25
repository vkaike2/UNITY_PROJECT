using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SecondMap : Map
{
    private class InternalCombat : MapFiniteStateBase
    {
        public override FiniteState State => FiniteState.Combat;

        public override void EnterState()
        {
        }

        public override void OnExitState()
        {
        }

        public override void Start(Map map)
        {
        }

        public override void Update()
        {
        }
    }
}
