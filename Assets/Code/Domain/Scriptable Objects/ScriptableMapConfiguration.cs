using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MapConfig", menuName = "ScriptableObjects/MapConfiguration")]
public class ScriptableMapConfiguration : ScriptableObject
{

    [SerializeField]
    private List<Wave> _waves;

    public List<Wave> Waves => _waves;

    private void OnValidate()
    {
        if (_waves == null) return;

        for (int i = 0; i < _waves.Count; i++)
        {
            _waves[i].name = $"{i} wave";

            _waves[i].ValidateMobs();
        }
    }


    [Serializable]
    public class Wave
    {
        [HideInInspector]
        public string name;

        [SerializeField]
        private float _waitToSpaw;
        [SerializeField]
        private List<Mob> _mobs;

        public List<Mob> Mobs => _mobs;
        public float WaitToSpawn => _waitToSpaw;

        public void ValidateMobs()
        {
            if(_mobs == null) return;

            foreach (var mob in _mobs)
            {
                if(mob.Enemy == null) continue;

                mob.name = mob.Enemy.name;
            }
        }

    }

    [Serializable]
    public class Mob
    {
        [HideInInspector]
        public string name;

        [SerializeField]
        private Enemy _enemy;
        [SerializeField]
        private int _spawnPosition;


        public Enemy Enemy => _enemy;
        public int SpawnPosition => _spawnPosition;
    }
}
