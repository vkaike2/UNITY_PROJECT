using System;
using Unity.VisualScripting;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private Animator _animator;

    [Header("CONFIGURATION")]
    [SerializeField]
    private SpawnSpeed _spawnSpeed;

    public ItemData ItemData { get; set; }

    private Rigidbody2D _rigidBody2D;

    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void DropItem()
    {
        _rigidBody2D.AddForce(
            new Vector2(_spawnSpeed.Horizontal.GetRandom(), _spawnSpeed.Vertical.GetRandom()),
            ForceMode2D.Impulse);

        _rigidBody2D.AddTorque(100);
    }

    public void ChangeAnimationOnItemOver(bool isMouseOver)
    {
        if (isMouseOver)
        {
            _animator.Play(MyAnimations.Selected.ToString());
        }
        else
        {
            _animator.Play(MyAnimations.Idle.ToString());
        }
    }

    private enum MyAnimations
    {
        Idle,
        Selected
    }

    [Serializable]
    private class SpawnSpeed
    {
        [SerializeField]
        private MinMax _vertical;
        [SerializeField]
        private MinMax _horizontal;

        public MinMax Vertical => _vertical;
        public MinMax Horizontal => _horizontal;
    }

    [Serializable]
    private class MinMax
    {
        [SerializeField]
        private float _min;
        [SerializeField]
        private float _max;

        public float Min => _min;
        public float Max => _max;

        public float GetRandom() => UnityEngine.Random.Range(_min, _max);
    }
}
