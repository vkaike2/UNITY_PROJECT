using UnityEngine;
using UnityEngine.Events;

public class BasicCollider : MonoBehaviour
{
    [field: SerializeField]
    public OnCollisionEnterEvent OnCollision { get; set; } = new OnCollisionEnterEvent();

    private void OnCollisionEnter2D(Collision2D col)
    {
        OnCollision.Invoke(col);
    }

    public class OnCollisionEnterEvent : UnityEvent<Collision2D> { }
}