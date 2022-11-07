using System;
using System.Collections;
using UnityEngine;

public class PlayerPoopingState : PlayerFiniteBaseState
{
    public override Player.FiniteState State => Player.FiniteState.Pooping;

    private bool _hasBeingFlipped = false;
    private PoopStateModel _poopModel;
    private GameObject[] _fullTrajectory;
    private float _velocityMultiplyer;
    private bool _canPoop = true;

    public override void EnterState()
    {
        if (!_canPoop)
        {
            _player.PoopInput.Value = false;
            _player.ChangeState(_player.GetPossibleState());
            return;
        }

        _initialGravity = _rigidbody2D.gravityScale;
        _rigidbody2D.gravityScale = _poopModel.GravityWhilePooping;
        StopPlayer();

        Vector2 mouseDirection = _player.GetMouseDirectionRelatedToPlayer();

        if (CheckIfNeedToFlipPlayer(-mouseDirection))
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

        for (int i = 0; i < _poopModel.NumberOfDots; i++)
        {
            _fullTrajectory[i].transform.position = CalculateDotTrajectory(i * 0.1f);
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
        }

        _rigidbody2D.gravityScale = _initialGravity;

        if (_player.MoveInput.Value != Vector2.zero)
        {
            _player.ChangeState(Player.FiniteState.Move);
        }
        else
        {
            _player.ChangeState(Player.FiniteState.Idle);
        }
    }

    private void StopPlayer()
    {
        _player.CanMove = false;
        _rigidbody2D.velocity = Vector2.zero;
    }

    private bool CheckIfNeedToFlipPlayer(Vector2 actionDirection)
    {
        return (_player.transform.localScale.x == 1 && actionDirection.x < 0) || (_player.transform.localScale.x == -1 && actionDirection.x > 0);
    }

    private void FlipPlayer()
    {
        if (!_hasBeingFlipped)
        {
            _player.transform.localScale = new Vector3(_player.transform.localScale.x == -1 ? 1 : -1, 1, 1);
        }
        else
        {
            _player.transform.localScale = new Vector3(_player.transform.localScale.x == -1 ? 1 : -1, 1, 1);
        }
    }

    private void TrhowPoop()
    {
        for (int i = 0; i < _poopModel.NumberOfDots; i++)
        {
            _fullTrajectory[i].SetActive(false);
        }

        Vector2 velocityDirection = CalculateVelocityDirection();

        GameObject projectile = GameObject.Instantiate(_poopModel.Prefab, _poopModel.SpawnPoint.position, Quaternion.identity);
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

        while ( timer <= _poopModel.VelocityTimer)
        {
            timer += Time.deltaTime;
            _velocityMultiplyer = timer / _poopModel.VelocityTimer;
            yield return new WaitForFixedUpdate();
        }
        _velocityMultiplyer = 1; 
    }

    IEnumerator WaitToPoopAgain()
    {
        _canPoop = false;
        _poopModel.CdwProgressBar.OnStart.Invoke(_poopModel.CdwToPoop, ProgressBarUI.Behaviour.Hide_After_Completion);

        yield return new WaitForSeconds(_poopModel.CdwToPoop);

        _canPoop = true;
    }
}
