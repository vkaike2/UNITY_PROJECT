using System.Collections;
using UnityEngine;


public class DefaultPoopProjectile : PoopProjectile
{
    protected override void HandleLayerColision()
    {
        Destroy(this.gameObject);
    }
}
