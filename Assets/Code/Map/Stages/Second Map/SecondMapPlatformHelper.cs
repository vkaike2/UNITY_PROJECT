using System;
using System.Collections.Generic;
using UnityEngine;

public class SecondMapPlatformHelper : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private ScriptableMapEvents _mapEvents;

    [Header("STEP (1)")]
    [SerializeField]
    private MapChanges _firstStepChanges;

    private void Start()
    {
        _mapEvents.OnChangeMapEvent.AddListener(OnChangeMap);
    }

    private void OnChangeMap(int mapId, int changeId)
    {
        if (mapId != ConstantValues.SECOND_MAP_ID) return;

        if (changeId == SecondMapChanges.UNLOCK_FIRST_STEP) _firstStepChanges.Apply();
    }

    [Serializable]
    private class MapChanges
    {
        [field: Header("HIDE")]
        [field: SerializeField]
        public List<GameObject> TilesToHide { get; private set; }
        [field: SerializeField]
        public List<Stalactite> StalactitesToHide { get; private set; }
        [field: SerializeField]
        public List<StoneBallSpawner> StoneBallsToHide { get; private set; }

        [field: Header("SHOW")]
        [field: SerializeField]
        public List<GameObject> TilesToShow { get; private set; }
        [field: SerializeField]
        public List<Stalactite> StalactitesToShow { get; private set; }
        [field: SerializeField]
        public List<StoneBallSpawner> StoneBallsToShow { get; private set; }


        private void ApplyOnTiles()
        {
            if (TilesToHide != null)
            {
                foreach (var tile in TilesToHide)
                {
                    tile.SetActive(false);
                }
            }

            if (TilesToShow != null)
            {
                foreach (var tile in TilesToShow)
                {
                    tile.SetActive(true);
                }
            }
        }

        private void ApplyOnStalactites()
        {
            if (StalactitesToHide != null)
            {
                foreach (var stalactite in StalactitesToHide)
                {
                    stalactite.gameObject.SetActive(false);
                }
            }

            if (StalactitesToShow != null)
            {
                foreach (var stalactite in StalactitesToShow)
                {
                    stalactite.gameObject.SetActive(true);
                }
            }
        }

        public void ApplyOnStoneBalls()
        {
            if (StoneBallsToHide != null)
            {
                foreach (var stoneBall in StoneBallsToHide)
                {
                    stoneBall.gameObject.SetActive(false);
                }
            }

            if (StoneBallsToShow != null)
            {
                foreach (var stoneBall in StoneBallsToShow)
                {
                    stoneBall.gameObject.SetActive(true);
                }
            }
        }

        public void Apply()
        {
            ApplyOnTiles();
            ApplyOnStalactites();
            ApplyOnStoneBalls();
        }
    }
}