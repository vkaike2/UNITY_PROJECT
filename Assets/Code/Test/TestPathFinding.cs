using Calcatz.MeshPathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathFinding : MonoBehaviour
{
    private Pathfinding _pathfinding;
    public float jumpForce = 0;

    private void Awake()
    {
        _pathfinding = GetComponent<Pathfinding>();
    }

    void Start()
    {
        _pathfinding.StartFindPath(0.3f, true, jumpForce);
    }

    void FixedUpdate()
    {
        Node[] pathResult = _pathfinding.GetPathResult();

        if (pathResult == null)
        {
            Debug.Log("UNREACHABLE");
        }
        else
        {
            Debug.Log("REACHABLE");
        }
    }
}
