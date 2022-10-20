using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Player))]
public class Fart : MonoBehaviour
{
    [Header("components")]
    [SerializeField]
    private Transform _mouseIndication;
    [SerializeField]
    private ParticleSystem _particleSystem;
    [Space]
    [Header("configuration")]
    [SerializeField]
    private float _cdwToManipulateKnockBack = 0.3f;
    [SerializeField]
    private float _knockBackForce = 500;
    [SerializeField]
    [Tooltip("Reduce percentage from horizontal force")]
    private float _helpForcePercentage = 0.8f;
    [SerializeField]
    private float _fartCdw = 1;

    private Rigidbody2D _rigidbody2D;
    private Player _player;
    private Quaternion _mouseRotation;
    Vector2 _mousePosition;


    private bool _canFart = true;


    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
    }

    void Update()
    {
        ControllMouseIndication();
    }

    private void ControllMouseIndication()
    {
        _mousePosition = Input.mousePosition;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        _mousePosition.x -= objectPos.x;
        _mousePosition.y -= objectPos.y;

        float angle = Mathf.Atan2(_mousePosition.y, _mousePosition.x) * Mathf.Rad2Deg;
        _mouseRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        _mouseIndication.rotation = _mouseRotation;

        _mouseIndication.localScale = new Vector3(transform.localScale.x * -1f, 1, 1);
    }

    public void OnLeftMouseButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DoesFart(-_mousePosition.normalized);
        }
    }

    private void DoesFart(Vector2 direction)
    {
        if (!_canFart) return;

        StartCoroutine(WaitToFartAgain());

        direction *= _knockBackForce;

        StartCoroutine(WaitAnimationTime((_player.transform.localScale.x == 1 && direction.x < 0) || (_player.transform.localScale.x == -1 && direction.x > 0)));

        direction = new Vector2(direction.x * _helpForcePercentage, direction.y);

        _particleSystem.transform.rotation = _mouseRotation;
        _particleSystem.Play();


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

        _player.Animator.PlayAnimationHightPriority(this, PlayerAnimatorModel.Animation.Fart, _cdwToManipulateKnockBack);
        yield return new WaitForSeconds(_cdwToManipulateKnockBack);

        if (flipPlayer)
        {
            _player.transform.localScale = new Vector3(_player.transform.localScale.x == -1 ? 1 : -1, 1, 1);
        }

        _player.CanMove = true;
    }

    IEnumerator WaitToFartAgain()
    {
        _canFart = false;
        yield return new WaitForSeconds(_fartCdw);

        _canFart = true;
    }
}
