using System.Collections;
using UnityEngine;

public class HitNumberVFXParent : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private HitNumberVFX _hitNumberVFX;
    [Space]
    [SerializeField]
    private Transform _parent;
    [SerializeField]
    private RectTransform _firstLocation, _secondLocation;

    [Header("CONFIGURATION")]
    [SerializeField]
    private float _duration = 1;


    public void SpawnNumber(float number)
    {
        Vector2 randomPosition = new Vector2(Random.Range(_firstLocation.position.x, _secondLocation.position.x), _firstLocation.position.y);

        HitNumberVFX vfx = Instantiate(_hitNumberVFX, randomPosition, Quaternion.identity);
        vfx.transform.parent = _parent;
        vfx.SetNumber(number);

        StartCoroutine(WaitThenDestroy(vfx));
    }

    private IEnumerator WaitThenDestroy(HitNumberVFX vfx)
    {
        yield return new WaitForSeconds(_duration);

        Destroy(vfx.gameObject);
    }

}
