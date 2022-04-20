using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using System;

namespace AGFG.Core
{
    public abstract class CharacterBase : MonoBehaviour
    {
        [SerializeField] protected RPGStats Stats;
        [ShowInInspector] [ReadOnly] public int CurrentHP { get; protected set; }

        public virtual CharacterAnimationBase Animation { get; protected set; }
        public virtual CharacterDeathController DeathController { get; protected set; }
        public RPGStats GetStats() => Stats;
        public bool IsAlive => CurrentHP > 0;

        protected virtual void Awake()
        {
            Animation = new CCharacterAnimation(GetComponentInChildren<Animator>());
            DeathController = GetComponent <CharacterDeathController>();
            CurrentHP = Stats.MaxHP;
        }

        public void TakeDamage(int dmg)
        {
            CurrentHP -= dmg;
            CheckDeath();
        }

        protected void CheckDeath()
        {
            if(CurrentHP <= 0)
            {
                CurrentHP = 0;
                DeathController.StartDeath(Animation);
            }
        }
    }

    public abstract class AGFGEntity<T> : CharacterBase where T : CharacterAnimationBase
    {
    }

    public enum RPGEntityTags
    {
        MainCharacter,
        ControllableCharacter,
        Enemy,
        BossEnemy,
    }
}