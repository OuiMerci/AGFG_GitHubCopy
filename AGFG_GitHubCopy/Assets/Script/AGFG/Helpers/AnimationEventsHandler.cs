using UnityEngine;
using AGFG.Activities;
using AGFG.Core;

namespace AGFG.Helpers
{
    public class AnimationEventsHandler : MonoBehaviour
    {
        [SerializeField] ActivityFighting activityFighting;
        [SerializeField] CharacterDeathController deathController;

        private void Start()
        {
            deathController = GetComponentInParent<CharacterDeathController>();
        }

        public void ApplyDamage()
        {
            activityFighting.CombatLogic.ApplyDamage();
        }

        public void EndDeath()
        {
            deathController.OnAnimationComplete();
        }
    }

}