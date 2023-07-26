using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MapCombatState : MapFiniteStateBase
{
    public override Map.FiniteState State => Map.FiniteState.Combat;

    private Toilet _toilet;
    private Map _map;
    private ScriptableMapConfiguration _configuration;
    private List<Enemy> _enemies = new List<Enemy>();

    private bool _canGoToNextStage = false;

    public override void Start(Map map)
    {
        _toilet = map.Toilet;
        _map = map;
        _map.OnMapIsReadyEvent.AddListener(OnMapIsReady);
        _configuration = _map.MapConfiguration;

        //_toilet.DisableIt();
    }

    public override void EnterState()
    {
        _map.Animator.Play(Map.MyAnimations.TurningOn.ToString());


        if (_configuration == null)
        {
            _canGoToNextStage = true;
        }
        else
        {
            _map.StartCoroutine(SpawnWaves());
        }
    }

    public override void OnExitState()
    {
    }

    public override void Update()
    {
        CheckIfCanChangeState();
    }

    private void OnMapIsReady()
    {
        _map.GameManager.Player.FreezePlayer(false);
    }

    private void CheckIfCanChangeState()
    {
        if (!_canGoToNextStage) return;

        if (_enemies.Count != _enemies.Count(e => e == null)) return;

        _map.ChangeState(Map.FiniteState.Wait);
    }

    private IEnumerator SpawnWaves(int index = 0)
    {
        ScriptableMapConfiguration.Wave wave = _configuration.Waves[index];
        foreach (var mob in wave.Mobs)
        {
            Enemy enemy = GameObject.Instantiate(mob.Enemy, GetEnemyPosition(mob.SpawnPosition), new Quaternion());
            enemy.transform.SetParent(_map.EnemiesParent);
            _enemies.Add(enemy);
        }


        index++;

        if (_configuration.Waves.Count == index)
        {
            _canGoToNextStage = true;
        }
        else
        {
            yield return new WaitForSeconds(wave.WaitToSpawn);
            _map.StartCoroutine(SpawnWaves(index));
        }
    }


    private Vector3 GetEnemyPosition(int position)
    {
        return _map.EnemySpawnPositions.FirstOrDefault(e => e.gameObject.name == $"Position ({position})").position;
    }
}
