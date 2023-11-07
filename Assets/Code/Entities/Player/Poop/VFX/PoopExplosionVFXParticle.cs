using System.Collections;
using UnityEngine;


public class PoopExplosionVFXParticle : MonoBehaviour
{
    private CircleCollider2D _hitboxCollider;

    public void SetCircleCollider(CircleCollider2D hitboxCollider)
    {
        _hitboxCollider = hitboxCollider;
    }

    public void Animation_TriggerExplosion()
    {
        if (_hitboxCollider == null) return;

        _hitboxCollider.enabled = true;
    }
}
