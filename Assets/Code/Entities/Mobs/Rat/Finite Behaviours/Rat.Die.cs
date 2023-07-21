using System.Collections;
using UnityEngine;


public partial class Rat : Enemy
{
    private class Die : RatBaseBehaviour
    {
        public override Behaviour Behaviour => Rat.Behaviour.Die;

        public override void OnEnterBehaviour()
        {
        }

        public override void OnExitBehaviour()
        {
        }

        public override void Update()
        {
        }
    }
}
