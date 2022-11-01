using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class PlayerFartState : PlayerBaseState
{
    public override Player.State State => Player.State.Fart;
    FartAttributeModel _fartModel;

    /// <summary>
    ///  turn false everytime that fart is on Cdw
    /// </summary>
    private bool _canFart = true;

    //not implemented by infinite states
    public override void EnterState() { }

    public override void Start(Player player)
    {
        base.Start(player);
        _player.FartInput.Performed = () => DoesFart();
        _fartModel = player.FartAttributeModel;
    }

    public override void Update()
    {
    }

    private void DoesFart()
    {
        if (!_canFart) return;

        Vector2 mousePosition = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(_player.transform.position);
        mousePosition.x -= objectPos.x;
        mousePosition.y -= objectPos.y;

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        Quaternion mouseRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        _player.StartCoroutine(WaitToFartAgain());

        Vector2 direction = -mousePosition.normalized;
        direction *= _fartModel.KnockBackForce;

        _player.StartCoroutine(WaitAnimationTime((_player.transform.localScale.x == 1 && direction.x < 0) || (_player.transform.localScale.x == -1 && direction.x > 0)));

        direction = new Vector2(direction.x * _fartModel.HelpForcePercentage, direction.y);

        _fartModel.ParticleSystem.transform.rotation = mouseRotation;
        _fartModel.ParticleSystem.Play();

        if (direction.y > 0)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
        }

        _rigidbody2D.AddForce(direction);
    }

    IEnumerator WaitAnimationTime(bool flipPlayer)
    {
        _player.CanMove = false;

        if (flipPlayer)
        {
            _player.transform.localScale = new Vector3(_player.transform.localScale.x == -1 ? 1 : -1, 1, 1);
        }

        _player.Animator.PlayAnimationHightPriority(_player, PlayerAnimatorModel.Animation.Fart, _fartModel.CdwToManipulateKnockBack);
        yield return new WaitForSeconds(_fartModel.CdwToManipulateKnockBack);

        if (flipPlayer)
        {
            _player.transform.localScale = new Vector3(_player.transform.localScale.x == -1 ? 1 : -1, 1, 1);
        }

        _player.CanMove = true;
    }

    IEnumerator WaitToFartAgain()
    {
        _canFart = false;
        _fartModel.ProgressBar.OnStart.Invoke(_fartModel.FartCdw, ProgressBarUI.Behaviour.Hide_After_Completion);
        yield return new WaitForSeconds(_fartModel.FartCdw);

        _canFart = true;
    }
}
