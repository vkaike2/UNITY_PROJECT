using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Calcatz.MeshPathfinding
{
    public class Node : MonoBehaviour
    {

#if UNITY_EDITOR
        [MenuItem("GameObject/Mesh Pathfinding/Create Node")]
        private static Node CreateNode()
        {
            Waypoints waypoints = null;
            Node originNode = null;
            if (Selection.activeGameObject == null)
            {
                waypoints = SearchWaypoints();
            }
            else
            {
                waypoints = Selection.activeGameObject.GetComponent<Waypoints>();
                if (waypoints == null)
                {
                    originNode = Selection.activeGameObject.GetComponent<Node>();
                    if (originNode != null)
                    {
                        waypoints = originNode.transform.parent.GetComponent<Waypoints>();
                    }
                    else
                    {
                        waypoints = SearchWaypoints();
                    }
                }
            }
            GameObject go = new GameObject("Node");
            go.transform.parent = waypoints.transform;
            Node newNode = go.AddComponent<Node>();
            if (originNode != null)
            {
                go.transform.position = originNode.transform.position;
                go.transform.rotation = originNode.transform.rotation;
                go.transform.localScale = originNode.transform.localScale;
                if (originNode.neighbours == null)
                {
                    originNode.neighbours = new List<Neighbours>();
                }
                originNode.neighbours.Add(new Neighbours(newNode));
                newNode.neighbours = new List<Neighbours>();
                newNode.neighbours.Add(new Neighbours(originNode));
            }
            Selection.activeGameObject = go;
            return newNode;
        }

        private static Waypoints SearchWaypoints()
        {
            Waypoints waypoints = FindObjectOfType<Waypoints>();
            if (waypoints == null)
            {
                waypoints = Waypoints.CreateWaypoints();
            }

            return waypoints;
        }
#endif

        [Tooltip("This node will be ignored in calculation if it's not traversable.")]
        public bool traversable = true;
        public bool needToJumpIfFirst = false;

        public List<Neighbours> neighbours;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.15f);
            if (neighbours == null) return;
            List<Neighbours> emptyNeighbours = new List<Neighbours>();
            foreach (Neighbours neighbour in neighbours)
            {
                if (neighbour.node == null) continue;
                if (neighbour != null)
                {
                    try
                    {
                        if (neighbour.node.neighbours != null)
                        {
                            if (!neighbour.node.neighbours.Any(e => e.node.GetInstanceID() == this.GetInstanceID()))
                            {
                                Gizmos.color = Color.white;
                            }
                        }
                    }
                    catch
                    {
                        emptyNeighbours.Add(neighbour);
                    }

                    Gizmos.DrawLine(transform.position, neighbour.node.transform.position);
                }
                else
                {
                    emptyNeighbours.Add(neighbour);
                }
            }
            foreach (Neighbours emptyNeighbour in emptyNeighbours)
            {
                neighbours.Remove(emptyNeighbour);
            }
        }

        private void OnValidate()
        {
            if (neighbours == null) return;

            neighbours = neighbours.Where(e => e.node != null).ToList();

            foreach (var neighbour in neighbours)
            {
                neighbour.name = neighbour.node.gameObject.name;
            }
        }

        public class Data : IHeapItem<Data>
        {
            public Node nodeObject;
            public float gCost, hCost;
            public Data parent;
            public bool onPath;
            int heapIndex;

            public Data(Node _nodeObject)
            {
                nodeObject = _nodeObject;
                gCost = hCost = 0;
            }

            public int HeapIndex
            {
                get { return heapIndex; }
                set { heapIndex = value; }
            }

            public float fCost
            {
                get { return gCost + hCost; }
            }

            public int CompareTo(Data nodeData)
            {
                int compare = fCost.CompareTo(nodeData.fCost);
                if (compare == 0)
                {
                    compare = hCost.CompareTo(nodeData.hCost);
                }
                return -1 * compare;
            }
            public void ResetNode()
            {
                gCost = hCost = 0;
                parent = null;
            }
        }

        [Serializable]
        public class Neighbours
        {
            [HideInInspector]
            public string name;
            public Node node;

            public bool needToJump;
            public bool needToGoDownPlatform;

            public Neighbours() { }

            public Neighbours(Node node)
            {
                this.node = node;
            }

        }
    }
}
