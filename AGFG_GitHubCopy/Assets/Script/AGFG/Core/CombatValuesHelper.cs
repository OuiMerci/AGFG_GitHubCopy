using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGFG.Core
{
    public class CombatValuesHelper : MonoBehaviour
    {
        #region Singleton
        private static CombatValuesHelper _instance;
        public static CombatValuesHelper Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance != null)
                    Debug.LogError("[CombatValuesHelper] Trying to set singleton instance when instance already exists !");
                else
                    _instance = value;
            }
        }
        #endregion

        [SerializeField] private AnimationCurve AttackDelayFromSpeed;

        private void Awake()
        {
            _instance = this;
        }

        public float GetAttackDelayFromSpeed(float atkSpeed)
        {
            float clampedSpeed = Mathf.Clamp(atkSpeed, 0, 5);
            float delay = AttackDelayFromSpeed.Evaluate(atkSpeed);
            return delay;
        }
    }

}