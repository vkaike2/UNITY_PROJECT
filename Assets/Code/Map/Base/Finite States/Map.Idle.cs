﻿using System.Collections;
using UnityEngine;

public partial class BaseMap : MonoBehaviour
{
    /// <summary>
    ///  Description -> this is the initial state, where tha map will be waiting for 
    ///                 player to be spawned to start the combat state;
    /// </summary>
    protected class Idle : MapFiniteStateBase
    {
        public override FiniteState State => FiniteState.Idle;

        private BaseMap _map;
        private Toilet _toilet;
        private GameManager _gameManager;

        public override void Start(BaseMap map)
        {
            _map = map;
            _toilet = map.Toilet;
            _gameManager = map.GameManager;
        }

        public override void EnterState()
        {
            _toilet.SetState(Toilet.FiniteState.SpawnPlayer);
            _map.StartCoroutine(WaitForPlayerToBeSpawned());
        }

        public override void OnExitState()
        {
        }

       
        public override void Update()
        {
        }


        private IEnumerator WaitForPlayerToBeSpawned()
        {
            while (_gameManager.Player == null)
            {
                yield return new WaitForFixedUpdate();
            }

            _map.ChangeState(FiniteState.Combat);
        }
    }

}
