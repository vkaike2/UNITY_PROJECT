using System;
using System.Collections;
using UnityEngine;


public partial class Toilet : MonoBehaviour
{
    private class SpawnPlayer : ToiletStateBase
    {
        public override FiniteState State => FiniteState.SpawnPlayer;

        private const string ANIMATION_PLAYER_OUTSIDE = "Player_Outside";

        private Toilet _toilet;
        private ToiletSpawnPlayerModel _model;
        public override void Start(Toilet toilet)
        {
            _model = toilet.SpawnPlayerModel;
            _toilet = toilet;
        }

        public override void EnterState()
        {
            _toilet.MapManager.FocusCameraOnToiletInstatly();

            _toilet.Animator.Play(Toilet.MyAnimations.Open.ToString());

            _model.OnPlayerLeavingToiletEvent.AddListener(OnPlayerLeavingToilet);
            _model.OnToiletCompletlyOpen.AddListener(OnToiletCompletlyOpen);
            _model.OnToiletCompletlyClosed.AddListener(OnToiletCompletlyClosed);
        }

        public override void OnExitState()
        {
            _model.ResetEvents();
        }


        public override void Update()
        {
        }

        private void OnToiletCompletlyOpen()
        {
            _model.ToiletPlayerAnimator.Play(ANIMATION_PLAYER_OUTSIDE);
        }

        private void OnPlayerLeavingToilet()
        {
            Player player = GameObject.Instantiate(_model.PlayerPrefab, _model.PlayerSpawnPosition);
            player.transform.parent = null;

            _toilet.MapManager.ReturnFocusToPlayer();

            _toilet.Animator.Play(Toilet.MyAnimations.Closing.ToString());
        }

        private void OnToiletCompletlyClosed()
        {
            _toilet.ChangeState(FiniteState.Disabled);
        }
    }

}
