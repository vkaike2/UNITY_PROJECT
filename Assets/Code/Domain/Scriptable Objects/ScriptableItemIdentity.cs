using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item Identity")]
public class ScriptableItemIdentity : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: TextArea]
    [field: SerializeField]
    public string Description { get; private set; }
    [field: SerializeField]
    public bool HasLimit { get; private set; } = false;
    [field: SerializeField]
    public int Limit { get; private set; }
}
