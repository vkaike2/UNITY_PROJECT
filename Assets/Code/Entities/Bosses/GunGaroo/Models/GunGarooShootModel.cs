using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class GunGarooShootModel
{
    [field: Header("COMPONENTS")]
    [field: SerializeField]
    public Transform ShootPosition { get; private set; }
    [field: SerializeField]
    public GunGarooBullet BulletPrefab { get; private set; }

    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public float CdwToLeaveState { get; private set; }
    [field: SerializeField]
    public float BulletSpeed { get; private set; }
}
