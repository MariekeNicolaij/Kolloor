using UnityEngine;
using System.Collections;
using AI.States;
using System.Linq;

namespace AI
{
    public class GroundAI : GroundWaterBaseAI
    {
        #region hoper
        public bool HopOn = true;

        public Animation HopAnimation;
        #endregion

        protected override void Start()
        {
            type = AITypes.GroundAI;
            base.Start();

            if (HopOn)
                if (!HopAnimation)
                    HopAnimation = GetComponent<Animation>();
        }

        protected override void Update()
        {
            base.Update();

            if (transform.position.y < aiManager.Water.transform.position.y)
                Respawn();
        }

        protected override void MoveForward()
        {
            if (HopOn && !HopAnimation.isPlaying)
            {
                HopAnimation.wrapMode = WrapMode.Loop;
                HopAnimation.Play();
            }

            base.MoveForward();
        }

        public override void PickUp()
        {
            base.PickUp();

            if (HopOn)
                if (HopAnimation.isPlaying)
                    HopAnimation.wrapMode = WrapMode.Once;
        }
    }
}