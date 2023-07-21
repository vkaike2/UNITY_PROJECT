using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInRangeCheck : MonoBehaviour
{
    private Player _player;

    public bool PlayerIsInRange => _player != null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player == null) return;

        _player = player;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player == null) return;

        _player = null;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player == null) return;
        _player = player;
    }
}
