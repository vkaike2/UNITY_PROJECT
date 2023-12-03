using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfig", menuName = "ScriptableObjects/MapConfiguration")]
public class ScriptableMapConfiguration : ScriptableObject
{
    [field: Header("CONFIGURATION")]
    [field: SerializeField]
    public int Id { get; private set; }

    [field: Header("INNER STAGES")]
    [field: SerializeField]
    public List<InnerStage> InnerStages { get; private set; }

    private void OnValidate()
    {
        ValidateActions();
    }

    private void ValidateActions()
    {
        if (InnerStages == null) return;

        float timerCount = 0;
        foreach (InnerStage innerStage in InnerStages)
        {
            if (innerStage.Actions == null) continue;
            timerCount += innerStage.GetTimerForAllMyActions();
            innerStage.Validate(timerCount);
        }
    }

    [Serializable]
    public class InnerStage
    {
        public string name;

        [field: SerializeField]
        public List<MapAction> Actions { get; private set; }

        public float GetTimerForAllMyActions()
        {
            return Actions.Sum(e => e.Timer);
        }

        public void Validate(float timeToMe)
        {
            name = name.Split("-> ").Last();

            name = $"{timeToMe.SecondsToTime()} -> {name}";

            float timerCount = 0;
            foreach (var action in Actions)
            {
                if (action == null) continue;

                timerCount += action.Timer;
                action.Validate(timerCount);
            }
        }
    }

    [Serializable]
    public class MapAction
    {
        [HideInInspector]
        public string name;

        [field: Header("CONFIGURATIONS")]
        [field: SerializeField]
        public ActionType Type { get; private set; }

        [field: Header("END/START CRITERIA")]
        [field: SerializeField]
        public float Timer { get; private set; }
        [field: SerializeField]
        public bool WaitButtonSignal { get; private set; } = false;
        [field: SerializeField]
        public bool WaitForAllMonstersToBeKilled { get; private set; } = false;

        [field: Header("MONSTER")]
        [field: SerializeField]
        public MapEnemy Enemy { get; private set; }

        [field: Header("CHEST")]
        [field: SerializeField]
        public MapChest MapChest { get; private set; }

        [field: Header("MAP")]
        [field: SerializeField]
        public MapChange MapChange { get; private set; }

        public void Validate(float timeToMe)
        {
            name = $"{(timeToMe.SecondsToTime())} -> {Type.ToString()}";


            if(Timer == 0) 
            {
                Timer = 1;
            }

            switch (Type)
            {
                case ActionType.Monster:
                    name += $" - {Enemy?.ScriptableEnemy?.name}";
                    break;
                case ActionType.Chest:
                    break;
                case ActionType.ChangeMap:
                    break;
            }
        }


        public enum ActionType
        {
            Monster,
            Chest,
            ChangeMap
        }
    }

    [Serializable]
    public class MapEnemy
    {
        [field: Header("PREFAB")]
        [field: SerializeField]
        public ScriptableEnemy ScriptableEnemy { get; private set; }

        [field: Header("POSITION")]
        [field: SerializeField]
        public bool UseRandomPosition { get; set; }
        [field: SerializeField]
        public int SpawnPositionId { get; private set; }

        [field: Header("DROP")]
        [field: SerializeField]
        public ScriptablePossibleDrop ScriptablePossibleDrop { get; set; }
    }

    [Serializable]
    public class MapChest
    {
        [field: Header("DROP")]
        [field: SerializeField]
        public ScriptablePossibleDrop ScriptablePossibleDrop { get; set; }
    }

    [Serializable]
    public class MapChange
    {
        [field: SerializeField]
        public int ChangeId { get; private set; }
        [SerializeField]
        [TextArea]
        private string _description;
    }
}
