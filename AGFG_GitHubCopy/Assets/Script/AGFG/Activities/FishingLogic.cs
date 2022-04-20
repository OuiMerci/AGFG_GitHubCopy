using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AGFG.Core;
using AGFG.Inventory;

namespace AGFG.Activities
{
    public class FishingLogic : IHelpable
    {
        public ActivityFishing SelfActivity;
        public ControllableCharacter Fisher;
        public ActivityHelp ActHelp;
        public CCharacterAnimation Animation;
        public FishData Fish;
        public int StrengthBoost;
        public Coroutine FishingCoroutineRef;

        public event System.Action<bool> HelpUpdatedEvent;

        public RPGStats Stats => Fisher.GetStats();

        public FishingLogic(FishData fishData, ControllableCharacter fisherData, ActivityFishing self)
        {
            Fish = fishData;
            Fisher = fisherData;
            SelfActivity = self;
            Animation = Fisher.CharAnimation;
            ActHelp = Fisher.GetComponentInChildren<ActivityHelp>();

            Debug.Log("Fish : " + fishData.ItemName);

            StartFishing();
        }

        private void StartFishing()
        {
            Animation.StartFishing();
            FishingCoroutineRef = SelfActivity.StartCoroutine(FishingCoroutine());
        }

        private void Restart()
        {
            ActHelp.Unlink(this);
            StrengthBoost = 0;
            StartFishing();
        }

        private float GetBiteTime()
        {
            float result = Fisher.FishAttractivenessRatio * SelfActivity.GenerateWaitTime();
            result = Mathf.Max(result, ActivityFishing.MIN_FISHING_WAIT);
            Debug.Log("bite delay : " + result);
            return result;
        }

        private float GetReelTime()
        {
            //Add random time
            float random = Random.Range(0, ActivityFishing.REEL_TIME_MAXRANDOM);
            float result = Fish.BaseReelTime + random;

            // Apply Time reduction
            float reelTimeReduction = ActivityFishing.GetReelTimeReductionRatio(strengthDiff: Fisher.FishingForce - Fish.Strength);
            result *= reelTimeReduction;
            result = Mathf.Max(result, ActivityFishing.MIN_REELING_TIME);
            Debug.Log("reel delay : " + result);
            return result;
        }

        private bool IsFisherStrongEnough()
        {
            int totalStrength = Fisher.FishingForce + StrengthBoost;

            Debug.Log($"fisher str : {Fisher.FishingForce} - fish force : {Fish.Strength} - boos : {StrengthBoost}");
            return totalStrength >= Fish.Strength;
        }

        private IEnumerator FishingCoroutine()
        {
            Debug.Log("Start waiting for a fish");
            yield return new WaitForSeconds(GetBiteTime());

            Debug.Log("fish has bitten !");
            Animation.StartReeling();
            CheckHelpNeeded();
            yield return new WaitForSeconds(GetReelTime());

            Debug.Log("Is the fish caught ?");
            bool success = IsFisherStrongEnough();
            Debug.Log("Fishing result : " + success);

            if(success)
            {
                AGFGInventory.Instance.Fish.Add(Fish, 1);
                SelfActivity.SpawnFadingFeedback($"Received : {Fish.ItemName} !");
            }
            else
            {
                SelfActivity.SpawnFadingFeedback("Fish escaped !");
            }

            Restart(); // <- link to animation system
        }

        private void CheckHelpNeeded()
        {
            if(IsFisherStrongEnough() == false)
            {
                ActHelp.Link(this);
            }
        }

        public void StartHelp(AICCharacter ai)
        {
            StrengthBoost += ai.characterRef.FishingForce;
            ai.characterRef.CharAnimation.StartReeling();

            Debug.Log("Received help, new boost = " + StrengthBoost);
            HelpUpdatedEvent?.Invoke(IsFisherStrongEnough());
        }

        public void EndHelp(AICCharacter ai)
        {
            StrengthBoost -= ai.characterRef.FishingForce;
            Debug.Log("Lost help, new boost = " + StrengthBoost);
            HelpUpdatedEvent?.Invoke(IsFisherStrongEnough());
        }

        public void StopFishing()
        {
            ActHelp.Unlink(this);

            if(FishingCoroutineRef != null)
                SelfActivity.StopCoroutine(FishingCoroutineRef);
        }
    }

}