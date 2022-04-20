using System.Collections.Generic;
using UnityEngine;

namespace AGFG.Activities
{
    [System.Serializable]
    public class FighterTagSystem
    {
        public enum FighterTags
        {
            MainCharacter,
            ControllableCharacter,
            Enemy,
            BossEnemy,
        }

        [SerializeField] protected List<FighterTags> SelfTags;
        [SerializeField] protected List<FighterTags> TargetTags;

        public bool HasAllTags(params FighterTags[] tags)
        {
            foreach (FighterTags tag in tags)
            {
                if (SelfTags.Contains(tag) == false)
                    return false;
            }

            return true;
        }

        public bool HasAnyTag(params FighterTags[] tags)
        {
            foreach (FighterTags tag in tags)
            {
                if (SelfTags.Contains(tag))
                    return true;
            }

            return false;
        }

        public bool IsATarget(FighterTagSystem fighter)
        {
            return fighter.HasAnyTag(TargetTags.ToArray());
        }
    }
}