using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGFG.Core;
using AGFG.Activities;
using NodeCanvas.Framework;

namespace AGFG.AI
{
    public class SearchFightingActivity : SearchActivity<ActivityFighting>
    {
        public BBParameter<ActivityFighting> AgentFighter;

        protected override bool CheckCustomCondition(ActivityBase activity)
        {
            if (activity != AgentFighter.value && activity is ActivityFighting af)
            {
                return AgentFighter.value.Tags.IsATarget(af.Tags);
            }

            return false;
        }
    }

}