using System;
using System.Collections;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private CountDownUI _countDownUI;
    [SerializeField]
    private SpriteRenderer _enemySprite;

    private SpawnOption _option;

    private GameManager _gameManager;
    private bool _canSpawn;


    private void Awake()
    {
        _canSpawn = true;
    }

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _gameManager.OnPlayerDead.AddListener(OnPlayerDead);
    }

    public void SpawnEnemy(SpawnOption option, Sprite enemySprite, int countDown = 5)
    {
        _option = option;
        _enemySprite.sprite = enemySprite;
        _countDownUI.OnFinishCountDown.AddListener(OnFinishCountDown);
        _countDownUI.StartCountDown(countDown);
    }
   
    private void OnFinishCountDown()
    {
        if (!_canSpawn)
        {
            Destroy(this.gameObject);
            return;
        }

        Enemy enemy = Instantiate(_option.Enemy, _option.Position, Quaternion.identity);
        enemy.transform.SetParent(_option.EnemyContainer);
        _option.MapManager.EnemiesInsideMap.Add(enemy);

        enemy.AddMapConfigurations(_option.ItemContainer, _option.PossibleDrop);

        Destroy(this.gameObject);
    }

    private void OnPlayerDead(string damageSoruce)
    {
        _canSpawn = false;
    }


    public class SpawnOption
    {
        public SpawnOption(
            Enemy enemy, 
            Vector2 position, 
            Transform enemyContainer,
            MapManager mapManager,
            Transform itemContainer,
            PossibleDrop possibleDrop = null)
        {
            Enemy = enemy;
            Position = position;
            EnemyContainer = enemyContainer;
            MapManager = mapManager; 
            ItemContainer = itemContainer;
            PossibleDrop = possibleDrop;
        }

        public Enemy Enemy { get; set; }
        public Vector2 Position { get; set; }
        public Transform EnemyContainer { get; set; }
        public MapManager MapManager { get; set; }
        public BaseMap ParentMap { get; set; }
        public Transform ItemContainer { get; set; }
        public PossibleDrop PossibleDrop { get; set; }
    }

}
