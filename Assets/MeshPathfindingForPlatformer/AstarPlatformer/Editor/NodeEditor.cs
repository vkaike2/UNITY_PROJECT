using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Calcatz.MeshPathfinding
{
    [CustomEditor(typeof(Node)), CanEditMultipleObjects]
    class NodeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Node nodeTarget = (Node)target;
            serializedObject.Update();

            if (GUI.changed)
                serializedObject.ApplyModifiedProperties();

            if (Selection.gameObjects.Length > 1)
            {
                if (GUILayout.Button("Connect Selected Nodes"))
                {
                    List<Node> nodes = new List<Node>();
                    foreach (GameObject go in Selection.gameObjects)
                    {
                        Node node = go.GetComponent<Node>();
                        if (node != null)
                        {
                            nodes.Add(node);
                        }
                    }
                    if (nodes.Count > 1)
                    {
                        foreach (Node nodeA in nodes)
                        {
                            foreach (Node nodeB in nodes)
                            {
                                if (nodeA != nodeB)
                                {
                                    if (nodeA.neighbours == null) nodeA.neighbours = new List<Node.Neighbours>();
                                    if (!nodeA.neighbours.Any(e => e.node.GetInstanceID() == nodeB.GetInstanceID()))
                                    {
                                        nodeA.neighbours.Add(new Node.Neighbours(nodeB));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy | GizmoType.Pickable)]
        static void DrawGameObjectName(Transform transform, GizmoType gizmoType)
        {
            Node node = transform.GetComponent<Node>();
            if (node != null)
            {
                Handles.SphereHandleCap(transform.GetInstanceID(), transform.position, Quaternion.identity, 0.3f, EventType.Repaint);
                if (node.neighbours != null)
                {
                    foreach (Node.Neighbours neighbour in node.neighbours)
                    {
                        try
                        {
                            if (neighbour.node.neighbours != null)
                            {
                                if (!neighbour.node.neighbours.Any(e => e.node.GetInstanceID() == node.GetInstanceID()))
                                {
                                    float distance = Vector3.Distance(neighbour.node.transform.position, node.transform.position);
                                    Quaternion rot = Quaternion.LookRotation(neighbour.node.transform.position - node.transform.position);
                                    Handles.ArrowHandleCap(transform.GetInstanceID(), transform.position, rot, distance / 2, EventType.Repaint);
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                }
                Handles.Label(transform.position, transform.gameObject.name);
            }
        }
    }
}