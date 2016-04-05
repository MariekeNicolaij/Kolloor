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

        protected override void Start()
        {
                        type = AITypes.AirAI;
            base.Start();

            if (FlyingAnimation == null)
                FlyingAnimation = GetComponentInChildren<Animation>();
        }

        protected override void MoveForward()
        {
            if (!FlyingAnimation.isPlaying)
            {
                FlyingAnimation.wrapMode = WrapMode.Loop;
                FlyingAnimation.Play();
            }

            base.MoveForward();
        }
    }
}
