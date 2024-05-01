using System.Collections;
using UnityEngine;

public class SecondMapBossContainer : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private ScriptableMapEvents _mapEvents;
    [Space]
    [SerializeField]
    private ArmadilloContainer _armadilloContainer;
    
    [Header("SCENARIO")]
    [SerializeField]
    private GameObject _blockTiles;

    [Header("TEST")]
    [SerializeField]
    private Transform _initialPlayerPosition;

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _cdwToWaitBeforeSpawnBoss = 2f;


    private Player _player;

    private void Start()
    {
        _mapEvents.OnChangeMapEvent.AddListener(OnChangeMap);

        StartCoroutine(GetPlayerInstance());
    }

    private void OnChangeMap(int mapId, int changeId)
    {
        if (mapId != ConstantValues.SECOND_MAP_ID) return;

        if (changeId == SecondMapChanges.UNLOCK_THIRD_STEP)
        {
            StartCoroutine(WaitToSpawnBoss());
        }

        if (changeId == SecondMapChanges.HACK_TEST_BOSS)
        {
            TestBossStage();
        }
    }

    public void TestBossStage()
    {
        _player.transform.position = _initialPlayerPosition.position;
        StartCoroutine(WaitToSpawnBoss());
    }

    private IEnumerator GetPlayerInstance()
    {
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        yield return new WaitUntil(() => gameManager.Player != null);
        _player = gameManager.Player;
    }

    private IEnumerator WaitToSpawnBoss()
    {
        yield return new WaitForSeconds(_cdwToWaitBeforeSpawnBoss);
        _armadilloContainer.SpawnBoss();

        _blockTiles.SetActive(true);
    }
}