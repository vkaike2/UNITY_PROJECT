using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Calcatz.MeshPathfinding
{

    public class Waypoints : MonoBehaviour
    {

#if UNITY_EDITOR
        [MenuItem("GameObject/Mesh Pathfinding/Create Waypoints")]
        public static Waypoints CreateWaypoints()
        {
            GameObject go = new GameObject("Waypoints");
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.Euler(0, 0, 0);
            go.transform.localScale = Vector3.one;
            Selection.activeGameObject = go;
            return go.AddComponent<Waypoints>();
        }
#endif

        public List<Node> nodes;

        private void OnValidate()
        {
            if (nodes != null)
            {
                nodes.Clear();
            }
            else
            {
                nodes = new List<Node>();
            }
            Node[] nodesArray = GetComponentsInChildren<Node>();
            foreach (Node node in nodesArray)
            {
                nodes.Add(node);
            }
        }

        public Node FindNode(Vector3 _position)
        {
            int waypointIndex = 0;
            float distance = Vector3.Distance(_position, nodes[0].transform.position);
            for (int i = 0; i < nodes.Count; i++)
            {
                float newDistance = Vector3.Distance(_position, nodes[i].transform.position);

                if (newDistance < distance)
                {
                    distance = newDistance;
                    waypointIndex = i;
                }
            }
            return nodes[waypointIndex];
        }

        public Node FindNodeLessThanHeight(Vector3 _position, float _unitHeight)
        {
            int waypointIndex = 0;
            float maxY = _position.y + _unitHeight;
            float distance = Vector3.Distance(_position, nodes[0].transform.position);
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].transform.position.y > maxY) continue;
                float newDistance = Vector3.Distance(_position, nodes[i].transform.position);

                if (newDistance < distance)
                {
                    distance = newDistance;
                    waypointIndex = i;
                }
            }
            return nodes[waypointIndex];
        }

    }
}
