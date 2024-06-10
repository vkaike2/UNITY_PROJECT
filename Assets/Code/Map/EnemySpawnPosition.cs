using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public bool HasBeingUsedRecently { get; private set; } = false;

    private void OnValidate()
    {
        RenameObject();
    }

    public void RenameObject()
    {
        string availabilityName = IsAvailable ? "x" : " ";
        gameObject.name = $"[{availabilityName}][{Type}]Position {Id}";
    }

    public bool HasMonsterUnderMe()
    {
        List<Enemy> mob = RaycastUtils.GetComponentsUnderPosition<Enemy>(this.transform.position);
        return mob.Any();
    }

    public enum SpawnType
    {
        Ground,
        Air,
        Water
    }

    public void UseIt(float duringCdw)
    {
        StartCoroutine(CalculateUsageCdw(duringCdw));
    }

    private IEnumerator CalculateUsageCdw(float cdw)
    {
        HasBeingUsedRecently = true;
        yield return new WaitForSeconds(cdw);
        HasBeingUsedRecently = false;
    }
}
