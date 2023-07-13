using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPoopingState : PlayerFiniteBaseState
{
    public override Player.FiniteState State => Player.FiniteState.Pooping;

    private PlayerPoopStateModel _poopModel;
    private GameObject[] _fullTrajectory;

    private bool _hasBeingFlipped = false;
    private float _velocityMultiplyer;
    private Vector2 _previousVelocity;

    public override void EnterState()
    {
        _initialGravity = _rigidbody2D.gravityScale;
        _previousVelocity = _rigidbody2D.velocity;

        StopPlayer();

        Vector2 mouseDirection = _player.GetMouseDirectionRelatedToPlayer();

        if (CheckIfNeedToFlipPlayer(mouseDirection))
        {
            FlipPlayer();
            _hasBeingFlipped = true;
        }

        _player.Animator.PlayAnimation(PlayerAnimatorModel.Animation.Pooping);

        _player.PoopInput.Performed = () => OnPoopInputPerformed();
        _player.PoopInput.Canceled = () => OnPoopInputCanceled();
    }

    public override void Start(Player player)
    {
        base.Start(player);
        _poopModel = _player.PoopStateModel;

        _poopModel.CanPoop = true;

        _fullTrajectory = new GameObject[_poopModel.NumberOfDots];

        for (int i = 0; i < _poopModel.NumberOfDots; i++)
        {
            _fullTrajectory[i] = GameObject.Instantiate(_poopModel.TrajectoryPrefab, _poopModel.SpawnPoint.position, Quaternion.identity);
            _fullTrajectory[i].transform.parent = _poopModel.TrajectoryParent;
            _fullTrajectory[i].SetActive(false);
        }
    }

    public override void Update()
    {
        if (!_player.PoopInput.Value) return;

        _rigidbody2D.gravityScale = _poopModel.GravityWhilePooping;
        if (_previousVelocity.y > 0)
        {
            _rigidbody2D.velocity = Vector2.zero;
        }

        for (int i = 0; i < _poopModel.NumberOfDots; i++)
        {
            _fullTrajectory[i].SetActive(false);
        }

        for (int i = 0; i < _poopModel.NumberOfDots; i++)
        {
            _fullTrajectory[i].transform.position = CalculateDotTrajectory(i * 0.1f);
            RaycastHit2D[] hit = Physics2D.RaycastAll(_fullTrajectory[i].transform.position, -Vector2.up, 0.1f, _player.JumpStateModel.GroundLayer);

            if (hit.Any())
            {
                break;
            }

            _fullTrajectory[i].SetActive(true);
        }
    }

    private void OnPoopInputPerformed()
    {
        _player.StartCoroutine(CalculatePoopVelocity());
    }

    private void OnPoopInputCanceled()
    {
        TrhowPoop();

        _player.CanMove = true;

        if (_hasBeingFlipped)
        {
            FlipPlayer();
            _hasBeingFlipped = false;
        }

        _rigidbody2D.gravityScale = _initialGravity;
        _rigidbody2D.velocity = _previousVelocity;

        if (_player.MoveInput.Value != Vector2.zero)
        {
            _player.ChangeState(Player.FiniteState.Move);
        }
        else
        {
            _player.ChangeState(Player.FiniteState.Idle);
        }
    }

    public override void OnExitState()
    {
        _player.CanMove = true;

        for (int i = 0; i < _poopModel.NumberOfDots; i++)
        {
            _fullTrajectory[i].SetActive(false);
        }
    }

    private void StopPlayer()
    {
        _player.CanMove = false;
        _rigidbody2D.velocity = Vector2.zero;
    }
    
    private bool CheckIfNeedToFlipPlayer(Vector2 mouseDirection)
    {
        bool isFacingRight = _player.RotationalTransform.localScale.x == 1;

        return (!isFacingRight && mouseDirection.x < 0) 
            || (isFacingRight && mouseDirection.x > 0);
    }

    private void FlipPlayer()
    {
        _player.RotationalTransform.localScale = new Vector3(-_player.RotationalTransform.localScale.x, 1, 1);
    }

    private void TrhowPoop()
    {
        PlayPoopSoundEfect();

        for (int i = 0; i < _poopModel.NumberOfDots; i++)
        {
            _fullTrajectory[i].SetActive(false);
        }

        Vector2 velocityDirection = CalculateVelocityDirection();

        GameObject projectile = GameObject.Instantiate(_poopModel.Prefab, _poopModel.SpawnPoint.position, Quaternion.identity);

        if (_player.OnPoopEvent != null)
        {
            _player.OnPoopEvent(projectile);
        }

        Rigidbody2D rigidbody2D = projectile.GetComponent<Rigidbody2D>();

        rigidbody2D.velocity = velocityDirection;

        _player.StartCoroutine(WaitToPoopAgain());
    }

    private Vector2 CalculateVelocityDirection()
    {
        Vector2 mouseDirection = _player.GetMouseDirectionRelatedToPlayer();
        float currentVelocity = _poopModel.MaximumVelocity * _velocityMultiplyer;
        return currentVelocity * mouseDirection;
    }

    private Vector2 CalculateDotTrajectory(float time)
    {
        return (Vector2)_poopModel.SpawnPoint.position + (CalculateVelocityDirection() * time) + (time * time) * 0.5f * Physics2D.gravity;
    }

    IEnumerator CalculatePoopVelocity()
    {
        float timer = 0;
        _velocityMultiplyer = 0;

        while (timer <= _poopModel.VelocityTimer)
        {
            timer += Time.deltaTime;
            _velocityMultiplyer = timer / _poopModel.VelocityTimer;
            yield return new WaitForFixedUpdate();
        }
        _velocityMultiplyer = 1;
    }

    IEnumerator WaitToPoopAgain()
    {
        _poopModel.CanPoop = false;
        _poopModel.CdwProgressBar.OnSetBehaviour.Invoke(_poopModel.CdwToPoop, ProgressBarUI.Behaviour.ProgressBar_Hide);

        yield return new WaitForSeconds(_poopModel.CdwToPoop);

        _poopModel.CanPoop = true;
    }

    private void PlayPoopSoundEfect()
    {
        if (_player.AudioController == null) return;
        _player.AudioController.PlayClip(AudioController.ClipName.Player_Poop);
    }
}
