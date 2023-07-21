using UnityEngine;


public class MapWaitState : MapFiniteStateBase
{
    public override Map.FiniteState State => Map.FiniteState.Wait;

    private Toilet _toilet;
    private Map _map;

    public override void Start(Map map)
    {
        _toilet = map.MapManager.Toilet;
        _map = map;

        _toilet.OnToggleToiletEvent.AddListener(OnCloseToilet);
    }

    public override void EnterState()
    {
        _toilet.OpenToilet();
    }

    public override void OnExitState()
    {
    }

    public override void Update()
    {
    }

    private void OnCloseToilet(Toilet.State toiletState)
    {
        if (toiletState != Toilet.State.Closed) return;

        int childCount = _map.ObjectsPartent.childCount;

        for (int i = 0; i < childCount; i++)
        {
            GameObject.Destroy(_map.ObjectsPartent.GetChild(i).gameObject);
        }

        _map.Animator.Play(Map.MyAnimations.TurningOff.ToString());
        _toilet.DisableIt();

        _map.GameManager.Player.FreezePlayer(true);
        _map.MapManager.StartNextMap(_map);

        GameObject.Destroy(_map.gameObject, 1f);
    }
}
