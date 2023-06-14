using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Calcatz.MeshPathfinding
{
    [RequireComponent(typeof(Pathfinding))]
    public class PathfindingUserBase : MonoBehaviour
    {
        protected Pathfinding pathfinding;

        private void Awake()
        {
            pathfinding = GetComponent<Pathfinding>();
        }

    }
}
