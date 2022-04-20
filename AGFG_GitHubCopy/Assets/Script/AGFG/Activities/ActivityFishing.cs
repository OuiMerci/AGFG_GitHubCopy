using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGFG.Core;
using AGFG.Inventory;
using PMExtensions;

namespace AGFG.Activities
{
    public class ActivityFishing : ActivityBase<AICCharacter>
    {
        public const float MIN_FISHING_WAIT = 1;
        public const float WAIT_TIME_MAXRANDOM = 5;
        public const float MIN_REELING_TIME = 0.5f;
        public const float REEL_TIME_MAXRANDOM = 3;
        public const float REEL_TIME_REDUCTION = 0.5f;
        public const float SUPER_REEL_TIME_REDUCTION = .9f;
        public const float REEL_TIME_REDUCTION_THRESHOLD = 5;
        public const float SUPER_REEL_TIME_REDUCTION_THRESHOLD = 15;

        [SerializeField] private WeightedRandomList<FishData> fishAndProbabilities;
        [SerializeField] private float _baseWaitTime;

        public List<FishingLogic> ActiveFishingInstances = new List<FishingLogic>();

        public float BaseWaitTime => _baseWaitTime;

        public override void BeginActivity(AICCharacter ai)
        {
            base.BeginActivity(ai);
            ai.characterRef.CharAnimation.StartFishing();
            Debug.Log("Begin Fishing");

            var fishingLogic = new FishingLogic(fishAndProbabilities.Pick(), ai.characterRef, this);
            ActiveFishingInstances.Add(fishingLogic);
        }


        public float GenerateWaitTime()
        {
            float rand = Random.Range(0, WAIT_TIME_MAXRANDOM);
            return rand + BaseWaitTime;
        }

        public static float GetReelTimeReductionRatio(int strengthDiff)
        {
            if (strengthDiff >= SUPER_REEL_TIME_REDUCTION_THRESHOLD) return SUPER_REEL_TIME_REDUCTION;
            else if (strengthDiff >= REEL_TIME_REDUCTION_THRESHOLD) return REEL_TIME_REDUCTION;
            else return 1;
        }

        public override void DetachAI(AICharacterBase aiBase)
        {
            base.DetachAI(aiBase);
            var aiCC = (AICCharacter)aiBase;

            var relatedLogic = ActiveFishingInstances.Find(f => f.Fisher == aiCC.characterRef);
            ActiveFishingInstances.Remove(relatedLogic);
            relatedLogic.StopFishing();
        }
    }
}