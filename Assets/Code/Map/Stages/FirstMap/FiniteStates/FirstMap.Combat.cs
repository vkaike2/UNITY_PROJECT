using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class FirstMap : Map
{
    private class InternalCombat : MapFiniteStateBase
    {
        public override FiniteState State => FiniteState.Combat;

        private Chest _chest;
        private ScriptableMapConfiguration _mapConfiguration;
        private Map _map;

        private const int WARNING_COUNT_BEFORE_SPAWN_ENEMY = 3;
        private bool _waitForNextWaveButton = false;

        public override void Start(Map map)
        {
            _map = map;
            _chest = map.Chest;
            _mapConfiguration = map.MapConfiguration;

            _chest.gameObject.SetActive(false);

            _map.GameManager.OnPlayerDead.AddListener(OnPlayerDead);
        }

        private void OnPlayerDead()
        {
           _map.StopAllCoroutines();
        }

        public override void EnterState()
        {
            _map.UIEventManager.OnClickNextWaveButton.AddListener(OnClickNextWaveButton);
            _map.StartCoroutine(HandleInnerStages());
        }

        public override void OnExitState()
        {
            _map.UIEventManager.OnClickNextWaveButton.RemoveListener(OnClickNextWaveButton);
        }

        public override void Update() { }

        private IEnumerator HandleInnerStages()
        {
            for (int i = 0; i < _mapConfiguration.InnerStages.Count; i++)
            {
                ScriptableMapConfiguration.InnerStage innerStage = _mapConfiguration.InnerStages[i];
                
                _map.DisplayFeedBack(innerStage.name);

                yield return HandleActions(innerStage);
            }
        }

        private IEnumerator HandleActions(ScriptableMapConfiguration.InnerStage innerStage)
        {
            for (int i = 0; i < innerStage.Actions.Count; i++)
            {
                ScriptableMapConfiguration.MapAction action = innerStage.Actions[i];

                Action onWaitForPlayerAction = null;
                switch (action.Type)
                {
                    case ScriptableMapConfiguration.MapAction.ActionType.Monster:
                        onWaitForPlayerAction = SpawnMonster(action);
                        break;
                    case ScriptableMapConfiguration.MapAction.ActionType.Chest:
                        onWaitForPlayerAction = SpawnChest(action);
                        break;
                    case ScriptableMapConfiguration.MapAction.ActionType.ChangeMap:
                        onWaitForPlayerAction = ChangeMapAction(action);
                        break;
                }

                float timer = action.Timer;
                if (timer == 0)
                {
                    timer = action.Timer + 1;
                }

                yield return new WaitForSeconds(timer);

                if (action.WaitForAllMonstersToBeKilled)
                {
                    while(_map.MapManager.EnemiesInsideMap.Any(e => e != null))
                    {
                        yield return new WaitForFixedUpdate();
                    }
                }

                if (action.WaitButtonSignal)
                {
                    _waitForNextWaveButton = true;
                    onWaitForPlayerAction();

                    while (_waitForNextWaveButton)
                    {
                        yield return new WaitForFixedUpdate();
                    }
                }
            }
        }

        private Action ChangeMapAction(ScriptableMapConfiguration.MapAction action)
        {
            _map.MapEvents.OnChangeMapEvent.Invoke(_map.MapConfiguration.Id, action.MapChange.ChangeId);

            return () => { _map.UIEventManager.OnActivateNextWaveButton.Invoke(); };
        }

        private Action SpawnMonster(ScriptableMapConfiguration.MapAction action)
        {
            ScriptableMapConfiguration.MapEnemy enemy = action.Enemy;

            Vector2? spawnPosition = null;
            if (!enemy.UseRandomPosition)
            {

                EnemySpawnPosition enemySpawnPosition = _map.Containers
                    .EnemySpawnPositions
                    .FirstOrDefault(e => 
                        e.Id == enemy.SpawnPositionId
                        && e.IsAvailable 
                        && !e.HasBeingUsedRecently
                        && !e.HasMonsterUnderMe()
                        );

                if(enemySpawnPosition != null)
                {
                    spawnPosition = enemySpawnPosition.transform.position;
                }
            }


            if (enemy.UseRandomPosition || spawnPosition == null)
            {
                List<EnemySpawnPosition> filteredPositions = _map.Containers.EnemySpawnPositions
                    .Where(e => e.Type == enemy.ScriptableEnemy.SpawnType 
                        && e.IsAvailable 
                        && !e.HasBeingUsedRecently
                        && !e.HasMonsterUnderMe())
                    .ToList();
                EnemySpawnPosition randomPosition;
                
                if (filteredPositions.Count == 1)
                {
                    randomPosition = filteredPositions.FirstOrDefault();
                }
                else
                {
                    randomPosition = filteredPositions[UnityEngine.Random.Range(0, filteredPositions.Count)];
                }

                
                //This 0.3f is added to give some space between each mob on the same spawn position
                randomPosition.UseIt(WARNING_COUNT_BEFORE_SPAWN_ENEMY + 2f);

                spawnPosition = randomPosition.transform.position;
            }

            EnemySpawner.SpawnOption spawnOption =
                new EnemySpawner.SpawnOption(
                enemy.ScriptableEnemy.Enemy,
                spawnPosition.Value,
                _map.Containers.EnemiesContainer.transform,
                _map.MapManager,
                _map.Containers.ItemContainer,
                action.Enemy.ScriptablePossibleDrop?.PossibleDrop
                );

            EnemySpawner spawnedEnemy = Instantiate(enemy.ScriptableEnemy.Spawner, spawnOption.Position, Quaternion.identity);

            spawnedEnemy.transform.parent = _map.Containers.EnemiesContainer.transform;
            spawnedEnemy.SpawnEnemy(spawnOption, enemy.ScriptableEnemy.SpawnerSprite, WARNING_COUNT_BEFORE_SPAWN_ENEMY);

            return () => { _map.UIEventManager.OnActivateNextWaveButton.Invoke(); };
        }

        private Action SpawnChest(ScriptableMapConfiguration.MapAction action)
        {
            _chest.gameObject.SetActive(true);
            _chest.ActivateChest();

            _chest.SetPossibleDrops(action.MapChest.ScriptablePossibleDrop);

            if (action.WaitButtonSignal)
            {
                _chest.OnInteractWith.AddListener(OnInteractWithChest);
            }

            return () => { };
        }

        private void OnInteractWithChest()
        {
            _map.UIEventManager.OnActivateNextWaveButton.Invoke();
            _chest.OnInteractWith.RemoveListener(OnInteractWithChest);
        }

        private void OnClickNextWaveButton()
        {
            _waitForNextWaveButton = false;
        }
    }
}
