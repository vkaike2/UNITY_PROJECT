using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Calcatz.MeshPathfinding;
using Unity.VisualScripting;
using UnityEngine;

public class SecondMapPlatformHelper : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private ScriptableMapEvents _mapEvents;

    [Header("Step (0)")]
    [SerializeField]
    private MapChanges _initialStep;

    [Header("STEP (1)")]
    [SerializeField]
    private MapChanges _firstStepChanges;
    [Space]
    [SerializeField]
    private MapChanges _firstStep_One;
    [Space]
    [SerializeField]
    private MapChanges _firstStep_Two;
    [Space]
    [SerializeField]
    private MapChanges _firstMap_Three;
    [Space]
    [SerializeField]
    private MapChanges _firstMap_Four;

    [Header("STEP (2)")]
    [SerializeField]
    private MapChanges _secondStepChange;
    [Space]
    [SerializeField]
    private MapChanges _secondStep_One;
    [Space]
    [SerializeField]
    private MapChanges _secondStep_Two;
    [Space]
    [SerializeField]
    private MapChanges _secondStep_Three;

    [Header("STEP (3)")]
    [SerializeField]
    private MapChanges _thirdStepChange;

    private void Awake()
    {
        ResetAll();
    }

    private void Start()
    {
        _mapEvents.OnChangeMapEvent.AddListener(OnChangeMap);
    }

    private void ResetAll()
    {
        _firstStepChanges.Reset();
        _firstStep_One.Reset();
        _firstStep_Two.Reset();
        _firstMap_Three.Reset();
        _firstMap_Four.Reset();

        _secondStepChange.Reset();
        _secondStep_One.Reset();
        _secondStep_Two.Reset();
        _secondStep_Three.Reset();

        _thirdStepChange.Reset();

        _initialStep.Apply();
    }

    private void OnChangeMap(int mapId, int changeId)
    {
        if (mapId != ConstantValues.SECOND_MAP_ID) return;

        if (changeId == SecondMapChanges.UNLOCK_FIRST_STEP)
        {
            Debug.Log("Step (1)");
            _firstStepChanges.Apply();
        }

        if (changeId == SecondMapChanges.UNLOCK_FIRST_STEP_ONE)
        {
            Debug.Log("Step (1) - 1");
            _firstStep_One.Apply();
        }

        if (changeId == SecondMapChanges.UNLOCK_FIRST_STEP_TWO)
        {
            Debug.Log("Step (1) - 2");
            _firstStep_Two.Apply();
        }

        if (changeId == SecondMapChanges.UNLOCK_FIRST_STEP_THREE)
        {
            Debug.Log("Step (1) - 3");
            _firstMap_Three.Apply();
        }

        if (changeId == SecondMapChanges.UNLOCK_FIRST_STEP_FOUR)
        {
            Debug.Log("Step (1) - 4");
            _firstMap_Four.Apply();
        }

        if (changeId == SecondMapChanges.UNLOCK_SECOND_STEP)
        {
            Debug.Log("Step (2)");
            _secondStepChange.Apply();
        }

        if (changeId == SecondMapChanges.UNLOCK_SECOND_STEP_ONE)
        {
            Debug.Log("Step (2) - 1");
            _secondStep_One.Apply();
        }

        if (changeId == SecondMapChanges.UNLOCK_SECOND_STEP_TWO)
        {
            Debug.Log("Step (2) - 2");
            _secondStep_Two.Apply();
        }

        if (changeId == SecondMapChanges.UNLOCK_SECOND_STEP_THREE)
        {
            Debug.Log("Step (2) - 3");
            _secondStep_Three.Apply();
        }

        if (changeId == SecondMapChanges.UNLOCK_THIRD_STEP)
        {
            Debug.Log("Step (3)");
            _thirdStepChange.Apply();
        }

    }

    [Serializable]
    private class MapChanges
    {
        #region HIDE
        [field: Header("HIDE")]

        [field: Header("MAP")]
        [field: SerializeField]
        public List<GameObject> TilesToHide { get; private set; }
        [field: SerializeField]
        public List<OneWayPlatform> OneWayPlatformsToHide { get; private set; }

        [field: Header("OBJECTS")]
        [field: SerializeField]
        public List<Stalactite> StalactitesToHide { get; private set; }
        [field: SerializeField]
        public List<StoneBallSpawner> StoneBallsToHide { get; private set; }
        [field: SerializeField]
        public List<Spike> SpikesToHide { get; private set; }
        [field: SerializeField]
        public List<MapTrigger> TriggersToHide { get; private set; }
        [field: SerializeField]
        public List<GameObject> GoToInfoToHide { get; private set; }

        [field: Header("PATHFINDING")]
        [field: SerializeField]
        public List<Node> NodesToHide { get; private set; }

        [field: Header("ENEMY SPAWNER")]
        [field: SerializeField]
        public List<EnemySpawnPosition> SpawnPositionsToHide { get; set; }
        #endregion

        #region SHOW
        [field: Header("SHOW")]

        [field: Header("MAP")]
        [field: SerializeField]
        public List<GameObject> TilesToShow { get; private set; }
        [field: SerializeField]
        public List<OneWayPlatform> OneWayPlatformsToShow { get; private set; }

        [field: Header("OBJECTS")]
        [field: SerializeField]
        public List<Stalactite> StalactitesToShow { get; private set; }
        [field: SerializeField]
        public List<StoneBallSpawner> StoneBallsToShow { get; private set; }
        [field: SerializeField]
        public List<Spike> SpikesToShow { get; private set; }
        [field: SerializeField]
        public List<MapTrigger> TriggersToShow { get; private set; }
        [field: SerializeField]
        public List<GameObject> GoToInfoToShow { get; private set; }

        [field: Header("PATHFINDING")]
        [field: SerializeField]
        public List<Node> NodesToShow { get; private set; }
        [field: Header("ENEMY SPAWNER")]
        [field: SerializeField]
        public List<EnemySpawnPosition> SpawnPositionsToShow { get; set; }
        #endregion

        public void ApplyOnGameObjects(List<GameObject> hides, List<GameObject> shows = null)
        {
            if (hides != null)
            {
                foreach (var hide in hides)
                {
                    hide.SetActive(false);
                }
            }

            if (shows != null)
            {
                foreach (var show in shows)
                {
                    show.SetActive(true);
                }
            }
        }

        public void ApplyOnComponents(List<MonoBehaviour> hides, List<MonoBehaviour> shows = null)
        {
            ApplyOnGameObjects(hides.Select(e => e.gameObject).ToList(), shows?.Select(e => e.gameObject).ToList());
        }

        public void ApplyOnSpikes()
        {
            if (SpikesToHide != null)
            {
                foreach (var spike in SpikesToHide)
                {
                    spike.Activate(false);
                }
            }

            if (SpikesToShow != null)
            {
                foreach (var spike in SpikesToShow)
                {
                    spike.Activate(true);
                }
            }
        }
        private void ResetSpikes()
        {
            IEnumerable<Spike> allSpikes = SpikesToHide?.Concat(SpikesToShow);

            if (allSpikes != null)
            {
                foreach (var spike in allSpikes)
                {
                    spike.Activate(false);
                }
            }
        }

        private void ApplyOnNodes()
        {
            if (NodesToHide != null)
            {
                foreach (var node in NodesToHide)
                {
                    node.traversable = false;
                }
            }

            if (NodesToShow != null)
            {
                foreach (var node in NodesToShow)
                {
                    node.traversable = true;
                }
            }
        }
        private void ResetNodes()
        {
            IEnumerable<Node> allNodes = NodesToHide.Concat(NodesToShow);

            if (allNodes != null)
            {
                foreach (var node in allNodes)
                {
                    node.traversable = false;
                }
            }
        }

        private void ApplyOnEnemySpawners()
        {
            if (SpawnPositionsToHide != null)
            {
                foreach (var spawnPosition in SpawnPositionsToHide)
                {
                    spawnPosition.IsAvailable = false;
                }
            }

            if (SpawnPositionsToShow != null)
            {
                foreach (var spawnPosition in SpawnPositionsToShow)
                {
                    spawnPosition.IsAvailable = true;
                }
            }
        }
        private void ResetEnemySpawners()
        {
            IEnumerable<EnemySpawnPosition> allSpawners = SpawnPositionsToHide.Concat(SpawnPositionsToShow);

            if (allSpawners != null)
            {
                foreach (var spawnPosition in allSpawners)
                {
                    spawnPosition.IsAvailable = false;
                }
            }
        }

        private void ApplyOnStoneBalls()
        {
            if (StoneBallsToHide != null)
            {
                foreach (StoneBallSpawner stoneBall in StoneBallsToHide)
                {
                    stoneBall.Disable();
                    stoneBall.gameObject.SetActive(false);
                }
            }

            if (StoneBallsToShow != null)
            {
                foreach (StoneBallSpawner stoneBall in StoneBallsToShow)
                {
                    stoneBall.gameObject.SetActive(true);
                }
            }
        }

        private void ResetStoneBalls()
        {
            var allStoneBalls = StoneBallsToHide?.Concat(StoneBallsToShow);

            foreach (var stoneBall in allStoneBalls)
            {
                stoneBall.Disable();
                stoneBall.gameObject.SetActive(false);
            }
        }

        public void Apply()
        {
            ApplyOnGameObjects(TilesToHide, TilesToShow);
            ApplyOnGameObjects(GoToInfoToHide, GoToInfoToShow);
            ApplyOnSpikes();
            ApplyOnNodes();
            ApplyOnEnemySpawners();
            ApplyOnStoneBalls();

            ApplyOnComponents(
                StalactitesToHide.Select(e => (MonoBehaviour)e).ToList(),
                StalactitesToShow.Select(e => (MonoBehaviour)e).ToList());

            ApplyOnComponents(
                StoneBallsToHide.Select(e => (MonoBehaviour)e).ToList(),
                StoneBallsToShow.Select(e => (MonoBehaviour)e).ToList());

            ApplyOnComponents(
                StalactitesToHide.Select(e => (MonoBehaviour)e).ToList(),
                StalactitesToShow.Select(e => (MonoBehaviour)e).ToList());

            ApplyOnComponents(
                OneWayPlatformsToHide.Select(e => (MonoBehaviour)e).ToList(),
                OneWayPlatformsToShow.Select(e => (MonoBehaviour)e).ToList());

            ApplyOnComponents(
            TriggersToHide.Select(e => (MonoBehaviour)e).ToList(),
            TriggersToShow.Select(e => (MonoBehaviour)e).ToList());
        }

        public void Reset()
        {
            ApplyOnGameObjects(TilesToHide.Concat(TilesToShow).ToList());
            ApplyOnGameObjects(GoToInfoToHide.Concat(GoToInfoToShow).ToList());

            ResetSpikes();
            ResetNodes();
            ResetEnemySpawners();
            ResetStoneBalls();

            ApplyOnComponents(
                StalactitesToHide.Select(e => (MonoBehaviour)e).Concat(StalactitesToShow.Select(e => (MonoBehaviour)e)).ToList());

            ApplyOnComponents(
                StalactitesToHide.Select(e => (MonoBehaviour)e).Concat(StalactitesToShow.Select(e => (MonoBehaviour)e)).ToList());

            ApplyOnComponents(
                OneWayPlatformsToHide.Select(e => (MonoBehaviour)e).Concat(OneWayPlatformsToShow.Select(e => (MonoBehaviour)e)).ToList());

            ApplyOnComponents(
                TriggersToHide.Select(e => (MonoBehaviour)e).Concat(TriggersToShow.Select(e => (MonoBehaviour)e)).ToList());
        }
    }
}