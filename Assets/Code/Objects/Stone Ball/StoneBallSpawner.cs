using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField]
    private float _direction = 1;

    private List<StoneBall> _balls = new List<StoneBall>();

    private void Start()
    {
        StartCoroutine(ManageCdw());
    }

    public void Disable()
    {
        foreach (StoneBall ball in _balls)
        {
            if (ball == null) continue;

            Destroy(ball.gameObject);
        }
    }

    private IEnumerator ManageCdw()
    {
        _cdwIndication.StartCdw(_cdwBetweenSpawn);
        yield return new WaitForSeconds(_cdwBetweenSpawn);
        StoneBall stoneBall = Instantiate(_ballPrefab, this.transform.position, Quaternion.identity);
        stoneBall.transform.parent = null;

        stoneBall.SetDirection(_direction);

        _balls.Add(stoneBall);

        StartCoroutine(ManageCdw());
    }
}