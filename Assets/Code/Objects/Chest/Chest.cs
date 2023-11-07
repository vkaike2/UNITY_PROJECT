using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class Chest : MouseInteractable
{
    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _cdwBetweenEachItem = 0.3f;
    [SerializeField]
    private Transform _itemSpawnPoint;
    [SerializeField]
    private Pool _pool;

    [Header("COMPONENTS")]
    [SerializeField]
    private Animator _animator;

    private const float WAIT_BEFORE_SPAWN = 0.5F;
    private bool _isOpen = false;

    private void OnValidate()
    {
        if (_pool == null) return;
        _pool.OnValidate();
    }

    public override void ChangeAnimationOnItemOver(bool isMouseOver)
    {
        if (_isOpen) return;

        if (isMouseOver)
        {
            _animator.Play(MyAnimations.Selected.ToString());
        }
        else
        {
            _animator.Play(MyAnimations.Idle.ToString());
        }
    }

    public override void InteractWith(CustomMouse mouse)
    {
        //if (!PlayerIsInRange()) return;
        if (_isOpen) return;

        _isOpen = true;
        _animator.Play(MyAnimations.Open.ToString());

        StartCoroutine(SpawEveryItem());
    }

    private void SpawnItem(ItemData itemData)
    {
        ItemDrop itemDrop = Instantiate(itemData.Item.ItemDrop, _itemSpawnPoint);
        itemDrop.ItemData = itemData;
        itemDrop.DropItem(ItemDrop.DropConfiguration.Chest);
    }

    private IEnumerator SpawEveryItem()
    {
        yield return new WaitForSeconds(WAIT_BEFORE_SPAWN);

        for (int i = 0; i < _pool.NumberOfitems; i++)
        {
            Drop drop = _pool.GetDrop();

            SpawnItem(new ItemData(Guid.NewGuid(), drop.Item));
            yield return new WaitForSeconds(_cdwBetweenEachItem);
        }
    }

    private enum MyAnimations
    {
        Idle,
        Selected,
        Open
    }

    [Serializable]
    private class Pool
    {
        [SerializeField]
        private MinMax _numberOfItems;
        [Space(2)]
        [SerializeField]
        private List<Drop> _possibleDrops;

        public int NumberOfitems => _numberOfItems.GetRandomInt();

        public void OnValidate()
        {
            if (_possibleDrops == null || _possibleDrops.Count == 0) return;

            foreach (var drop in _possibleDrops)
            {
                if (drop.Item == null)
                {
                    drop.name = "empty";
                    continue;
                }

                string active = drop.IsActive ? "" : "[off]";

                drop.name = $"{active}{drop.Item.name}_{drop.Weight}";
            }
        }

        public Drop GetDrop()
        {
            List<Drop> possibleActiveDrops = _possibleDrops.Where(e => e.IsActive).ToList();

            int totalWeight = possibleActiveDrops.Sum(e => e.Weight);
            int randomWeight = UnityEngine.Random.Range(1, totalWeight + 1);
            int currentWeight = 0;

            foreach (var possibleDrop in possibleActiveDrops)
            {
                currentWeight += possibleDrop.Weight;

                if (currentWeight >= randomWeight)
                {
                    return possibleDrop;
                }
            }

            return _possibleDrops.OrderBy(e => e.Weight).FirstOrDefault();
        }

    }

    [Serializable]
    private class Drop
    {
        [HideInInspector]
        public string name;

        [SerializeField]
        private ScriptableItem _item;
        [Space]
        [SerializeField]
        private int _weight;

        [field: SerializeField]
        public bool IsActive { get; private set; } = true;

        public ScriptableItem Item => _item;
        public int Weight => _weight;
    }
}
