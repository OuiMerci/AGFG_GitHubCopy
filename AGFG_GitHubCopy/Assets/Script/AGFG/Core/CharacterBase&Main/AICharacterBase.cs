// AGFGCharacter should be used for the RPG Entity aspect of the object : game stats / gear / animation / colliders
// AICharacter should be used for the AI part of the character : decision making, Link to Nodecanvas, Navmesh movement...


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

namespace AGFG.Core
{
    public class AICharacterBase : Selectable
    {
        public ActivityBase CurrentActivity;
        public ActivityBase TargetActivity;
        public float SearchDistance;
        public float MaxDistanceFromOrigin;
        public NavMeshAgent NavAgent;
        public bool InterruptionTrigger;

        public override void HandleTarget(Selectable target)
        {
        }

        protected override void Awake()
        {
            base.Awake();

            NavAgent = GetComponentInParent<NavMeshAgent>();
        }

        public virtual void HandleActivityDetached(ActivityBase activity)
        {
            
        }

        public void SetCurrentActivity(ActivityBase act)
        {
            if(act != TargetActivity)
            {
                Debug.Log($"Target activity was supposed to be {TargetActivity} but currently setting to {act} ?!");
            }

            TargetActivity = null;
            CurrentActivity = act;
        }

        public void FaceTarget(Transform target)
        {
            Vector3 selfToTarget = target.transform.position - transform.position;
            transform.forward = selfToTarget.normalized;
        }

        public virtual void ResetAI()
        {
            if(NavAgent.enabled) NavAgent.SetDestination(transform.position);
            CurrentActivity?.DetachAI(this);
            CurrentActivity = null;
        }
    }
}