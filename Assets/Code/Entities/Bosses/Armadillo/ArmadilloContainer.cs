using UnityEngine;

public class ArmadilloContainer : MonoBehaviour
{
    [Header("PREFAB")]
    [SerializeField]
    private Armadillo _boss;

    [Header("POSITIONS")]
    [SerializeField]
    private Transform _spawnPosition;
    
    [Header("CONTAINER")]
    [SerializeField]
    private Transform _enemyContainer;

    public void SpawnBoss()
    {
        Armadillo boss = Instantiate(_boss, _spawnPosition.transform.position, Quaternion.identity);
        boss.transform.SetParent(_enemyContainer);
    }
}