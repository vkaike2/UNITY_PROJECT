using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WallOfSpike : MonoBehaviour
{
    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _spikeDamage;
    [SerializeField]
    private float _movementSpeed = 1;

    [Header("COMPONENTS")]
    [SerializeField]
    private Transform _objectParent;
    [SerializeField]
    private List<PositionInformation> _positions;
    [SerializeField]
    private Animator _animator;

    private const string ANIMATION_IDLE_WITH_NO_LEGS = "Wall of Spikes Idle no Legs";
    private const string ANIMATION_CREATING_LEGS = "Wall of Spikes Creating Legs";
    private const string ANIMATION_RUNNING = "Wall of Spikes Running";
    private const string ANIMATION_SITTING_DOWN = "Wall of Spikes Sitting Down";
    private bool _hasLegs = false;

    public OnPositionReadyEvent OnPositionReady { get; private set; } = new OnPositionReadyEvent();

    private Rigidbody2D _rigidbody2D;
    private Coroutine _movingCoroutine;

    private void OnValidate()
    {
        if (_positions == null || _positions.Count == 0)
        {
            _positions = new List<PositionInformation>()
            {
                new PositionInformation(PositionType.First),
                new PositionInformation(PositionType.Second)
            };
            return;
        }
    }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        AddDamageToSpikes();
        DetatchTransformPositions();
    }

    public void GoToPosition(PositionType position)
    {
        if (_movingCoroutine != null)
        {
            StopCoroutine(_movingCoroutine);
        }

        _movingCoroutine = StartCoroutine(MoveToPosition(
            _positions.FirstOrDefault(e => e.PositionType == position).Position,
            () => { OnPositionReady.Invoke(position); }
            ));
    }

    private void AddDamageToSpikes()
    {
        List<ImpactDamageStatus> impactDamageFromSpikes = GetComponentsInChildren<ImpactDamageStatus>().ToList();

        foreach (var damage in impactDamageFromSpikes)
        {
            damage.ImpactDamage.Set(_spikeDamage);
        }
    }

    private void DetatchTransformPositions()
    {
        foreach (var position in _positions)
        {
            position.TransformPosition.SetParent(_objectParent);
        }

    }

    private IEnumerator MoveToPosition(Vector2 targetPosition, Action callBack)
    {
        if (!_hasLegs)
        {
            _animator.Play(ANIMATION_CREATING_LEGS);
            yield return new WaitForSeconds(1f);
            _hasLegs = true;
        }


        targetPosition = new Vector2(targetPosition.x, 0);
        Vector2 myHorizontalPosition = GetHorizontalPosition();
        Vector2 direction = (targetPosition - myHorizontalPosition).normalized;

        Vector3 initialLocalScale = this.transform.localScale;

        _animator.Play(ANIMATION_RUNNING);

        this.transform.localScale = new Vector3(-initialLocalScale.x, initialLocalScale.y, initialLocalScale.z);

        // going to the right
        if (direction.x > 0)
        {
            while (myHorizontalPosition.x < targetPosition.x)
            {
                myHorizontalPosition = MoveWallTowardsDirection(direction);
                yield return new WaitForFixedUpdate();
            }
        }
        // going to the left
        else
        {
            while (myHorizontalPosition.x > targetPosition.x)
            {
                myHorizontalPosition = MoveWallTowardsDirection(direction);
                yield return new WaitForFixedUpdate();
            }
        }

        _rigidbody2D.velocity = Vector2.zero;
        this.transform.position = new Vector3(targetPosition.x, this.transform.position.y, this.transform.position.z);

        this.transform.localScale = initialLocalScale;

        _animator.Play(ANIMATION_SITTING_DOWN);
        callBack();
    }

    private Vector2 MoveWallTowardsDirection(Vector2 direction)
    {
        Vector2 myHorizontalPosition = GetHorizontalPosition();
        _rigidbody2D.velocity = direction * _movementSpeed;
        return myHorizontalPosition;
    }

    private Vector2 GetHorizontalPosition() => new Vector2(this.transform.position.x, 0);

    public enum PositionType
    {
        First,
        Second
    }

    [Serializable]
    public class PositionInformation
    {
        [HideInInspector]
        public string name;

        [field: SerializeField]
        public PositionType PositionType { get; private set; }
        [field: SerializeField]
        public Transform TransformPosition { get; private set; }

        public PositionInformation(PositionType type)
        {
            PositionType = type;
            name = $"{type} Position";
        }

        public Vector2 Position => TransformPosition.position;
    }

    public class OnPositionReadyEvent : UnityEvent<PositionType> { }
}
