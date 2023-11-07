using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapParalax : MonoBehaviour
{
    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _movementMultiplier = 0.5f;

    [Header("COMPONENTS")]
    [SerializeField]
    private Transform _centerOfTheMap;
    [SerializeField]
    private Transform _graphics;

    private GameManager _gameManager;

    private bool _gotInitialValues = false;
    private Vector2 _offset;
    private float _initialYaxis;
    private Vector2 _initialPosition;

    private void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void FixedUpdate()
    {
        if (_gameManager.Player == null) return;

        if (!_gotInitialValues)
        {
            _offset = _centerOfTheMap.position - _graphics.position;
            _initialYaxis = _graphics.position.y;

            _initialPosition = (Vector2)_gameManager.Player.transform.position - _offset;
            _gotInitialValues = true;
        }

        Vector2 newPosition = (Vector2)_gameManager.Player.transform.position - _offset;
        Vector2 difference = newPosition - _initialPosition;

        difference *= -_movementMultiplier;

        newPosition = _initialPosition + difference;
        newPosition = new Vector2(newPosition.x, _initialYaxis);

        _graphics.position = newPosition;
    }
}
