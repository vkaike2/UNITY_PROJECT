using System.Collections;
using UnityEngine;


public partial class Map : MonoBehaviour
{
    /// <summary>
    ///  Toilet -> available to go to next stage
    ///  Enemy  -> no enemy
    /// </summary>
    private class Wait : MapFiniteStateBase
    {
        public override Map.FiniteState State => Map.FiniteState.Wait;

        private Toilet _toilet;

        public override void Start(Map map)
        {
            _toilet = map.Toilet;
        }

        public override void EnterState()
        {
            _toilet.SetState(Toilet.FiniteState.Enabled);
        }

        public override void OnExitState()
        {
        }

        public override void Update()
        {
        }
    }
}
