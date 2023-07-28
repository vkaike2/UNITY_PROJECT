using System.Collections;
using UnityEngine;


public class EnemySpawnPosition : MonoBehaviour
{
    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public int Id { get; set; } = 0;
    [field: SerializeField]
    public SpawnType Type { get; private set; }
    [field: SerializeField]
    public bool IsAvailable { get; set; } = false;
    [field: SerializeField]
    public ScriptableMapConfiguration.MapStage Stage { get; private set; }

    private void OnValidate()
    {
        RenameObject();
    }

    public void RenameObject()
    {
        string availabilityName = IsAvailable ? "x" : " ";
        gameObject.name = $"[{availabilityName}][{Type}]Position {Id}";
    }

    public enum SpawnType
    {
        Ground,
        Air
    }


}
