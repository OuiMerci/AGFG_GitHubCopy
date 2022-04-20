using NodeCanvas.Framework;
using ParadoxNotion.Design;
using AGFG.Core;
using UnityEngine;


namespace AGFG.AI
{
	[Description("Execute the specified Activity")]
	public class ExecuteActivity : ActionTask {

		[RequiredField] public BBParameter<ActivityBase> TargetActivityBB;
		public BBParameter<AICharacterBase> ai;
		public bool leaveOnExecute;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit(){
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute(){
			bool validType = TargetActivityBB.value.IsValidAIType(ai.value);
			TargetActivityBB.value?.BeginActivityIfTypeValid(ai.value);

			if(leaveOnExecute)
				EndAction(validType);
		}

		//Called when the task is disabled.
		protected override void OnStop(){
			//TargetActivityBB.value?.DetachAI(ai.value);
			Debug.Log(name + " detached " + ai.value);
		}
	}
}