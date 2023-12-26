using System.Collections;
using UnityEngine;

public class StoneBallSpawner : MonoBehaviour
{
    [Header("UI COMPONENTS")]
    [SerializeField]
    private CdwIndicationUI _cdwIndication;

    [Header("COMPONENTS")]
    [SerializeField]
    private StoneBall _ballPrefab;

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _cdwBetweenSpawn;

    private void Start()
    {
        StartCoroutine(ManageCdw());
    }

    private IEnumerator ManageCdw()
    {
        _cdwIndication.StartCdw(_cdwBetweenSpawn);
        yield return new WaitForSeconds(_cdwBetweenSpawn);
        StoneBall stoneBall = Instantiate(_ballPrefab, this.transform.position, Quaternion.identity);
        stoneBall.transform.parent = null;
        
        StartCoroutine(ManageCdw());
    }
}