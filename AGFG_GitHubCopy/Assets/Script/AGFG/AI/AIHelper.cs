using UnityEngine;
using UnityEngine.AI;

namespace AGFG.AI
{
    public class AIHelper
    {
        static public float GetPathDistance(NavMeshPath path)
        {
            float length = 0;
            if (path.status != NavMeshPathStatus.PathInvalid && path.corners.Length >= 2)
            {
                for (int i = 1; i < path.corners.Length; ++i)
                {
                    length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }
            }

            return length;
        }
    }

}