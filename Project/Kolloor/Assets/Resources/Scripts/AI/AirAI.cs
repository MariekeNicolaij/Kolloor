using UnityEngine;
using System.Collections;

namespace AI
{
    public class AirAI : BaseAI
    {
        /// <summary>
        /// The max hight above the ground
        /// </summary>
        public float MaxFlighHight = 20;
        /// <summary>
        /// Tne minimal height above the ground
        /// </summary>
        public float MinFlighHight = 15;

        public Animation FlyingAnimation;
        private bool HasAnimation = false;

        private bool startAnimation = false;
        private bool coroutine = false;
        private int MaxTimeTillStart = 10;

        protected override void Start()
        {
            type = AITypes.AirAI;
            base.Start();

            if (FlyingAnimation == null)
            {
                FlyingAnimation = GetComponentInChildren<Animation>();
                if (FlyingAnimation != null)
                    HasAnimation = true;
            }

        }

        protected override void MoveForward()
        {
            if (HasAnimation && !FlyingAnimation.isPlaying && startAnimation)
            {
                FlyingAnimation.wrapMode = WrapMode.Loop;
                FlyingAnimation.Play();
            }else if (!coroutine)
            {
                StartCoroutine(AnimationCounter());
                coroutine = true;
            }

            base.MoveForward();
        }

        private IEnumerator AnimationCounter()
        {
            float wait = Random.Range(0, MaxTimeTillStart);

            float i = 0;

            while (i < wait)
            {
                i += Time.deltaTime;
                yield return null;
            }
            startAnimation = true;
        }
    }
}
