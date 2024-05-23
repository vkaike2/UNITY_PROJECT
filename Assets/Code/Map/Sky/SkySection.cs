using UnityEngine;

public class SkySection : MonoBehaviour
{
    [SerializeField]
    private Transform _leftLimit;
    [SerializeField]
    private Transform _rightLimit;


    public bool ImInsideSkyLimit(Vector2 position)
    {
        return position.x < _rightLimit.position.x && position.x > _leftLimit.position.x;
    }
}