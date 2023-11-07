using UnityEngine;
using UnityEngine.UI;

public class ItemBeingUsedUI : MonoBehaviour
{
    private Image _image;


    private void Awake()
    {
        _image = GetComponent<Image>();
    }


    public void ActivateImage(bool activate, Sprite itemSprite = null)
    {
        if (itemSprite != null)
        {
            _image.sprite = itemSprite;
        }

        _image.enabled = activate;
    }

}
