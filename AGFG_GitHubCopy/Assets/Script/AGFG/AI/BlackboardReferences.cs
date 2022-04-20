using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AGFG.Core;
using AGFG.Activities;

namespace AGFG.AI
{
    public class BlackboardReferences : MonoBehaviour
    {
        // RPG Entity References
        public CharacterBase CharBaseRef;

        // AIBase References
        public AICharacterBase AiBaseRef;
        public ActivityBase TargetActivity { get => AiBaseRef.TargetActivity; set { AiBaseRef.TargetActivity = value;} }
        public ActivityBase CurrentActivity => AiBaseRef.CurrentActivity;
        public NavMeshAgent NavAgent => AiBaseRef.NavAgent;
        public bool NeedsActivity => AiBaseRef.TargetActivity == null && AiBaseRef.CurrentActivity == null;
        public float SearchDistance => AiBaseRef.SearchDistance;
        public float GetMaxDistanceFromOrigin => AiBaseRef.MaxDistanceFromOrigin;
        public bool HasTargetActivity => AiBaseRef.TargetActivity != null;
        public bool IsLoopComplete => AiBaseRef.TargetActivity == null && AiBaseRef.CurrentActivity != null;

        // AI Char References
        public AICCharacter AiCharRef => (AICCharacter)AiBaseRef;
        public bool InterruptionTrigger { get => AiCharRef.InterruptionTrigger; set { AiCharRef.InterruptionTrigger = value;} }
        public bool GoToRequestedTrigger { get => AiCharRef.GoToRequestedTrigger; set { AiCharRef.GoToRequestedTrigger = value; } }
        public Vector3 RequestedDestination => AiCharRef.RequestedDestination;
        public float DelayToAutoMode => AiCharRef.DelayToAutoMode;
        public float FightingPriority => AiCharRef.AIPriorities.Fighting;
        public float FishingPriority => AiCharRef.AIPriorities.Fishing;
        public float MiningPriority => AiCharRef.AIPriorities.Mining;
        public bool IsControlledByPlayer => AiCharRef.controlledByPlayer;

        // Activity References
        public bool TargetActivityRequiresGoTo => TargetActivity ? TargetActivity.RequiresGoTo : true;

        // ActivityFighting References
        public ActivityFighting ActivityFightingRef;
        public CombatLogicBase CombatLogic => ActivityFightingRef.CombatLogic;
        public bool IsFighting => CombatLogic.isFighting;
        public bool AttackDelayPassed => CombatLogic.AttackDelayPassed;
        public bool IsTargetOutOfRange => CombatLogic.IsTargetOutOfRange();

        public void StartAttackAnimation()
        {
            Debug.Log("popo my name is " + gameObject.name);
            CombatLogic.StartAttack();
        }

        public ActivityBase GetCurrentOrTargetActivity()
        {
            return CurrentActivity ?? TargetActivity;
        }
    }
}