using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MapTrigger : MonoBehaviour
{
    [Header("TRIGGER")]
    [SerializeField]
    private int _playerLayer;

    [Header("EVENT")]
    [SerializeField]
    private ScriptableMapEvents _mapEvent;
    [Space(2)]
    [SerializeField]
    private int _mapId;
    [SerializeField]
    private int _eventId;

    [Header("CONFIGURATION")]
    [SerializeField]
    private CallbackAfterActivation _callback;

    private bool _isActive = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isActive) return;

        if (collision == null) return;
        if (collision.gameObject == null) return;

        if (collision.gameObject.layer != _playerLayer) return;

        _mapEvent.OnChangeMapEvent.Invoke(_mapId, _eventId);
        
        ProcessCallback();
    }

    private void ProcessCallback()
    {
        switch (_callback)
        {
            case CallbackAfterActivation.Invalidate:
                _isActive = false;
                break;
        }
    }


    private enum CallbackAfterActivation
    {
        Invalidate
    }
}
