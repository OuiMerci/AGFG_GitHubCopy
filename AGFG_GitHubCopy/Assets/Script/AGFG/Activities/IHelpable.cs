using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGFG.Core;

namespace AGFG.Activities
{
    public interface IHelpable
    {
        void StartHelp(AICCharacter ai);
        void EndHelp(AICCharacter ai);

        event System.Action<bool> HelpUpdatedEvent;
    }
}