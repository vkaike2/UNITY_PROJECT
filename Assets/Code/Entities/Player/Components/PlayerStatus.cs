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
        [field: Header("PREFAB")]
        [field: SerializeField]
        public StatusMonobehaviourAttribute<FartProjectile> Projectile { get; private set; }

        [field: Space]
        [field: SerializeField]
        public EntityAttribute Damage { get; private set; }
        [field: SerializeField]
        public EntityAttribute AreaOfEffect { get; set; }
        [field: SerializeField]
        public EntityAttribute Duration { get; private set; }
        [field: SerializeField]
        public EntityAttribute ImpulseForce { get; private set; }
        [field: SerializeField]
        public EntityAttribute Cooldown { get; private set; } // 0.5

        [field: Space]
        [field: SerializeField]
        public StatusIntAttribute AmountOfParticle { get; set; }

    }

    [Serializable]
    public class PoopStatus
    {
        [field: Header("PREFAB")]
        [field: SerializeField]
        public StatusMonobehaviourAttribute<PoopProjectile> Projectile { get; private set; }


        [field: Space]
        [field: SerializeField]
        public EntityAttribute Damage { get; private set; }

        [field: SerializeField]
        public EntityAttribute AreaOfEffect { get; set; }

        [field: SerializeField]
        public EntityAttribute Duration { get; private set; }

        [field: SerializeField]
        public EntityAttribute CdwToPoop { get; private set; }


        [field: Space]
        [field: SerializeField]
        public StatusFloatAttribute MaximumVelocity { get; private set; } // 10;

        [field: SerializeField]
        [field: Tooltip("How many secconds it takes to go from 0 to maximum velocity")]
        public StatusFloatAttribute VelocityTimer { get; private set; } // 1;
    }

    [Serializable]
    public class EntityAttribute
    {
        [Header("CONFIGURATION")]
        [SerializeField]
        private float _minimumValue = 1;

        [field: Header("ATTRIBUTE")]
        [field: SerializeField]
        public StatusFloatAttribute Base { get; private set; }
        [field: SerializeField]
        public StatusFloatAttribute Multiplier { get; private set; }
        [field: SerializeField]
        public StatusFloatAttribute Increased { get; private set; }

        public float Get()
        {
            var value = (Base.Get() * Increased.Get()) * Multiplier.Get();

            if(value < _minimumValue)
            {
                return _minimumValue;
            }

            return value;
        }
    }
}
