// AGFGCharacter should be used for the RPG Entity aspect of the object : game stats / gear / animation / colliders
// AICharacter should be used for the AI part of the character : decision making, Navmesh movement...

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AGFG.Core
{
    public class ControllableCharacter : CharacterBase
    {
        static public HashSet<ControllableCharacter> s_CharacterList {get; set;} = new HashSet<ControllableCharacter>();
        public CCharacterAnimation CharAnimation => (CCharacterAnimation)Animation;
        public float LastActionTime { get; private set;} = -10; // Initialize to allow instant action
        public float FishAttractivenessRatio => 1 - fishAttractiveness;

        [SerializeField] private float fishAttractiveness;
        public float MiningDelay;
        public float MiningForce;
        public int FishingForce;

        protected override void Awake()
        {
            base.Awake();

            Animation = new CCharacterAnimation(GetComponentInChildren<Animator>());
            s_CharacterList.Add(this);
        }

        public bool ValidateActionDelay(float actionDelay)
        {
            if(actionDelay + LastActionTime < Time.time)
            {
                LastActionTime = Time.time;
                return true;
            }

            return false;
        }

        public void HandleActivityDetached(ActivityBase activity)
        {
            Animation.StartIdle();
        }
    }
}