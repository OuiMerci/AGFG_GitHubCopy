using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AGFG.Core;

namespace AGFG.Activities
{
    public class ActivityFighting : ActivityBase<AICharacterBase>
    {
        [SerializeField] protected Image hpImage;
        [SerializeField] protected FighterTagSystem tagSystem;

        private CombatLogicBase _combatLogic;
        private bool isDetachingAll;
        public override bool ValidForCCharacterInteraction => tagSystem.HasAnyTag(FighterTagSystem.FighterTags.Enemy);
        public override bool RequiresGoTo => false;
        public CombatLogicBase CombatLogic => GetCombatLogic();
        public CharacterBase CharacterRef { get; protected set;}
        public AICharacterBase AIRef { get; protected set; }
        public FighterTagSystem Tags => tagSystem;

        private CombatLogicBase GetCombatLogic()
        {
            _combatLogic = _combatLogic ?? new CombatLogicBase(this);
            return _combatLogic;
        }

        protected override void Awake()
        {
            base.Awake();
            CharacterRef = GetComponentInParent<CharacterBase>();
            AIRef = GetComponentInParent<AICharacterBase>();
        }

        public override void BeginActivity(AICharacterBase ai)
        {
            base.BeginActivity(ai);
            Debug.Log("Begin Fighting");
            
            var af = ai.GetComponentInChildren<ActivityFighting>();
            if(af)
            {
                af.StartAttack(this);
            }
        }

        public void StartAttack(ActivityFighting target)
        {
            CombatLogic.StartFighting(target);
        }

        public void StopFighting()
        {
            CombatLogic?.StopFighting();
        }

        public void TakeDamage(int damage, Elements element)
        {
            if (CharacterRef.IsAlive == false) return;

            damage = ApplyElementalMultiplier(damage, element);
            damage = ApplyArmorReduction(damage);
            CharacterRef.TakeDamage(damage);
            DisplayDamages(damage, element);
        }

        private void DisplayDamages(float damage, Elements element)
        {
            // TODO: add <color> tag depending element

            damage *= -1;
            string str = $"{damage} !";

            SpawnFadingFeedback(str);

            float healthRatio = (float)CharacterRef.CurrentHP / (float)CharacterRef.GetStats().MaxHP;
            hpImage.fillAmount = healthRatio;
        }

        protected virtual int ApplyElementalMultiplier(int damage, Elements element)
        {
            int resistance = CharacterRef.GetStats().ElementalResistance.GetResistance(element);
            var multiplier = (resistance / 100) + 1;

            return damage * multiplier;
        }

        protected virtual int ApplyArmorReduction(int damage)
        {
            return damage - CharacterRef.GetStats().BaseArmor;
        }

        public override void DetachAI(AICharacterBase aiBase)
        {
            base.DetachAI(aiBase);
            var aiChar = (AICCharacter)aiBase;
            var af = aiChar.TryGetActivity<ActivityFighting>();

            if(af != null && af.CombatLogic.isFighting)
            {
                af.StopFighting();
            }
        }

        public override void DisableActivity()
        {
            StopFighting();
            base.DisableActivity();
        }
    }
}