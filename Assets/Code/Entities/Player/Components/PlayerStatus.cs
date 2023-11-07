using System;
using UnityEngine;

public class PlayerStatus : HealthStatus
{
    [field: Header("MOVEMENT")]
    [field: SerializeField]
    public StatusFloatAttribute MovementSpeed { get; private set; } // 7
    [field: SerializeField]
    public StatusFloatAttribute JumpForce { get; private set; } //  8.5

    [field: Space(5)]

    [field: Header("ATTACKS")]
    [field: SerializeField]
    public FartStatus Fart { get; private set; }  

    [field: SerializeField]
    public PoopStatus Poop { get; private set; }


    [Serializable]
    public class FartStatus
    {
        [field: SerializeField]
        public StatusFloatAttribute Damage { get; private set; } // 0.1

        [field: SerializeField]
        public StatusMonobehaviourAttribute<FartProjectile> Projectile { get; private set; }

        [field: SerializeField]
        public StatusFloatAttribute Duration { get; private set; } // 1
        [field: SerializeField]
        public StatusFloatAttribute ImpulseForce { get; private set; } // 50
        [field: SerializeField]
        public StatusFloatAttribute Cooldown { get; private set; } // 0.5
        [field: SerializeField]
        public StatusIntAttribute AmountOfParticle { get; set; }

        [field: SerializeField]
        public StatusFloatAttribute AreaOfEffect { get; set; }

    }

    [Serializable]
    public class PoopStatus
    {
        [field: SerializeField]
        public StatusFloatAttribute Damage { get; private set; } // 2.5
        [field: SerializeField]
        public StatusFloatAttribute DamageMultiplier { get; private set; } // 1

        [field: SerializeField]
        public StatusMonobehaviourAttribute<PoopProjectile> Projectile { get; private set; }

        [field: SerializeField]
        public StatusIntAttribute AreaOfEffect { get; set; }

        [field: SerializeField]
        public StatusFloatAttribute Duration { get; private set; } // 2

        [field: SerializeField]
        public StatusFloatAttribute MaximumVelocity { get; private set; } // 10;
        [field: SerializeField]
        [field: Tooltip("How many secconds it takes to go from 0 to maximum velocity")]
        public StatusFloatAttribute VelocityTimer { get; private set; } // 1;
        [field: SerializeField]
        public StatusFloatAttribute CdwToPoop { get; private set; } // 3f;

        public float GetDamage()
        {
            return Damage.Get() * DamageMultiplier.Get();
        }
    }
}
