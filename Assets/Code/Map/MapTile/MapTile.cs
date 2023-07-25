using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    [Header("CONFIGURATIONS")]
    [SerializeField]
    private int _iterationNumber = 0;
    [SerializeField]
    private List<PossibleIterations> _possibleIterations;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        AssignRandomSprite();
    }

    private void OnValidate()
    {
        if (_possibleIterations == null) return;
        if (_possibleIterations.Count == 0) return;

        for (int i = 0; i < _possibleIterations.Count; i++)
        {
            _possibleIterations[i].SetNameName(i);
        }
    }

    private void AssignRandomSprite()
    {
        if (_possibleIterations == null || _possibleIterations.Count == 0) return;

        _spriteRenderer.sprite = _possibleIterations[_iterationNumber].GetRandomSprite();
    }


    [Serializable]
    public class PossibleIterations
    {
        [HideInInspector]
        public string name;

        [field: SerializeField]
        public List<PossibleTiles> Tiles { get; private set; }

        public void SetNameName(int number)
        {
            name = $"iteration - {number}";
        }

        public Sprite GetRandomSprite()
        {
            int totalWeight = Tiles.Select(e => e.Weight).Sum();
            int randomValue = UnityEngine.Random.Range(0, totalWeight);
            int count = 0;
            foreach (var tile in Tiles)
            {
                count += tile.Weight;

                if (count >= randomValue) return tile.Sprite;
            }

            return Tiles.OrderByDescending(e => e.Weight).Select(e => e.Sprite).FirstOrDefault();
        }
    }

    [Serializable]
    public class PossibleTiles
    {
        public string name;

        [field: SerializeField]
        public Sprite Sprite { get; set; }
        [field: SerializeField]
        public int Weight { get; set; }
    }
}
