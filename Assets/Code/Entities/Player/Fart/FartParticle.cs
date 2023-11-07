using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class FartParticle : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [Header("CONFIGURATION")]
    [SerializeField]
    private MinMax _vibrationDistance;
    [SerializeField]
    private float _velocity;

    private Vector2 _offset;
    private Vector2 _direction;

    private float _currentVibrationDistance;

    private void Awake()
    {
        _currentVibrationDistance = _vibrationDistance.Min;
    }

    private void Start()
    {
        SetRandomAlphaValue();

        _offset = this.transform.localPosition;
    }

    private void FixedUpdate()
    {
        if (_direction == Vector2.zero)
        {
            _direction = GetRandomDirection();
        }

        Vector3 newPosition = transform.localPosition + new Vector3(_direction.x, _direction.y, 0f) * _velocity * Time.fixedDeltaTime;
        this.transform.localPosition = newPosition;

        float offsetDistance = GetOffsetDistance();

        if (offsetDistance > _currentVibrationDistance)
        {
            _direction = -_direction;
        }
    }

    public void SetInitialValues(float duration)
    {
        StartCoroutine(CalculateVibrationDistance(duration));
    }

    private void SetRandomAlphaValue()
    {
        Color spriteColor = _spriteRenderer.color;
        spriteColor.a = 0.7f;
        _spriteRenderer.color = spriteColor;
    }

    private Vector2 GetRandomDirection()
    {
        Vector2 direction = new Vector2(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        return direction;
    }

    private float GetOffsetDistance() => Mathf.Abs(Vector2.Distance(_offset, this.transform.localPosition));

    private IEnumerator CalculateVibrationDistance(float duration)
    {
        float cdw = 0;
        while (cdw <= duration)
        {
            cdw += Time.deltaTime;

            float percentage = cdw / duration;
            _currentVibrationDistance = (_vibrationDistance.Range * percentage) + _vibrationDistance.Min;

            yield return new WaitForFixedUpdate();
        }

    }
}
