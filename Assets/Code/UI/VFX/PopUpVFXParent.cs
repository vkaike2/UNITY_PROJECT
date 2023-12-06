using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PopUpVFXParent : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private HitNumberVFX _hitNumberVFX;
    [SerializeField]
    private IncreaseHealthVFX _increaseHealthVFX;
    [SerializeField]
    private EquipItemVFX _equipItemVfx;
    [Space]
    [SerializeField]
    private ScriptableItemEvents _itemEvents;
    [Space]
    [SerializeField]
    private Transform _parent;
    [SerializeField]
    private RectTransform _firstLocation, _secondLocation;

    [Header("CONFIGURATION")]
    [SerializeField]
    private float _duration = 1;
    [SerializeField]
    private float _onEquipDuration = 2;


    private void Start()
    {
        if (_equipItemVfx != null)
        {
            Debug.Log("test");
            _itemEvents.OnEquipItem.AddListener(OnEquipItemVFX);
        }
    }


    public void AddHealth(float number)
    {
        Vector2 randomPosition = new Vector2(UnityEngine.Random.Range(_firstLocation.position.x, _secondLocation.position.x), _firstLocation.position.y);

        IncreaseHealthVFX vfx = Instantiate(_increaseHealthVFX, randomPosition, Quaternion.identity);
        vfx.transform.parent = _parent;
        vfx.AddHealth(number);

        StartCoroutine(WaitThenDestroy(vfx.gameObject, _duration));
    }

    public void SetHitNumber(float number)
    {
        Vector2 randomPosition = new Vector2(UnityEngine.Random.Range(_firstLocation.position.x, _secondLocation.position.x), _firstLocation.position.y);

        HitNumberVFX vfx = Instantiate(_hitNumberVFX, randomPosition, Quaternion.identity);
        vfx.transform.parent = _parent;
        vfx.SetNumber(number);

        StartCoroutine(WaitThenDestroy(vfx.gameObject, _duration));
    }

    private void OnEquipItemVFX(ScriptableItem scriptableItem)
    {
        Vector2 centralPosition = new Vector2((_firstLocation.position.x + _secondLocation.position.x)/2, _firstLocation.position.y);

        EquipItemVFX vfx = Instantiate(_equipItemVfx, centralPosition, Quaternion.identity);
        vfx.transform.parent = _parent;
        vfx.Equip(scriptableItem.Identity.Description, scriptableItem.Target);

        StartCoroutine(WaitThenDestroy(vfx.gameObject, _onEquipDuration));
    }


    private IEnumerator WaitThenDestroy(GameObject gameObject, float duration)
    {
        yield return new WaitForSeconds(duration);

        Destroy(gameObject);
    }

}
