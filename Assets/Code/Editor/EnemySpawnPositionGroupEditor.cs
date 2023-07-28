using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(EnemySpawnPositionGroup)), CanEditMultipleObjects]
public class EnemySpawnPositionGroupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EnemySpawnPositionGroup positionGroup = (EnemySpawnPositionGroup)target;

        if (GUILayout.Button("Generate Ids"))
        {
            positionGroup.GenerateSequentialIds();
        }
    }
}
