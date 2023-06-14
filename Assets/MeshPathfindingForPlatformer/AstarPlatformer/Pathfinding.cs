
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using System.Linq;

namespace Calcatz.MeshPathfinding
{

    public class Pathfinding : MonoBehaviour
    {

        [Tooltip("The waypoints used to calculate the path.")]
        public Waypoints waypoints;

        [FormerlySerializedAs("target")]
        [Tooltip("The target position, defined by a transform's position, to create the path.")]
        [SerializeField] private Transform m_target;

        [Tooltip("Whether or not to include the starting node in the calculated path.")]
        [SerializeField] private bool m_includeStartingNode = true;

        public Transform Target => m_target;

        private Node[] pathResult;

        /*
         * Each Pathfinding user has their own node data dictionary
         * since each has their own path calculation,
         * but they share the same node objects
         */
        private Dictionary<Node, Node.Data> nodeDataDictionary;
        private Node.Data startNodeData;

        public bool includeStartingNode { get => m_includeStartingNode; set => m_includeStartingNode = value; }

        public void StopPathFinding()
        {
            StopAllCoroutines();
        }

        public void StartFindPath(Transform _target, float _unitHeight, bool _looping = true)
        {
            if (_target != null)
            {
                m_target = _target;
            }
            StartFindPath(_unitHeight, _looping);
        }

        public void StartFindPath(float _jumpForce)
        {
            StartFindPath(1, true, _jumpForce);
        }

        public void StartFindPath(float _unitHeight, bool _looping = true, float _jumpForce = 0)
        {
            InitNodeDataDictionary();
            StartCoroutine(FindPath(_unitHeight, _looping, _jumpForce));
        }

        public void SetTarget(Transform _target)
        {
            m_target = _target;
        }
        public Node GetStartNode()
        {
            return startNodeData.nodeObject;
        }
        public Node[] GetPathResult()
        {
            return pathResult;
        }

        private void InitNodeDataDictionary()
        {
            nodeDataDictionary = new Dictionary<Node, Node.Data>();
            foreach (Node node in waypoints.nodes)
            {
                Node.Data nodeData = new Node.Data(node);
                nodeDataDictionary.Add(node, nodeData);
            }
        }

        private IEnumerator FindPath(float _unitHeight, bool _looping, float _jumpForce)
        {
            do
            {
                foreach (Node.Data nodeData in nodeDataDictionary.Values)
                {
                    nodeData.ResetNode();
                }

                bool success = false;

                startNodeData = nodeDataDictionary[waypoints.FindNodeLessThanHeight(transform.position, _unitHeight)];
                Node.Data targetNodeData = nodeDataDictionary[waypoints.FindNode(m_target.position)];

                if (startNodeData.nodeObject.traversable && targetNodeData.nodeObject.traversable)
                {
                    Heap<Node.Data> openSet = new Heap<Node.Data>(nodeDataDictionary.Count);
                    HashSet<Node.Data> closedSet = new HashSet<Node.Data>();

                    openSet.Add(startNodeData);
                    while (openSet.Count > 0)
                    {
                        Node.Data currentNode = openSet.RemoveFirstItem();

                        closedSet.Add(currentNode);

                        if (currentNode == targetNodeData)
                        {
                            success = true;
                            break;
                        }

                        foreach (Node.Neighbours neighbour in currentNode.nodeObject.neighbours)
                        {
                            Node.Data neighbourData = nodeDataDictionary[neighbour.node];
                            if (!neighbourData.nodeObject.traversable || closedSet.Contains(neighbourData))
                            {
                                continue;
                            }

                            float newCostToNeighbour = currentNode.gCost + GetDistance(currentNode.nodeObject, neighbour.node);
                            if (newCostToNeighbour < neighbourData.gCost || !openSet.Contains(neighbourData))
                            {
                                neighbourData.gCost = newCostToNeighbour;
                                neighbourData.hCost = GetDistance(neighbour.node, targetNodeData.nodeObject);
                                neighbourData.parent = currentNode;
                                if (!openSet.Contains(neighbourData))
                                {
                                    openSet.Add(neighbourData);
                                }
                            }
                        }
                    }
                }
                if (success)
                {
                    MakePath(startNodeData, targetNodeData, _jumpForce);
                }
                yield return new WaitForSeconds(0.25f);
            } while (_looping);
        }

        public Node[] FindPath(float _jumpForce, float _unitHeight = 1, Transform target = null)
        {
            if(target != null)
            {
                this.SetTarget(target);
            }

            InitNodeDataDictionary();

            foreach (Node.Data nodeData in nodeDataDictionary.Values)
            {
                nodeData.ResetNode();
            }

            bool success = false;

            startNodeData = nodeDataDictionary[waypoints.FindNodeLessThanHeight(transform.position, _unitHeight)];
            Node.Data targetNodeData = nodeDataDictionary[waypoints.FindNode(m_target.position)];

            if (startNodeData.nodeObject.traversable && targetNodeData.nodeObject.traversable)
            {
                Heap<Node.Data> openSet = new Heap<Node.Data>(nodeDataDictionary.Count);
                HashSet<Node.Data> closedSet = new HashSet<Node.Data>();

                openSet.Add(startNodeData);
                while (openSet.Count > 0)
                {
                    Node.Data currentNode = openSet.RemoveFirstItem();

                    closedSet.Add(currentNode);

                    if (currentNode == targetNodeData)
                    {
                        success = true;
                        break;
                    }

                    foreach (Node.Neighbours neighbour in currentNode.nodeObject.neighbours)
                    {
                        Node.Data neighbourData = nodeDataDictionary[neighbour.node];
                        if (!neighbourData.nodeObject.traversable || closedSet.Contains(neighbourData))
                        {
                            continue;
                        }

                        float newCostToNeighbour = currentNode.gCost + GetDistance(currentNode.nodeObject, neighbour.node);
                        if (newCostToNeighbour < neighbourData.gCost || !openSet.Contains(neighbourData))
                        {
                            neighbourData.gCost = newCostToNeighbour;
                            neighbourData.hCost = GetDistance(neighbour.node, targetNodeData.nodeObject);
                            neighbourData.parent = currentNode;
                            if (!openSet.Contains(neighbourData))
                            {
                                openSet.Add(neighbourData);
                            }
                        }
                    }
                }
            }
            if (success)
            {
                return MakePath(startNodeData, targetNodeData, _jumpForce);
            }

            return null;
        }

        Node[] MakePath(Node.Data _startNodeData, Node.Data _targetNodeData, float _jumpForce)
        {
            List<Node> path = new List<Node>();
            Node.Data currentNode = _targetNodeData;

            while (currentNode != _startNodeData)
            {
                path.Add(currentNode.nodeObject);

                if (currentNode.parent.nodeObject.neighbours.FirstOrDefault(e => e.node == currentNode.nodeObject).jumpForceToReach > _jumpForce)
                {
                    path = null;
                    break;
                }
                currentNode = currentNode.parent;
            }

            if (path == null)
            {
                pathResult = null;
                return pathResult;
            }

            if (includeStartingNode)
            {
                path.Add(_startNodeData.nodeObject);
            }

            pathResult = path.ToArray();
            System.Array.Reverse(pathResult);
            return pathResult;
        }

        float GetDistance(Node nodeA, Node nodeB)
        {
            return Vector3.Distance(nodeA.transform.position, nodeB.transform.position);
        }

        private void OnDrawGizmosSelected()
        {
            if (pathResult != null)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < pathResult.Length; i++)
                {
                    Gizmos.DrawWireSphere(pathResult[i].transform.position, 0.25f);
                    Gizmos.color = Color.red;
                    if (i < pathResult.Length - 1)
                    {
                        Gizmos.DrawLine(pathResult[i].transform.position, pathResult[i + 1].transform.position);
                    }
                }
            }
        }

    }
}
