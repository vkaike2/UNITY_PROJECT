using Calcatz.MeshPathfinding;
using Cinemachine.Utility;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class ChickenFollowingPlayerBehaviour : ChickenFollowingBehaviour
{
    public override Chicken.Behaviour Behaviour => Chicken.Behaviour.FollowingPlayer;


    public override void Start(Enemy enemy)
    {
        base.Start(enemy);

        _onInteractWithTarget = (e) => InteractWithTarget(e);
    }

    public override void OnEnterBehaviour()
    {
        _chicken.PlayerPathfinding.StartFindPath(_chicken.JumpForce);
        _pathfinding = _chicken.PlayerPathfinding;
    }

    public override void OnExitBehaviour()
    {
        _chicken.WormPathfinding.StopPathFinding();
        base.OnExitBehaviour();
    }

    private void InteractWithTarget(Target target)
    {
        if (target == null) return;
        if(target.TargetTransform== null) return;


        Player player = _chicken.gameObject.GetComponent<Player>();
        Debug.Log("Atk Player");
    }
}
