// AGFGCharacter should be used for the RPG Entity aspect of the object : game stats / gear / animation / colliders
// AICharacter should be used for the AI part of the character : decision making, Link to Nodecanvas, Navmesh movement...

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

namespace AGFG.Core
{
    public class AICCharacter : AICharacterBase
    {
        static public HashSet<AICCharacter> s_AICharacterList { get; set; } = new HashSet<AICCharacter>();

        public ActivityBase[] Activities { get; private set; }

        public ControllableCharacter characterRef;
        public bool controlledByPlayer;

        public bool GoToRequestedTrigger;
        public Vector3 RequestedDestination;
        public float DelayToAutoMode;
        public AIPriorities AIPriorities;

        protected override void Awake()
        {
            base.Awake();
            AICCharacter.s_AICharacterList.Add(this);

            NavAgent = GetComponent<NavMeshAgent>();
            characterRef = GetComponent<ControllableCharacter>();
            Activities = GetComponentsInChildren<ActivityBase>();
            AllowsMultipleSelection = true;
        }

        public override void HandleTarget(Selectable target)
        {
            if (target is ActivityBase activity)
            {
                SendToActivity(activity);
            }
            else if (target is AICCharacter ai)
            {
                foreach(ActivityBase act in ai.Activities)
                {
                    if(act.ValidForCCharacterInteraction)
                    {
                        SendToActivity(act);
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("IDK what to do with this");
            }
        }

        public void SendToActivity(ActivityBase activity)
        {
            controlledByPlayer = true;
            
            Debug.Log("Set activity and set as controlled");

            if(CurrentActivity != activity)
            {
                CurrentActivity?.DetachAI(this);
                TargetActivity = activity;
                InterruptionTrigger = true;
            }
        }

        public override void HandleActivityDetached(ActivityBase activity)
        {
            characterRef.HandleActivityDetached(activity);

            CurrentActivity = null;
            InterruptionTrigger = true;
            controlledByPlayer = false;
        }

        public void HandleGoToRequest(Vector3 dest)
        {
            ResetAI();
            
            RequestedDestination = dest;
            GoToRequestedTrigger = true;
            InterruptionTrigger = true;
        }

        public override void ResetAI()
        {
            base.ResetAI();
            controlledByPlayer = false;
        }

        public T TryGetActivity<T>() where T : ActivityBase
        {
            foreach (ActivityBase act in Activities)
            {
                if (act is T validAct)
                {
                    return validAct;
                }
            }

            return null;
        }
    }

    [System.Serializable]
    public class AIPriorities
    {
        public float Mining;
        public float Fighting;
        public float Fishing;
    }
}