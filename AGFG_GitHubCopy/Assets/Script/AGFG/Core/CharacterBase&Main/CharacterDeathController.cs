using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace AGFG.Core
{
    public class CharacterDeathController : MonoBehaviour
    {
        public event Action OnDeathStart;
        public event Action OnDeathEnd;

        [Title("At death Start")]
        [SerializeField] protected List<Behaviour> ComponentsToDisable;
        [SerializeField] protected List<ActivityBase>  ActivitiesToDeactivate;

        [Title("At death End")]
        [SerializeField] protected List<Behaviour> ComponentsToDisableAfterAnim;

        public void StartDeath(CharacterAnimationBase animation)
        {
            OnDeathStart?.Invoke();

            ComponentsToDisable.ForEach(c => c.enabled = false);
            ActivitiesToDeactivate.ForEach(a => a.DisableActivity());
            animation.StartDeath();
        }

        public void OnAnimationComplete()
        {
            ComponentsToDisableAfterAnim.ForEach(c => c.enabled = false);
            OnDeathEnd?.Invoke();
        }
    }

}