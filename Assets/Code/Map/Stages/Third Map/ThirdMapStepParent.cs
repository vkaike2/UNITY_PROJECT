using UnityEngine;

public class ThirdMapStepParent : BaseMapStepParent
{
    [Header("CONFIGURATIONS")]
    [SerializeField]
    private Transform _tilesParent;
    [SerializeField]
    private Transform _collidersParent;

    public override void Hide()
    {
        ToggleShowAndHide(false);
    }

    public override void Show()
    {
        ToggleShowAndHide(true);
    }

    private void ToggleShowAndHide(bool value)
    {
        if (_tilesParent != null) _tilesParent.gameObject.SetActive(value);
        if (_collidersParent != null) _collidersParent.gameObject.SetActive(value);
    }
}