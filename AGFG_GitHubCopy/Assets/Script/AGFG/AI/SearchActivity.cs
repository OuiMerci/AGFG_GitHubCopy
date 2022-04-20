using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;
using AGFG.Core;
using AGFG.Activities;

namespace AGFG.AI
{
	[Description("Search for the specified Activity type")]
	public class SearchActivity<T> : ConditionTask
	{
		public BBParameter<float> searchDistanceBB;
		public BBParameter<ActivityBase> bestActivity;

		protected virtual bool CheckCustomCondition(ActivityBase activity) => true;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit(){
			return null;
		}

		//Called whenever the condition gets enabled.
		protected override void OnEnable(){
			var result = SearchNearbyActivity();
			bestActivity.value = result;
		}

		//Called whenever the condition gets disabled.
		protected override void OnDisable(){
			
		}

        //Called once per frame while the condition is active.
        //Return whether the condition is success or failure.
        protected override bool OnCheck()
        {
            return bestActivity.value != null;
        }

        protected ActivityBase SearchNearbyActivity()
        {
			ActivityBase bestResult = null;
			float bestResultDistance = float.MaxValue;
			ActivityBase[] activityArray = AreaManager.Instance.TryGetRelevantActivityArray<T>();

			for(int i = 0; i < activityArray.Length; i++)
            {
				var activity = activityArray[i];
				if(activity.IsActive == false) continue;
				float distance = float.MaxValue;

				NavMeshPath path = new NavMeshPath();

				if(NavMesh.CalculatePath(agent.transform.position, activity.transform.position, NavMesh.AllAreas, path))
				{
					distance = AIHelper.GetPathDistance(path);
                }

				if(distance <= searchDistanceBB.value && CheckCustomCondition(activity))
                {
					if(bestResult == null || bestResultDistance > distance)
                    {
						bestResult = activity;
						bestResultDistance = distance;
                    }
                }
            }

			return bestResult;
        }
	}
}