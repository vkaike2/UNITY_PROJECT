using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static ScriptableItem;

public class EquipItemVFX : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private TMP_Text _label;

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private List<ItemConfiguration> _configuration;

    private void OnValidate()
    {
        if (_configuration == null) return;
        
        foreach (var config in _configuration)
        {
            config.name = config.ItemTarget.ToString();
        }
    }

    public void Equip(string description, ItemTarget itemTarget)
    {
        ItemConfiguration config = _configuration.FirstOrDefault(e => e.ItemTarget == itemTarget);

        _label.color = config.Color;
        _label.text = description;
    }

    [Serializable]
    public class ItemConfiguration
    {
        [HideInInspector]
        public string name;

        [field: SerializeField]
        public ItemTarget ItemTarget { get; private set; }
        [field: SerializeField]
        public Color Color { get; private set; }
    }
}
