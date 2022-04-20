using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AGFG.Core;

namespace AGFG.Activities
{
    public class ActivityHelp : ActivityBase<AICCharacter>
    {
        public override bool ValidForCCharacterInteraction => linkedObject != null;

        private IHelpable linkedObject;
        [SerializeField] private Image helpWarning;
        [SerializeField] private Image helpComplete;

        public override void BeginActivity(AICCharacter ai)
        {
            base.BeginActivity(ai);

            if (linkedObject != null)
            {
                ai.TryGetActivity<ActivityHelp>()?.Link(linkedObject);
                linkedObject.StartHelp(ai);
            }
            else
            {
                DetachAI(ai);
            }
        }

        public void Link(IHelpable helpable)
        {
            linkedObject = helpable;
            helpWarning.enabled = true;
            helpable.HelpUpdatedEvent += UpdateFeedback;
        }

        public void Unlink(IHelpable helpable)
        {
            if(helpable == linkedObject)
            {
                DetachAll();
                linkedObject = null;
                UpdateFeedback(false);
                helpable.HelpUpdatedEvent -= UpdateFeedback;
            }
        }

        public override void DetachAI(AICharacterBase aiBase)
        {
            base.DetachAI(aiBase);
            var aiChar = (AICCharacter)aiBase;
            linkedObject.EndHelp(aiChar);
            aiChar.TryGetActivity<ActivityHelp>()?.Unlink(linkedObject);
        }

        private void UpdateFeedback(bool complete)
        {
            if(linkedObject == null)
            {
                helpComplete.enabled = false;
                helpWarning.enabled = false;
            }
            else
            {
                helpComplete.enabled = complete;
                helpWarning.enabled = !complete;
            }
        }
    }
}