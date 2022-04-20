using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGFG.Core;
using AGFG.Inventory;

namespace AGFG.Activities
{
    public class ActivityMining : ActivityBase <AICCharacter>
    {
        [SerializeField] private ResourceData minableResource;
        [SerializeField] private float miningResistance;
        [SerializeField] private int quantityPerSuccess;
        [SerializeField] private int remainingQuantity;

        private float currentResistance;
        private int rewardThisFrame = 0;

        protected override void Start()
        {
            base.Start();

            currentResistance = miningResistance;
        }

        protected override void Update()
        {
            base.Update();

            TryApplyMining();
            GiveReward();
            rewardThisFrame = 0;
            CheckRemainingQuantity();
        }

        public override void BeginActivity(AICCharacter ai)
        {
            base.BeginActivity(ai);

            ai.characterRef.CharAnimation.StartMining();
        }

        protected void TryApplyMining()
        {
            if (CheckReady())
            {
                foreach (AICCharacter ai in AttachedCharacters)
                {
                    ApplyMining(ai.characterRef);
                }
            }
        }

        protected void ApplyMining(ControllableCharacter character)
        {
            DeduceMiningForce(character.MiningForce);
        }

        protected void DeduceMiningForce(float miningForce)
        {
            currentResistance -= miningForce * Time.deltaTime;
            CheckResistanceBroken();
        }

        protected void CheckResistanceBroken()
        {
            if(currentResistance <= 0)
            {
                currentResistance += miningResistance;
                PrepareReward();
            }

            if(miningResistance != 0 && currentResistance <= 0) // In case MiningForce is enough to break Resistance multiple time
            {
                CheckResistanceBroken();
            }
        }

        protected void PrepareReward()
        {
            int reward = quantityPerSuccess > remainingQuantity ? remainingQuantity : quantityPerSuccess;

            rewardThisFrame += reward;
            remainingQuantity -= reward;
        }

        protected void GiveReward()
        {
            if (rewardThisFrame > 0)
            {
                AGFGInventory.Instance.Resources.Add(minableResource, rewardThisFrame);
                SpawnFadingFeedback($"+{rewardThisFrame} {minableResource.ItemName}");
            }
        }

        protected void CheckRemainingQuantity()
        {
            if(remainingQuantity <= 0)
                DisableActivity();
        }
    }
}