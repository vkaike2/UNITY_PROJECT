using System.Collections;
using UnityEngine;


public class PlayerFartState : PlayerInfiniteBaseState
{
    private FartStateModel _fartModel;
    private PlayerDamageableStateModel _damageableModel;
    /// <summary>
    ///  turn false everytime that fart is on Cdw
    /// </summary>
    private bool _canFart = true;

    public override void Start(Player player)
    {
        base.Start(player);
        _player.FartInput.Performed = () => DoesFart();
        _fartModel = player.FartStateModel;
        _damageableModel = _player.DamageableStateModel;
    }

    public override void Update()
    {
    }

    private void DoesFart()
    {
        if (!_canFart || !_damageableModel.CanAtk) return;

        (Vector2 position, Vector2 direction, Quaternion rotation) mouse = _player.GetMouseInformationRelatedToPlayer();

        Vector2 fartForce  = _fartModel.KnockBackForce * -mouse.direction;

        _player.StartCoroutine(WaitToFartAgain());
        _player.StartCoroutine(WaitAnimationTime(CheckIfNeedToFlipPlayer(-mouse.direction)));

        fartForce = new Vector2(fartForce.x, fartForce.y * _fartModel.HelpForcePercentage);

        _fartModel.ParticleSystem.transform.rotation = mouse.rotation;
        _fartModel.ParticleSystem.Play();

        if (fartForce.y > 0)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
        }

        _rigidbody2D.AddForce(fartForce);
    }

    private bool CheckIfNeedToFlipPlayer(Vector2 actionDirection)
    {
        return (_player.RotationalTransform.localScale.x == 1 && actionDirection.x < 0) || (_player.RotationalTransform.localScale.x == -1 && actionDirection.x > 0);
    }

    IEnumerator WaitAnimationTime(bool flipPlayer)
    {
        _player.CanMove = false;

        if (flipPlayer)
        {
            _player.RotationalTransform.localScale = new Vector3(_player.RotationalTransform.localScale.x == -1 ? 1 : -1, 1, 1);
        }

        _player.Animator.PlayAnimationHightPriority(_player, PlayerAnimatorModel.Animation.Fart, _fartModel.CdwToManipulateKnockBack);
        yield return new WaitForSeconds(_fartModel.CdwToManipulateKnockBack);

        if (flipPlayer)
        {
            _player.RotationalTransform.localScale = new Vector3(_player.RotationalTransform.localScale.x == -1 ? 1 : -1, 1, 1);
        }

        _player.CanMove = true;
    }

    IEnumerator WaitToFartAgain()
    {
        _canFart = false;
        _fartModel.ProgressBar.OnSetBehaviour.Invoke(_fartModel.FartCdw, ProgressBarUI.Behaviour.ProgressBar_Hide);
        yield return new WaitForSeconds(_fartModel.FartCdw);

        _canFart = true;
    }

}
