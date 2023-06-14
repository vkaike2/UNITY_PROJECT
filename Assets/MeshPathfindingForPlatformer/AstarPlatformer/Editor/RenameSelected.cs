using UnityEngine;
using UnityEditor;
using System;

namespace Calcatz.MeshPathfinding
{
    public class RenameSelected : EditorWindow
    {
        // public fields
        public GameObject[] objects;

        // private fields
        private static readonly Vector2Int size = new Vector2Int(250, 100);
        private string _gameObjectPrefix;
        private int _startIndex;
        private SerializedObject _serializedObject;

        [MenuItem("GameObject/Rename Selected")]
        public static void ShowWindow()
        {
            RenameSelected window = GetWindow<RenameSelected>();
            window.minSize = size;
            window.maxSize = size;

            // sort gameobjects by sibling index
            var unsortedGameObjects = Selection.gameObjects;
            var sortedGameObjects = new GameObject[unsortedGameObjects.Length];
            for (var i = 0; i < unsortedGameObjects.Length; i++)
            {
                sortedGameObjects[i] = unsortedGameObjects[i];
            }
            Array.Sort(sortedGameObjects, (a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));

            window.objects = sortedGameObjects;
        }

        private void OnEnable()
        {
            ScriptableObject target = this;
            _serializedObject = new SerializedObject(target);
        }

        private void OnGUI()
        {
            _gameObjectPrefix = EditorGUILayout.TextField("Selected Prefix", _gameObjectPrefix);
            _startIndex = EditorGUILayout.IntField("Start Index", _startIndex);

            _serializedObject.Update();

            SerializedProperty serializedProperty = _serializedObject.FindProperty("objects");

            EditorGUILayout.PropertyField(serializedProperty, true);

            if (GUILayout.Button("Rename Objects"))
            {

                for (int objectI = 0, i = _startIndex; objectI < serializedProperty.arraySize; objectI++)
                {
                    serializedProperty.GetArrayElementAtIndex(objectI).objectReferenceValue.name = $"{_gameObjectPrefix}{i++}";
                }
            }

            _serializedObject.ApplyModifiedProperties();
        }
    }
}