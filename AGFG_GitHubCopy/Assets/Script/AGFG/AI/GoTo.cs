using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;
using AGFG.Core;

namespace AGFG.AI
{
	public enum Animation
    {
		None,
		Idle,
		Fight
    }

	[Description("Go to a specified destination")]
	public class GoTo : ActionTask{

		[RequiredField]
		public BBParameter<NavMeshAgent> NavAgent;
		[RequiredField]
		public BBParameter<Vector3> Destination;
		public BBParameter<CharacterBase> EntityRef;
		public Animation AnimationOnStop;
		public bool ResetDestinationOnInterruption = true;
		private bool UseAttackRangeAsStopDistance;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit(){
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute(){
			NavAgent.value.SetDestination(Destination.value);
			EntityRef?.value?.Animation.StartRunning();
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate(){
			if (IsDestinationReached())
            {
				//ApplyAnimation();
				EndAction(true);
			}
			else if(Destination.value.x != NavAgent.value.destination.x
										|| Destination.value.z != NavAgent.value.destination.z)
			{
				//Debug.Log("resetting dest");
				NavAgent.value.SetDestination(Destination.value);
			}
		}

        protected override void OnStop()
        {
            base.OnStop();
			ApplyAnimation();

			if(ResetDestinationOnInterruption)
				NavAgent.value.SetDestination(agent.transform.position);
		}

        private void ApplyAnimation()
        {
			switch(AnimationOnStop)
            {
				case Animation.Idle: EntityRef?.value?.Animation.StartIdle(); break;
				case Animation.Fight: EntityRef?.value?.Animation.StartFight(); break;
			}
        }
		private bool IsDestinationReached()
        {
			var agent = NavAgent.value;

			if (!agent.pathPending)
			{
				if (agent.remainingDistance <= agent.stoppingDistance)
				{
					if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}