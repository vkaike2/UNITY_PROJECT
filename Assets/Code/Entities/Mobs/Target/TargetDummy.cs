using System.Collections;
using UnityEngine;


public class TargetDummy : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private Transform _rotationalSprite;


    [Header("CONFIGURATIONS")]
    [SerializeField]
    private bool _isFacingRight = true;


    private void Start()
    {
        _rotationalSprite.localScale = new Vector3(_isFacingRight ? 1 : -1, 1, 1);
    }
}
