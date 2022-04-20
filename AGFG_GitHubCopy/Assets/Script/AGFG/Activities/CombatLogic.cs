using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AGFG.Core;

namespace AGFG.Activities
{
    public class CombatLogicBase
    {
        protected ActivityFighting selfActivity;
        public ActivityFighting target;
        public float prevAttackTime = -50;
        public float nextAttackDelay = 0;
        public bool isFighting;
        protected float attackDelay; //get from curve and Attack speed
        protected CharacterAnimationBase animation;
        public RPGStats Stats => selfActivity.CharacterRef.GetStats();
        public bool AttackDelayPassed => prevAttackTime + attackDelay < Time.time;

        public CombatLogicBase(ActivityFighting self)
        {
            isFighting = false;
            selfActivity = self;
            animation = self.CharacterRef.Animation;
        }

        public void StartFighting(ActivityFighting targ)
        {
            isFighting = true;
            animation.StartFight();
            target = targ;
            RefreshAttackDelay();
        }

        public void RefreshAttackDelay()
        {
            attackDelay = CombatValuesHelper.Instance.GetAttackDelayFromSpeed(Stats.AttackSpeed);
        }

        public void StopFighting()
        {
            isFighting = false;
            prevAttackTime = Time.time;
            //selfActivity.CharacterRef.Animation.StartIdle();
            selfActivity.AIRef.ResetAI();
        }

        public bool IsTargetOutOfRange()
        {
            var distance = Vector3.Distance(target.transform.position, selfActivity.transform.position);
            return distance > Stats.AttackRange;
        }

        public void StartAttack()
        {
            selfActivity.AIRef.FaceTarget(target.transform);
            prevAttackTime = Time.time;
            animation.StartAttack();
        }

        public void ApplyDamage()
        {
            // method to apply buffs and elements

            Debug.Log($"{selfActivity.gameObject.name} attacks {target.gameObject.name} for {Stats.BaseAttack} dmg");
            target.TakeDamage(Stats.BaseAttack, Elements.None);
        }
    }

}