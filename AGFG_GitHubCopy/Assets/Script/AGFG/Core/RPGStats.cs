using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AGFG.Core
{
    [Serializable]
    public class RPGStats
    {
        public int MaxHP;
        public int BaseAttack;
        public int BaseArmor;
        public float AttackSpeed;
        public float AttackRange;
        public ElementalMultipliers ElementalResistance;
    }
}