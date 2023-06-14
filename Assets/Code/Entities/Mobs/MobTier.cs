using System.Collections;
using UnityEngine;


public class MobTier : MonoBehaviour
{
    [Header("PREFAB")]
    [SerializeField]
    private GameObject _tierIconPrefab;


    private void Awake()
    {
    }

    public void AddTier()
    {
        GameObject tierIcon = GameObject.Instantiate(_tierIconPrefab, this.transform.position, Quaternion.identity);
        tierIcon.transform.parent = this.transform;
    }

}
