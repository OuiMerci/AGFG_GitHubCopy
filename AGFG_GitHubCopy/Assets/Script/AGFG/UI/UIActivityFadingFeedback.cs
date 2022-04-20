// TODO : Use a pool to optimize this !

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGFG.UI
{
    public class UIActivityFadingFeedback : MonoBehaviour
    {
        public float LifeTime = 2f;
        public float MoveSpeed;

        private float spawnTime;
        private float previousY;

        // Start is called before the first frame update
        void Start()
        {
            spawnTime = Time.time;
            previousY = transform.position.y;
        }

        // Update is called once per frame
        void Update()
        {
            MoveUp();
            CheckLifeTime();
        }

        private void MoveUp()
        {
            float newY = previousY + MoveSpeed * Time.deltaTime;
            previousY = newY;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        private void CheckLifeTime()
        {
            if (LifeTime + spawnTime < Time.time)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}