using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;


[Serializable]
public class PossibleDrop
{
    [SerializeField]
    private float _cdwBetweenEachItem = 0.3f;
    [field: Space]
    [field: SerializeField]
    public List<Pool> ItemPools { get; private set; }

    public List<DropContent> GetDrops()
    {
        List<DropContent> drops = new List<DropContent>(); ;

        foreach (Pool pool in ItemPools)
        {
            for (int i = 0; i < pool.NumberOfItems; i++)
            {
                DropContent drop = pool.GetDrop();
                if (drop == null) continue;
                drops.Add(drop);
            }
        }
        return drops;
    }

    public IEnumerator SpawnEveryItem(Vector2 spawnPosition, Transform itemContainer)
    {
        List<DropContent> dropContents = GetDrops();

        foreach (DropContent drop in dropContents)
        {
            SpawnItem(new ItemData(Guid.NewGuid(), drop.Item), spawnPosition, itemContainer);
            yield return new WaitForSeconds(_cdwBetweenEachItem);
        }
    }

    private void SpawnItem(ItemData itemData, Vector2 spawnPosition, Transform itemContainer)
    {
        ItemDrop itemDrop = GameObject.Instantiate(itemData.Item.ItemDrop, spawnPosition, Quaternion.identity);
        itemDrop.ItemData = itemData;
        itemDrop.transform.parent = itemContainer;
        itemDrop.DropItem(ItemDrop.DropConfiguration.Chest);
    }
}

[Serializable]
public class Pool
{
    public string name;

    [SerializeField]
    private MinMax _numberOfItems;
    [Space(2)]
    [SerializeField]
    private List<Drop> _drops;

    public int NumberOfItems => _numberOfItems.GetRandomInt();

    public void Validate()
    {
        if (_drops == null || _drops.Count == 0) return;

        foreach (var drop in _drops)
        {
            drop.UpdateWeight();
        }

        int totalWeight = _drops.Where(e => e.IsActive).Sum(e => e.Weight);
        
        foreach (var drop in _drops)
        {
            drop.Validate(totalWeight);
        }
    }

    public DropContent GetDrop()
    {
        List<Drop> possibleDrops = _drops.Where(e => e.IsActive).ToList();

        if (possibleDrops == null || !possibleDrops.Any()) return null;

        int totalWeight = possibleDrops.Sum(e => e.Weight);
        int randomWeight = UnityEngine.Random.Range(1, totalWeight + 1);
        int currentWeight = 0;

        foreach (var possibleDrop in possibleDrops)
        {
            currentWeight += possibleDrop.Weight;

            if (currentWeight >= randomWeight)
            {
                return possibleDrop.GetContent();
            }
        }

        return _drops.OrderBy(e => e.Weight).FirstOrDefault().GetContent();
    }
}

[Serializable]
public class Drop
{
    [HideInInspector]
    public string name;

    [field: SerializeField]
    public int Weight { get; private set; }
    [field: SerializeField]
    public bool IsActive { get; private set; }

    [field: Space]
    [field: SerializeField]
    public List<DropContent> DropContents { get; private set; }

    public void UpdateWeight()
    {
        if (Weight == 0)
        {
            Weight = 1;
        }
    }
    public void Validate(int totalWeight)
    {
        if (IsActive)
        {
            float percentage = (Weight / (float)totalWeight) * 100;
            name = $"[on] {percentage:n2}% chance";
        }
        else
        {
            name = "[off]";
        }


        if (DropContents == null || DropContents.Count == 0) return;

        foreach (var dropContent in DropContents)
        {
            dropContent.UpdateWeight();
        }
        int totalContentWeight = DropContents.Where(e => e.IsActive).Sum(e => e.Weight);
        foreach (var dropContent in DropContents)
        {
            dropContent.Validate(totalContentWeight);
        }
    }

    public DropContent GetContent()
    {
        if (DropContents == null || !DropContents.Any())
        {
            return null;
        }

        List<DropContent> possibleActiveDrops = DropContents.Where(e => e.IsActive).ToList();

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

        return DropContents.FirstOrDefault();
    }
}

[Serializable]
public class DropContent
{
    [field: HideInInspector]
    public string name;

    [field: SerializeField]
    public ScriptableItem Item { get; private set; }
    [field: Space]
    [field: SerializeField]
    public int Weight { get; private set; }

    [field: SerializeField]
    public bool IsActive { get; private set; } = true;

    public void UpdateWeight()
    {
        if (Weight == 0)
        {
            Weight = 1;
        }
    }

    public void Validate(int totalWeight)
    {
        if (Item == null)
        {
            name = "Empty";
        }
        else if (IsActive)
        {
            float percentage = (Weight / (float)totalWeight) * 100;
            name = $"{Item.name} {percentage:n2}% chance";
        }
        else
        {
            name = $"[off]{Item.name}";
        }

    }

}
