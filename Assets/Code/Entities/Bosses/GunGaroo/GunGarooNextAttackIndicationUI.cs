using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunGarooNextAttackIndicationUI : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    private void Awake()
    {
        _image.enabled = false;
    }

    public void SetSpriteFor(Sprite sprite, float seconds)
    {
        StartCoroutine(ChangeImageForSeconds(sprite, seconds));
    }

    private IEnumerator ChangeImageForSeconds(Sprite sprite, float seconds)
    {
        _image.enabled = true;
        _image.sprite = sprite;
        yield return new WaitForSeconds(seconds);
        _image.enabled = false;
    }
}
