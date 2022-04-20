using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGFG.Core
{
    public class CharacterAnimationBase
    {
        protected Animator animator;

        int StartRunningHash;
        int StartIdleHash;
        int StartFightHash;
        int EndFightHash;
        int StartAttackHash;
        int StartDeathHash;

        public CharacterAnimationBase(Animator anim)
        {
            animator = anim;
            InitAnimator();
        }

        protected virtual void InitAnimator()
        {
            StartIdleHash = Animator.StringToHash("StartIdle");
            StartRunningHash = Animator.StringToHash("StartRunning");
            StartFightHash = Animator.StringToHash("StartFight");
            EndFightHash = Animator.StringToHash("EndFight");
            StartAttackHash = Animator.StringToHash("StartAttack");
            StartDeathHash = Animator.StringToHash("StartDeath");
        }

        public void StartRunning() => animator.SetTrigger(StartRunningHash);
        public void StartIdle() => animator.SetTrigger(StartIdleHash);
        public void StartFight() => animator.SetTrigger(StartFightHash);
        public void EndFight() => animator.SetTrigger(EndFightHash);
        public void StartAttack() => animator.SetTrigger(StartAttackHash);
        public void StartDeath() => animator.SetTrigger(StartDeathHash);
    }
}