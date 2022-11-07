using System;
using System.Collections;
using UnityEngine;


public class OneWayPlatform : MonoBehaviour
{
    [Space]
    [Header("configuration")]
    [SerializeField]
    private float _deactivationDuration = 0.5f;
    
    private BoxCollider2D _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }


    public void PlayerDownPlatform()
    {
        StartCoroutine(MakeItTrigger());
    }


    IEnumerator MakeItTrigger()
    {
        _boxCollider.enabled = false;
        yield return new WaitForSeconds(_deactivationDuration);
        _boxCollider.enabled = true;
    }
}
