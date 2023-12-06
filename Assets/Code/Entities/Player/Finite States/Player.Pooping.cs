using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class Player : MonoBehaviour
{
    private class Pooping : PlayerBaseState
    {
        public override FiniteState State => FiniteState.Pooping;

        private GameObject[] _fullTrajectory;
        private Transform _trajectoryParent;

        private float _gravityWhenStarted;
        private Vector2 _previousVelocity;

        private bool _hasBeingFlipped;
        private float _velocityMultiplyer;

        public override void Start(Player player)
        {
            base.Start(player);

            _poopModel.CanPoop = true;

            _trajectoryParent = FindObjectOfType<PoopProjectileParent>().transform;

            InstantiateFullTrajectory();
        }

        public override void EnterState()
        {
            AssignEvents();

            _gravityWhenStarted = _rigidBody2D.gravityScale;
            _previousVelocity = _rigidBody2D.velocity;
            _rigidBody2D.velocity = Vector2.zero;

            Vector2 mouseDirection = GetMouseDirectionRelatedToPlayer();

            if (CheckIfNeedToFlipPlayer(mouseDirection))
            {
                FlipPlayer();
                _hasBeingFlipped = true;
            }

            StartPooping();
        }

        public override void OnExitState()
        {
            UnassignEvents();
        }

        public override void Update()
        {
            ControlPlayerWhilePooping();
        }

        private void AssignEvents()
        {
            _player.PoopInput.Canceled.AddListener(OnPoopInputCanceled);
        }

        private void UnassignEvents()
        {
            _player.PoopInput.Canceled.RemoveListener(OnPoopInputCanceled);
        }

        private void OnPoopInputCanceled()
        {
            ThrowPoop();

            if (_hasBeingFlipped)
            {
                FlipPlayer();
                _hasBeingFlipped = false;
            }

            _rigidBody2D.gravityScale = _gravityWhenStarted;
            _rigidBody2D.velocity = _previousVelocity;

            ChangeBackToPreviousState();
        }

        private void StartPooping()
        {
            _player.PlayerAnimator.PlayAnimation(PlayerAnimatorModel.Animation.Pooping);

            _player.StartCoroutine(CalculatePoopVelocity());
        }

        private void ControlPlayerWhilePooping()
        {
            _rigidBody2D.gravityScale = _poopModel.GravityWhilePooping;

            if (_previousVelocity.y > 0)
            {
                _rigidBody2D.velocity = Vector2.zero;
            }

            for (int i = 0; i < _poopModel.NumberOfDots; i++)
            {
                _fullTrajectory[i].SetActive(false);
            }

            for (int i = 0; i < _poopModel.NumberOfDots; i++)
            {
                _fullTrajectory[i].transform.position = CalculateDotTrajectory(i * 0.1f);
                RaycastHit2D[] hit = Physics2D.RaycastAll(_fullTrajectory[i].transform.position, -Vector2.up, 0.1f, _jumpModel.GroundLayer);

                if (hit.Any())
                {
                    break;
                }

                _fullTrajectory[i].SetActive(true);
            }
        }

        private void ThrowPoop()
        {
            PlayPoopSoundEffect();

            for (int i = 0; i < _poopModel.NumberOfDots; i++)
            {
                _fullTrajectory[i].SetActive(false);
            }

            Vector2 velocityDirection = CalculateVelocityDirection();

            PoopProjectile projectile = Instantiate(_status.Poop.Projectile.Get(), _poopModel.SpawnPoint.position, Quaternion.identity);

            projectile.SetInitialValues(velocityDirection, _player.Status, _player.DamageDealer);

            _player.StartCoroutine(CalculateCooldown());
        }

        #region AUXILIAR METHODS
        private Vector2 GetMouseDirectionRelatedToPlayer()
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector3 objectPos = Camera.main.WorldToScreenPoint(_player.transform.position);
            mousePosition.x -= objectPos.x;
            mousePosition.y -= objectPos.y;
            return mousePosition.normalized;
        }

        private bool CheckIfNeedToFlipPlayer(Vector2 mouseDirection)
        {
            bool isFacingRight = _player.RotationalTransform.localScale.x == 1;
            return (!isFacingRight && mouseDirection.x < 0) || (isFacingRight && mouseDirection.x > 0);
        }

        private void FlipPlayer()
        {
            _player.RotationalTransform.localScale = new Vector3(-_player.RotationalTransform.localScale.x, 1, 1);
        }

        private void InstantiateFullTrajectory()
        {
            _fullTrajectory = new GameObject[_poopModel.NumberOfDots];

            for (int i = 0; i < _poopModel.NumberOfDots; i++)
            {
                _fullTrajectory[i] = GameObject.Instantiate(_poopModel.TrajectoryPrefab, _poopModel.SpawnPoint.position, Quaternion.identity);
                _fullTrajectory[i].transform.parent = _trajectoryParent;
                _fullTrajectory[i].SetActive(false);
            }
        }

        private Vector2 CalculateDotTrajectory(float time)
        {
            return (Vector2)_poopModel.SpawnPoint.position + (CalculateVelocityDirection() * time) + (time * time) * 0.5f * Physics2D.gravity;
        }

        private Vector2 CalculateVelocityDirection()
        {
            Vector2 mouseDirection = GetMouseDirectionRelatedToPlayer();
            float currentVelocity = _status.Poop.MaximumVelocity.Get() * _velocityMultiplyer;
            return currentVelocity * mouseDirection;
        }

        private void PlayPoopSoundEffect()
        {
            if (_player.AudioController == null) return;
            _player.AudioController.PlayClip(AudioController.ClipName.Player_Poop);
        }

        private void ChangeBackToPreviousState()
        {
            if (IsPlayerTouchingGround)
            {
                if (_player.MoveInput.Value != Vector2.zero)
                {
                    ChangeState(FiniteState.Move);
                }
                else
                {
                    ChangeState(FiniteState.Idle);
                }
            }
            else
            {
                ChangeState(FiniteState.Falling);
            }
        }

        private IEnumerator CalculatePoopVelocity()
        {
            float timer = 0;
            _velocityMultiplyer = 0;

            while (timer <= _status.Poop.VelocityTimer.Get())
            {
                timer += Time.deltaTime;
                _velocityMultiplyer = timer / _status.Poop.VelocityTimer.Get();
                yield return new WaitForFixedUpdate();
            }
            _velocityMultiplyer = 1;
        }

        private IEnumerator CalculateCooldown()
        {
            _poopModel.CanPoop = false;
            _poopModel.CdwProgressBar.OnSetBehaviour.Invoke(_status.Poop.CdwToPoop.Get(), ProgressBarUI.Behaviour.ProgressBar_Hide);

            float cdw = 0;
            while (cdw <= _status.Poop.CdwToPoop.Get())
            {
                cdw += Time.deltaTime;

                UIEventManager.instance.OnPlayerPoopProgressBar.Invoke(cdw / _status.Poop.CdwToPoop.Get());
                yield return new WaitForFixedUpdate();
            }

            _poopModel.CanPoop = true;
        }

        #endregion
    }
}
