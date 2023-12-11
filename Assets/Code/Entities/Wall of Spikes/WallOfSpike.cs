using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallOfSpike : MonoBehaviour
{
    [Header("EVENTS")]
    [SerializeField]
    private ScriptableMapEvents _mapEvents;

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

    private Rigidbody2D _rigidBody2d;
    private Coroutine _movingCoroutine;

    private void Awake()
    {
        _rigidBody2d = GetComponent<Rigidbody2D>();

        AddDamageToSpikes();
        DetachTransformPositions();
    }

    private void Start()
    {
        _mapEvents.OnChangeMapEvent.AddListener(OnChangeMap);

        FreezeWall(true);
    }

    private void OnValidate()
    {
        foreach (var position in _positions)
        {
            position.name = position.TransformPosition.gameObject.name;
        }
    }

    #region MAP EVENTS
    private void OnChangeMap(int mapId, int changeId)
    {
        if (mapId == ConstantValues.FIRST_MAP_ID) ChangeEventForFistMap(changeId);
    }

    private void ChangeEventForFistMap(int changeId)
    {
        if (changeId == FirstMapChanges.WALL_OF_SPIKE_MOVE_TO_MEDIUM)
        {
            GoToPosition(_positions[1].Position, () => _mapEvents.OnChangeMapEvent.Invoke(ConstantValues.FIRST_MAP_ID, FirstMapChanges.WALL_OF_SPIKE_READY_MEDIUM));
        }

        if (changeId == FirstMapChanges.WALL_OF_SPIKE_MOVE_TO_LARGE)
        {
            GoToPosition(_positions[2].Position, () => _mapEvents.OnChangeMapEvent.Invoke(ConstantValues.FIRST_MAP_ID, FirstMapChanges.WALL_OF_SPIKE_READY_LARGE));
        }

        // Go back to Initial position
        if (changeId == FirstMapChanges.PREPARE_MAP_TO_BOSS)
        {
            GoToPosition(_positions[0].Position, () => _mapEvents.OnChangeMapEvent.Invoke(ConstantValues.FIRST_MAP_ID, FirstMapChanges.WALL_OF_SPIKE_READY_BOSS));
        }
    }
    #endregion

    private void GoToPosition(Vector2 position, Action callback)
    {
        FreezeWall(false);
        if (_movingCoroutine != null)
        {
            StopCoroutine(_movingCoroutine);
        }

        _movingCoroutine = StartCoroutine(MoveToPosition(
            position,
            () =>
            {
                FreezeWall(true);
                callback();
            }
            ));
    }

    #region INTERNAL METHODS
    private void AddDamageToSpikes()
    {
        List<ImpactDamageStatus> impactDamageFromSpikes = GetComponentsInChildren<ImpactDamageStatus>().ToList();

        foreach (var damage in impactDamageFromSpikes)
        {
            damage.ImpactDamage.Set(_spikeDamage);
        }
    }

    private void DetachTransformPositions()
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

        bool isInitiallyFacingRight = this.transform.localScale.x == 1;

        if (targetPosition.x > myHorizontalPosition.x && !isInitiallyFacingRight)
        {
            this.transform.localScale = new Vector3(-initialLocalScale.x, initialLocalScale.y, initialLocalScale.z);
        }
        else if(targetPosition.x < myHorizontalPosition.x && isInitiallyFacingRight)
        {
            this.transform.localScale = new Vector3(-initialLocalScale.x, initialLocalScale.y, initialLocalScale.z);
        }


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

        _rigidBody2d.velocity = Vector2.zero;
        this.transform.position = new Vector3(targetPosition.x, this.transform.position.y, this.transform.position.z);

        this.transform.localScale = initialLocalScale;

        _animator.Play(ANIMATION_SITTING_DOWN);
        callBack();
    }

    private Vector2 MoveWallTowardsDirection(Vector2 direction)
    {
        Vector2 myHorizontalPosition = GetHorizontalPosition();
        _rigidBody2d.velocity = direction * _movementSpeed;
        return myHorizontalPosition;
    }

    private Vector2 GetHorizontalPosition() => new Vector2(this.transform.position.x, 0);

    private void FreezeWall(bool freeze)
    {
        _rigidBody2d.isKinematic = freeze;
    }
    #endregion

    [Serializable]
    public class PositionInformation
    {
        [HideInInspector]
        public string name;

        [field: SerializeField]
        public Transform TransformPosition { get; private set; }

        public Vector2 Position => TransformPosition.position;
    }
}
