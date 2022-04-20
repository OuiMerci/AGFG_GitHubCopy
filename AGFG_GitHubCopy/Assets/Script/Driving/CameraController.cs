using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGFGDriving
{
    public class CameraController : MonoBehaviour
    {
        public Vector3 lookOffset;
        public GameObject Car;
        public GameObject Attach;

        void LateUpdate()
        {
            transform.position = Attach.transform.position;
            transform.LookAt(Car.transform.position + lookOffset);
        }
    }
}