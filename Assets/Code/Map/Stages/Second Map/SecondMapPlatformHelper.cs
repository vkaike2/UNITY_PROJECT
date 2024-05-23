using System;
using System.Collections.Generic;
using System.Linq;
using Calcatz.MeshPathfinding;
using UnityEngine;

public class SecondMapPlatformHelper : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private ScriptableMapEvents _mapEvents;

    [Header("STEPS")]
    [SerializeField]
    private MapChanges _stepZero;

    [SerializeField]
    private MapChanges _stepOne;
    [SerializeField]
    private MapChanges _stepOneOne;
    [SerializeField]
    private MapChanges _stepOneTwo;
    [SerializeField]
    private MapChanges _stepOneThree;
    [SerializeField]
    private MapChanges _stepOneFour;


    [SerializeField]
    private MapChanges _stepTwo;
    [SerializeField]
    private MapChanges _stepTwoOne;
    [SerializeField]
    private MapChanges _stepTwoTwo;
    [SerializeField]
    private MapChanges _stepTwoThree;

    [SerializeField]
    private MapChanges _stepThree;

    private MapChanges _lastMapChangeApplied = null;

    private void Start()
    {
        InitializeAllSteps();
        ResetAllSteps();
        _mapEvents.OnChangeMapEvent.AddListener(OnChangeMap);
    }

    private void ResetAllSteps()
    {
        _stepOne.Reset();
        _stepOneOne.Reset();
        _stepOneTwo.Reset();
        _stepOneThree.Reset();
        _stepOneFour.Reset();

        _stepTwo.Reset();
        _stepTwoOne.Reset();
        _stepTwoTwo.Reset();
        _stepTwoThree.Reset();

        _stepThree.Reset();

        _stepZero.Apply();
        _lastMapChangeApplied = _stepZero;
    }
    private void InitializeAllSteps()
    {
        _stepZero.Initialize(this);
        _stepOne.Initialize(this);
        _stepOneOne.Initialize(this);
        _stepOneTwo.Initialize(this);
        _stepOneThree.Initialize(this);
        _stepOneFour.Initialize(this);

        _stepTwo.Initialize(this);
        _stepTwoOne.Initialize(this);
        _stepTwoTwo.Initialize(this);
        _stepTwoThree.Initialize(this);

        _stepThree.Initialize(this);
    }

    private void OnChangeMap(int mapId, int changeId)
    {
        if (mapId != ConstantValues.SECOND_MAP_ID) return;

        if (changeId == SecondMapChanges.UNLOCK_FIRST_STEP)
        {
            Debug.Log("Step (1)");
            ApplyStepChange(_stepOne);
            return;
        }

        if (changeId == SecondMapChanges.UNLOCK_FIRST_STEP_ONE)
        {
            Debug.Log("Step (1) - 1");
            ApplyStepChange(_stepOneOne);
            return;
        }

        if (changeId == SecondMapChanges.UNLOCK_FIRST_STEP_TWO)
        {
            Debug.Log("Step (1) - 2");
            ApplyStepChange(_stepOneTwo);
            return;
        }

        if (changeId == SecondMapChanges.UNLOCK_FIRST_STEP_THREE)
        {
            Debug.Log("Step (1) - 3");
            ApplyStepChange(_stepOneThree);
            return;
        }

        if (changeId == SecondMapChanges.UNLOCK_FIRST_STEP_FOUR)
        {
            Debug.Log("Step (1) - 4");
            ApplyStepChange(_stepOneFour);
            return;
        }

        if (changeId == SecondMapChanges.UNLOCK_SECOND_STEP)
        {
            Debug.Log("Step (2)");
            ApplyStepChange(_stepTwo);
            return;
        }

        if (changeId == SecondMapChanges.UNLOCK_SECOND_STEP_ONE)
        {
            Debug.Log("Step (2) - 1");
            ApplyStepChange(_stepTwoOne);
            return;
        }

        if (changeId == SecondMapChanges.UNLOCK_SECOND_STEP_TWO)
        {
            Debug.Log("Step (2) - 2");
            ApplyStepChange(_stepTwoTwo);
            return;
        }

        if (changeId == SecondMapChanges.UNLOCK_SECOND_STEP_THREE)
        {
            Debug.Log("Step (2) - 3");
            ApplyStepChange(_stepTwoThree);
            return;
        }

        if (changeId == SecondMapChanges.UNLOCK_THIRD_STEP)
        {
            Debug.Log("Step (3)");
            ApplyStepChange(_stepThree);
            return;
        }

        if (changeId == SecondMapChanges.HACK_TEST_BOSS)
        {
            TestUnlockBoss();
        }
    }

    private void TestUnlockBoss()
    {
        ApplyStepChange(_stepThree);
    }

    private void ApplyStepChange(MapChanges change)
    {
        change.Apply();
        _lastMapChangeApplied.Reset();
        _lastMapChangeApplied = change;
    }

    [Serializable]
    private class MapChanges
    {
        [field: Header("MAP STEP CHANGE")]
        [field: SerializeField]
        public string StepName { get; private set; }
        [field: SerializeField]
        public MapStepParent MapStep { get; private set; }

        [field: Header("- HIDE")]
        [field: Header("PATHFINDING")]
        [field: SerializeField]
        public List<Node> NodesToHide { get; private set; }
        [field: Header("ENEMY SPAWNER")]
        [field: SerializeField]
        public List<EnemySpawnPosition> SpawnPositionsToHide { get; set; }

        [field: Header("- SHOW")]
        [field: Header("PATHFINDING")]
        [field: SerializeField]
        public List<Node> NodesToShow { get; private set; }
        [field: Header("ENEMY SPAWNER")]
        [field: SerializeField]
        public List<EnemySpawnPosition> SpawnPositionsToShow { get; set; }

        public void Initialize(MonoBehaviour monoBehaviour)
        {
            MapStepParent[] stepParents = monoBehaviour.GetComponentsInChildren<MapStepParent>();
            MapStep = stepParents.FirstOrDefault(e => e.StepName == StepName);
        }

        public void Apply()
        {
            MapStep.Show();

            ApplyOnNodes();
            ApplyOnEnemySpawners();
        }

        public void Reset()
        {
            MapStep.Hide();

            ResetNodes();
            ResetEnemySpawners();
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

    }
}