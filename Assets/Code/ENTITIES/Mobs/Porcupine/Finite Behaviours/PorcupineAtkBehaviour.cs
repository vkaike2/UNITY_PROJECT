using System.Collections;
using UnityEngine;


public class PorcupineAtkBehaviour : PorcupineFiniteBaseBehaviour
{
    public override Porcupine.Behaviour Behaviour => Porcupine.Behaviour.Atk;

    private PorcupineAtkBehaviourModel _atkModel;

    public override void Start(Enemy enemy)
    {
        base.Start(enemy);

        _atkModel = _porcupine.AtkModel;
        _porcupine.AnimatorEvents.OnTriggerAtkProjectile.AddListener(OnTriggerAtkProjectile);
        _porcupine.AnimatorEvents.OnStartAtkJump.AddListener(OnStartAtkJump);
        _porcupine.AnimatorEvents.OnEndAtk.AddListener(OnEndAnimation);
        _atkModel.CanAtk = true;
    }

    public override void OnEnterBehaviour()
    {
        // is dead
        if (!_atkModel.CanAtk)
        {
            return;
        }

        _porcupine.CanMove = false;
        _rigidbody2D.velocity = Vector3.zero;
        _porcupine.Animator.PlayAnimation(PorcupineAnimatorModel.AnimationName.Atk);
    }

    public override void OnExitBehaviour() { }

    public override void Update() { }

    private void OnStartAtkJump()
    {
        _rigidbody2D.velocity = new Vector2(0, _atkModel.JumpForce);
    }

    private void OnTriggerAtkProjectile()
    {
        // facing left
        PorcupineProjectile leftProjectile = GameObject.Instantiate(_atkModel.Projectile, _atkModel.ProjectileSpawnPoint.position, Quaternion.identity, _atkModel.ProjectileContainer);
        leftProjectile.SetInitialInitialValues(new Vector2(_atkModel.ProjectileSpeed * -0.5f, _atkModel.ProjectileSpeed), _atkModel.ProjectileDuration);
        _atkModel.OnRegisterProjectile.Invoke(leftProjectile);

        // facing up
        PorcupineProjectile upProjectile = GameObject.Instantiate(_atkModel.Projectile, _atkModel.ProjectileSpawnPoint.position, Quaternion.identity, _atkModel.ProjectileContainer);
        upProjectile.SetInitialInitialValues(new Vector2(0, _atkModel.ProjectileSpeed), _atkModel.ProjectileDuration);
        _atkModel.OnRegisterProjectile.Invoke(upProjectile);

        // facing right
        PorcupineProjectile rightProjectile = GameObject.Instantiate(_atkModel.Projectile, _atkModel.ProjectileSpawnPoint.position, Quaternion.identity, _atkModel.ProjectileContainer);
        rightProjectile.SetInitialInitialValues(new Vector2(_atkModel.ProjectileSpeed * 0.5f, _atkModel.ProjectileSpeed), _atkModel.ProjectileDuration);
        _atkModel.OnRegisterProjectile.Invoke(rightProjectile);
    }

    private void OnEndAnimation()
    {
        _porcupine.StartCoroutine(CountCdwBetweenAtks());
        _porcupine.StartCoroutine(WaitTouchGroundThenChangeToPatrol());
    }

    private IEnumerator WaitTouchGroundThenChangeToPatrol()
    {
        while (!_atkModel.GroundCheckRaycast.IsRaycastCollidingWithLayer)
        {
            Debug.Log(_atkModel.GroundCheckRaycast.IsRaycastCollidingWithLayer);
            yield return new WaitForFixedUpdate();
        }

        _porcupine.ChangeBehaviour(Porcupine.Behaviour.Patrol);
    }

    private IEnumerator CountCdwBetweenAtks()
    {
        _atkModel.CanAtk = false;
        float cdw = 0;
        while (cdw <= _atkModel.CdwBetweenAtks)
        {
            cdw += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        _atkModel.CanAtk = true;
    }
}
