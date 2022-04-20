using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGFG.Core
{
    public class CCharacterAnimation : CharacterAnimationBase
    {
        int StartMiningHash;
        int StartFishingHash;
        int StartReelingHash;
        int StartFightingHash;

        public CCharacterAnimation(Animator anim) : base (anim)
        {
            animator = anim;
            InitAnimator();
        }

        protected override void InitAnimator()
        {
            base.InitAnimator();

            StartMiningHash = Animator.StringToHash("StartMining");
            StartFishingHash = Animator.StringToHash("StartFishing");
            StartReelingHash = Animator.StringToHash("StartReeling");
            StartFightingHash = Animator.StringToHash("StartFighting");
        }

        public void StartMining() => animator.SetTrigger(StartMiningHash);
        public void StartFishing() => animator.SetTrigger(StartFishingHash);
        public void StartReeling() => animator.SetTrigger(StartReelingHash);
        public void StartFighting() => animator.SetTrigger(StartFightingHash);
    }
}