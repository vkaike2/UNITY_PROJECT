using System;
using System.Collections;
using UnityEngine;


public partial class Toilet : MonoBehaviour
{
    private class Enabled : ToiletStateBase
    {
        public override FiniteState State => FiniteState.Enabled;

        private const string ANIMATION_PLAYER_INSIDE = "Player_Inside";

        private Toilet _toilet;
        private ToiletEnabledModel _model;

        private bool _playerIsInteractingWith = false;

        public override void Start(Toilet toilet)
        {
            _toilet = toilet;
            _model = _toilet.EnabledModel;
        }

        public override void Update()
        {
            this.OnPlayerOverPlatform();
        }

        public override void EnterState()
        {
            _model.OnInteractWithToiletEvent.AddListener(OnInteractWithToilet);
            _model.OnPlayerEnteringToiletEvent.AddListener(OnPlayerEnteringToilet);
            _model.OnToiletCompletlyClosed.AddListener(OnToiletCompletlyClosed);

            _toilet.PlatformCollider.SetActive(true);
            _toilet.Animator.Play(Toilet.MyAnimations.Open.ToString());
            _toilet.CanInteractWithPlayer = false;
        }

        public override void OnExitState()
        {
            _model.ResetEvents();
        }

        private void OnPlayerOverPlatform()
        {
            if (_playerIsInteractingWith) return;

            if (_toilet.PlayerInRangeCheck.PlayerIsInRange)
            {
                _toilet.CanInteractWithPlayer = true;
                _toilet.Animator.Play(Toilet.MyAnimations.OpenedSelected.ToString());
            }
            else
            {
                _toilet.CanInteractWithPlayer = false;
                _toilet.Animator.Play(Toilet.MyAnimations.Opened.ToString());
            }
        }

        private void OnInteractWithToilet(Player player)
        {
            _playerIsInteractingWith = true;
            player.FreezePlayer(true);

            _toilet.MapManager.FocusCameraOnToilet(() =>
            {
                GameObject.Destroy(player.gameObject);
                _model.ToiletPlayerAnimator.Play(ANIMATION_PLAYER_INSIDE);
            });
        }

        private void OnToiletCompletlyClosed()
        {
            _toilet.MapManager.StartNextMap(_toilet.ParentMap);
        }


        private void OnPlayerEnteringToilet()
        {
            _toilet.Animator.Play(Toilet.MyAnimations.Closing.ToString());
        }

    }
}
