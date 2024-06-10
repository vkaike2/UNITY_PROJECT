using System;
using System.Collections.Generic;
using System.Linq;
using Calcatz.MeshPathfinding;
using UnityEngine;

public abstract class BaseMapPlatformHelper : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    protected ScriptableMapEvents _mapEvents;

    protected MapChanges _lastMapChangeApplied = null;

    protected abstract void ResetAllSteps();
    protected abstract void InitializeAllSteps();
    protected abstract void OnChangeMap(int mapId, int changeId);

    private void Start()
    {
        InitializeAllSteps();
        ResetAllSteps();
        _mapEvents.OnChangeMapEvent.AddListener(OnChangeMap);
    }


    protected void ApplyStepChange(MapChanges change)
    {
        change.Apply();
        _lastMapChangeApplied.Reset();
        _lastMapChangeApplied = change;
    }

    [Serializable]
    protected class MapChanges
    {
        [field: Header("MAP STEP CHANGE")]
        [field: SerializeField]
        public string StepName { get; private set; }
        [field: SerializeField]
        public BaseMapStepParent MapStep { get; private set; }

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
            BaseMapStepParent[] stepParents = monoBehaviour.GetComponentsInChildren<BaseMapStepParent>();
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