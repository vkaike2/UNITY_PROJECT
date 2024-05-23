using UnityEngine;

public class ThirdMapStepParent : MapStepParent
{
    [Header("CONFIGURATIONS")]
    [SerializeField]
    private Transform _tilesParent;
    [SerializeField]
    private Transform _collidersParent;

    public override void Hide()
    {
        throw new System.NotImplementedException();
    }

    public override void Show()
    {
        throw new System.NotImplementedException();
    }
}