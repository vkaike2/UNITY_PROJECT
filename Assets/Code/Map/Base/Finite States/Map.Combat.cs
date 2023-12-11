using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Map : MonoBehaviour
{
    /// <summary>
    ///  Toilet -> disable
    ///  Enemy  -> will spawn periodically from time to time
    /// </summary>
    protected class Combat : MapFiniteStateBase
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
