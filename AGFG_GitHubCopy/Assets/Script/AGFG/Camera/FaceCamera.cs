using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGFG.AGFGCamera
{
    public class FaceCamera : MonoBehaviour
    {
        [SerializeField] private float refreshDelay;
        private float lastRefresh;

        private void LateUpdate()
        {
            if(lastRefresh + refreshDelay < Time.time)
            {
                lastRefresh = Time.time;
                UpdateOrientation();
            }
        }

        private void UpdateOrientation()
        {
            var toCam = Camera.main.transform.position - transform.position;
            transform.forward = -toCam.normalized;
        }
    }
}