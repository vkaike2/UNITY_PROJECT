using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GunGarooIdleModel
{

    [field: Header("CONFIGURATIONS")]
    [field: SerializeField]
    public List<NextAttack> AttackList { get; private set; }

    public void Validate()
    {
        if (AttackList != null && AttackList.Count > 0) return;

        AttackList = new List<NextAttack>()
        {
            new NextAttack(PossibleAttacks.JumpToOtherSide, GunGaroo.Behaviour.JumpToOtherSide),
            new NextAttack(PossibleAttacks.Shoot, GunGaroo.Behaviour.Shoot),
            new NextAttack(PossibleAttacks.JumpToThePlayer, GunGaroo.Behaviour.JumpToThePlayer),
            new NextAttack(PossibleAttacks.SuperJump, GunGaroo.Behaviour.SuperJump),
        };
    }

    [Serializable]
    public class NextAttack
    {
        [HideInInspector]
        public string name;

        [field: SerializeField]
        public PossibleAttacks Attack { get; private set; }
        [field: SerializeField]
        public int Weight { get; private set; }
        [field: SerializeField]
        public MinMax CdwToStart { get; private set; }
        [field: SerializeField]
        public GunGaroo.Behaviour NextBehaviour { get; private set; }
        [field: SerializeField]
        public Sprite NextAttackIndication { get; private set; }

        public NextAttack(
            PossibleAttacks attack,
            GunGaroo.Behaviour nextBehaviour)
        {
            name = attack.ToString();
            Attack = attack;
            NextBehaviour = nextBehaviour;
        }
    }

    public enum PossibleAttacks
    {
        JumpToOtherSide,
        JumpToThePlayer,
        Shoot,
        SuperJump
    }
}
