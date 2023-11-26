using UnityEngine;

public class CameraConfiner : MonoBehaviour
{
    [field: Header("CONFIGURATION")]
    [field: SerializeField]
    public float CameraSize { get; private set; }
    public Collider2D Collider { get; private set; }

    private void Awake()
    {
        Collider = GetComponent<Collider2D>();
    }
}
