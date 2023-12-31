using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MapStepParent : MonoBehaviour
{
    [field: Header("CONFIGURATION")]
    [field: SerializeField]
    public string StepName { get; private set; }

    public abstract void Show();
    public abstract void Hide();
}