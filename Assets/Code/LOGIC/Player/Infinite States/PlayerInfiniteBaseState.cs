using System.Collections;
using UnityEngine;


public abstract class PlayerInfiniteBaseState
{
    protected Player _player;
    protected Rigidbody2D _rigidbody2D;

    public virtual void Start(Player player)
    {
        _player = player;
        _rigidbody2D = _player.GetComponent<Rigidbody2D>();
    }

    public abstract void Update();
}
