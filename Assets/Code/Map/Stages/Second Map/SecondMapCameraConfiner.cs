using System.Collections.Generic;
using UnityEngine;

public class SecondMapCameraConfiner : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private ScriptableMapEvents _mapEvents;
    [Space]
    [SerializeField]
    private List<CameraConfiner> _cameraConfiners;
}