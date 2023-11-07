using System.Collections;
using UnityEngine;


public class StaticFartProjectile : FartProjectile
{
    public override void SetInitialValues(Vector2 velocity, Player player)
    {
        BaseBehavioyrForFart(player);
    }

}
