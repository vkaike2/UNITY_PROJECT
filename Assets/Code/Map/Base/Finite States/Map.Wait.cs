using System.Collections;
using UnityEngine;


public partial class BaseMap : MonoBehaviour
{
    /// <summary>
    ///  Toilet -> available to go to next stage
    ///  Enemy  -> no enemy
    /// </summary>
    protected class Wait : MapFiniteStateBase
    {
        public override BaseMap.FiniteState State => BaseMap.FiniteState.Wait;

        private Toilet _toilet;

        public override void Start(BaseMap map)
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
