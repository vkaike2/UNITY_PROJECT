using CustomAttributes;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using static ScriptableMapConfiguration;


[CreateAssetMenu(fileName = "MapConfig", menuName = "ScriptableObjects/MapConfiguration")]
public class ScriptableMapConfiguration : ScriptableObject
{
    [Header("TIMER CONFIGURATION")]
    [SerializeField, ReadOnly]
    private float _fullTime;

    [Space]

    [SerializeField]
    private float _movementBuffer = 2f;
    [field: Space]
    [field: SerializeField]
    public float EnfOfSmallStage { get; private set; }
    [field: SerializeField]
    public float EnfOfMediumStage { get; private set; }

    [field: Header("WAVES")]
    [field: SerializeField]
    public List<Wave> Waves { get; private set; }

    public float GetFullTime()
    {
        _fullTime = Waves.Sum(e => e.GetFullTime());

        return _fullTime;
    }

    private void OnValidate()
    {
        if (Waves == null) return;

        GetFullTime();

        float waitToSpawnCount = 0;

        for (int waveIndex = 0; waveIndex < Waves.Count; waveIndex++)
        {
            Waves[waveIndex].name = CreateWaveName(waveIndex);

            waitToSpawnCount += Waves[waveIndex].WaitToStartSpawningWave;

            SetMobsStage(waveIndex);

            var fullTimeUntillLastWave = Waves.Take(waveIndex).Sum(e => e.GetFullTime());
            Waves[waveIndex].SetMobName(fullTimeUntillLastWave + Waves[waveIndex].WaitToStartSpawningWave);
        }
    }

    private string CreateWaveName(int waveIndex)
    {
        string waveDuration = "";

        //-> duration
        if (waveIndex == 0)
        {
            waveDuration += $"[{ConvertSecondsToMinutes(0)}, ";
        }
        waveDuration += $"{ConvertSecondsToMinutes((int)Waves[waveIndex].GetFullTime())}]";

        //-> wave number 
        string waveNumber = (waveIndex + 1).ToString().PadLeft(2, '0');

        //-> mobs inside wave
        string mobsInWave = $"[{Waves[waveIndex].GetMobNames()}]";

        return $"{waveDuration} {waveNumber} wave -> {mobsInWave}";
    }

    private void SetMobsStage(int waveIndex)
    {
        Wave wave = Waves[waveIndex];

        float currentTime = 0;
        if (waveIndex > 0)
        {
            currentTime = Waves.Take(waveIndex).Sum(e => e.GetFullTime());
        }
        else
        {

        }

        foreach (Mob mob in wave.Mobs)
        {

            mob.mapStage = SetMobStage(currentTime);


            if (mob.UsePartentTimer)
            {
                currentTime += wave.WaitToSpawnNextEnemyFromThisWave;
            }
            else
            {
                currentTime += mob.CdwToSpawnNextEnemy;
            }
        }
    }

    private string SetMobStage(float time)
    {
        if (time < (EnfOfSmallStage + _movementBuffer))
        {
            return MapStage.Small.ToString();
        }
        else if (time < (EnfOfMediumStage + _movementBuffer))
        {
            return MapStage.Medium.ToString();
        }
        else if (time >= (EnfOfMediumStage + _movementBuffer))
        {
            return MapStage.Large.ToString();
        }

        return "lol, this is an error I guess";
    }

    public static string ConvertSecondsToMinutes(int seconds)
    {
        int minutes = seconds / 60;
        int remainingSeconds = seconds % 60;
        return $"{minutes:00}:{remainingSeconds:00}";
    }

    [Serializable]
    public class Wave
    {
        [HideInInspector]
        public string name;

        [field: SerializeField]
        public float WaitToStartSpawningWave { get; private set; }
        [field: SerializeField]
        public float WaitToSpawnNextEnemyFromThisWave { get; set; }
        [field: SerializeField]
        public List<Mob> Mobs { get; private set; }


        public float GetFullTime()
        {
            float countTime = 0;

            foreach (var mob in Mobs)
            {
                countTime += GetMobTimeToSpawn(mob);
            }

            return countTime + WaitToStartSpawningWave;
        }


        public void SetMobName(float waveTime)
        {
            if (Mobs == null) return;


            foreach (var mob in Mobs)
            {
                if (mob.Enemy == null) continue;

                mob.name = $"({Mobs.IndexOf(mob)}) {ConvertSecondsToMinutes((int)waveTime)} [{mob.mapStage}] {mob.Enemy.name}";

                waveTime += GetMobTimeToSpawn(mob);
            }
        }

        private float GetMobTimeToSpawn(Mob mob)
        {
            if (mob.UsePartentTimer)
            {
                mob.CdwToSpawnNextEnemy = WaitToSpawnNextEnemyFromThisWave;
                return WaitToSpawnNextEnemyFromThisWave;
            }
            else
            {
                return mob.CdwToSpawnNextEnemy;
            }
        }

        public string GetMobNames()
        {
            return string.Join(", ", Mobs.Select(e => e.Enemy?.name.ToLower()));
        }

        public void SetMapStageInfo(string mapStage)
        {
            if (Mobs == null) return;

            foreach (var mob in Mobs)
            {
                mob.mapStage = mapStage;
            }
        }
    }

    [Serializable]
    public class Mob
    {
        [HideInInspector]
        public string name;
        [HideInInspector]
        public string mapStage;

        [field: Header("PREFAB")]
        [field: SerializeField]
        public Enemy Enemy { get; private set; }

        [field: Header("POSITION")]
        [field: SerializeField]
        public bool UseRandomPosition { get; set; }
        [field: SerializeField]
        public int SpawnPositionId { get; private set; }

        [field: Header("ENEMY TYPE")]
        [field: SerializeField]
        public EnemySpawnPosition.SpawnType SpawnType { get; private set; }

        [field: Header("SPAWN")]
        [field: SerializeField]
        public bool UsePartentTimer { get; set; }
        [field: SerializeField]
        public float CdwToSpawnNextEnemy { get; set; }
    }

    public enum MapStage
    {
        Small,
        Medium,
        Large,
    }
}
