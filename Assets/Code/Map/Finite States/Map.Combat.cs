using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Map : MonoBehaviour
{
    /// <summary>
    ///  Toilet -> disable
    ///  Enemy  -> will spawn periodically from time to time
    /// </summary>
    private class Combat : MapFiniteStateBase
    {
        public override Map.FiniteState State => Map.FiniteState.Combat;

        private Map _map;
        private CameraConfinerInformation _cameraConfiner;
        private ScriptableMapConfiguration _configuration;
        private readonly List<Enemy> _enemies = new List<Enemy>();

        private bool _canGoToNextStage = false;

        private int? _lastEnemyPosition = null;

        private List<WallInPositionCheck> _wallChecks = new List<WallInPositionCheck>();

        public override void Start(Map map)
        {
            _map = map;
            _cameraConfiner = _map.CameraConfiner;
            _configuration = _map.MapConfiguration;
        }

        public override void EnterState()
        {
            StartSpawningWavesOfMobs();

            foreach (var wall in _map.WallsOfSpike)
            {
                wall.OnPositionReady.AddListener(OnPositionReady);
            }
        }

        public override void OnExitState()
        {
            foreach (var wall in _map.WallsOfSpike)
            {
                wall.OnPositionReady.RemoveListener(OnPositionReady);
            }
        }

        public override void Update()
        {
            CheckIfCanChangeState();
        }

        private void StartSpawningWavesOfMobs()
        {
            if (_configuration == null)
            {
                _canGoToNextStage = true;
                return;
            }

            
            _map.StartCoroutine(ChangeMapAcordinglyToTime());
        }

        private void CheckIfCanChangeState()
        {
            if (!_canGoToNextStage) return;

            if (_enemies.Count != _enemies.Count(e => e == null)) return;

            _map.ChangeState(Map.FiniteState.Wait);
        }

        private IEnumerator ChangeMapAcordinglyToTime()
        {
            _map.StartCoroutine(SpawnWaves());

            yield return new WaitForSeconds(_configuration.EnfOfSmallStage);
            // move to medium
            ChangeMapToMedium();


            yield return new WaitForSeconds(_configuration.EnfOfMediumStage - _configuration.EnfOfSmallStage);
            // move to full size
            ChangeMapToLarge();
        }

        private void ChangeMapToMedium()
        {
            _map.StartCoroutine(ChangeCameraToNextPosition(_cameraConfiner.MediumCollider, _cameraConfiner.MediumCameraSize));
            foreach (var wall in _map.WallsOfSpike)
            {
                wall.GoToPosition(WallOfSpike.PositionType.First);
            }
        }

        private void ChangeMapToLarge()
        {
            _map.StartCoroutine(ChangeCameraToNextPosition(_cameraConfiner.LargeCollider, _cameraConfiner.LargeCameraSize));
            foreach (var wall in _map.WallsOfSpike)
            {
                wall.GoToPosition(WallOfSpike.PositionType.Second);
            }
        }

        private void OnPositionReady(WallOfSpike.PositionType position)
        {
            WallInPositionCheck wallPosition = new WallInPositionCheck();
            wallPosition.position = position;

            _wallChecks.Add(wallPosition);

            if (_wallChecks.Count(e => e.position == position) != 2) return;

            switch (position)
            {
                case WallOfSpike.PositionType.First:
                    ActivateEnemySpawnPositions(ScriptableMapConfiguration.MapStage.Medium);
                    ActivateWaypointsNodes(ScriptableMapConfiguration.MapStage.Medium);
                    break;
                case WallOfSpike.PositionType.Second:
                    ActivateEnemySpawnPositions(ScriptableMapConfiguration.MapStage.Medium);
                    ActivateWaypointsNodes(ScriptableMapConfiguration.MapStage.Medium);

                    ActivateEnemySpawnPositions(ScriptableMapConfiguration.MapStage.Large);
                    ActivateWaypointsNodes(ScriptableMapConfiguration.MapStage.Large);
                    break;
                default:
                    break;
            }
        }

        private void ActivateEnemySpawnPositions(ScriptableMapConfiguration.MapStage Stage)
        {
            foreach (var spawnPositions in _map.Conatiners.EnemySpawnPositions.Where(e => e.Stage == Stage).ToList())
            {
                spawnPositions.IsAvailable = true;
            }
        }

        private void ActivateWaypointsNodes(ScriptableMapConfiguration.MapStage Stage)
        {
            foreach (var node in _map.Nodes.Where(e => e.Stage == Stage).ToList())
            {
                node.traversable = true;
            }
        }

        /// <summary>
        /// Spawn every wave of monsters
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private IEnumerator SpawnWaves(int index = 0)
        {
            ScriptableMapConfiguration.Wave wave = _configuration.Waves[index];

            yield return new WaitForSeconds(wave.WaitToStartSpawningWave);

            foreach (ScriptableMapConfiguration.Mob mob in wave.Mobs)
            {
                Enemy enemy = GameObject.Instantiate(mob.Enemy, GetEnemyPosition(mob), new Quaternion());
                enemy.transform.SetParent(_map.Conatiners.EnemiesContainer);
                _enemies.Add(enemy);

                yield return new WaitForSeconds(mob.CdwToSpawnNextEnemy);
            }

            index++;

            if (_configuration.Waves.Count == index)
            {
                _canGoToNextStage = true;
            }
            else
            {
                _map.StartCoroutine(SpawnWaves(index));
            }
        }

        private Vector3 GetEnemyPosition(ScriptableMapConfiguration.Mob mob)
        {
            if (mob.UseRandomPosition)
            {
                List<EnemySpawnPosition> availablePositions = _map.Conatiners.EnemySpawnPositions.Where(e => e.IsAvailable && e.Type == mob.SpawnType).ToList();
                int randomId = GetRandomId(availablePositions.Count, _lastEnemyPosition);
                _lastEnemyPosition = randomId;

                EnemySpawnPosition randomPosition = availablePositions[randomId];

                return randomPosition.transform.position;
            }
            else
            {
                return _map.Conatiners.EnemySpawnPositions.FirstOrDefault(e => e.Id == mob.SpawnPositionId).transform.position;
            }
        }

        private int GetRandomId(int max, int? excluding) 
        {
            List<int> indexList = new List<int>();
            for (int i = 0; i < max; i++)
            {
                if (excluding.HasValue && excluding.GetValueOrDefault() == i) continue;

                indexList.Add(i);
            }
            
            return indexList[UnityEngine.Random.Range(0, indexList.Count)];
        }

        private IEnumerator ChangeCameraToNextPosition(Collider2D mapConfiner, float cameraSize)
        {
            _map.MapManager.CinemachineConfiner2D.m_BoundingShape2D = mapConfiner;

            while (_map.MapManager.VirualCamera.m_Lens.OrthographicSize <= cameraSize)
            {
                _map.MapManager.VirualCamera.m_Lens.OrthographicSize += Time.deltaTime * 2;
                _map.MapManager.CinemachineConfiner2D.InvalidateCache();
                yield return new WaitForFixedUpdate();
            }
        }

        public class WallInPositionCheck
        {
            public WallOfSpike.PositionType position { get; set; }
        }
    }
}
