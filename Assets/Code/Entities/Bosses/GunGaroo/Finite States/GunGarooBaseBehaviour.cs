using System.Collections;
using UnityEngine;


public abstract class GunGarooBaseBehaviour
{
    public bool IsStarted { get; private set; } = false;
    public abstract GunGaroo.Behaviour Behaviour { get; }
    
    protected GunGaroo _gunGaroo;
    protected Rigidbody2D _rigidBody2D;

    public virtual void Start(GunGaroo gunGaroo)
    {
        _gunGaroo = gunGaroo;
        IsStarted = true;

        _rigidBody2D = gunGaroo.GetComponent<Rigidbody2D>();
    }

    public abstract void Update();

    public abstract void OnEnterBehaviour();
    public abstract void OnExitBehaviour();
}
