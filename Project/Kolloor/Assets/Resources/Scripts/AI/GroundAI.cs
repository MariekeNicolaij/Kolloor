using UnityEngine;
using System.Collections;
using AI.States;

namespace AI
{
    public class GroundAI : BaseAI
    {
        public bool HopOn = true;
        [Range(1, 10)]
        public float Hop = 5;

        public bool StopRandom = true;

        [Range(0, 1)]
        public float MinRandomStopTime = 0;
        [Range(1, 2)]
        public float MaxRandomStopTime = 1;
        [Range(1, 10)]
        public float WaitRaid = 2;

        private bool stop = false;

        private float waitTime = 0;
        private float timeToWait = 0;

        private bool groundContact = false;

        protected override void Start()
        {
            base.Start();
            timeToWait = Random.Range(1, WaitRaid);
        }

        public override void MoveForward()
        {
            if (StopRandom)
            {
                if (waitTime <= 0)
                {
                    timeToWait -= Time.smoothDeltaTime;

                    if (timeToWait <= 0 && !stop)
                    {
                        stop = true;
                    }
                    else
                    {
                        base.MoveForward();

                        if (groundContact)
                        {
                            if (stop)
                            {
                                waitTime = Random.Range(MinWaitTime, MaxWaitTime);
                                stop = false;
                            }
                            else if (HopOn)
                                rigidBody.AddForce(Vector3.up * (Hop * 10));
                        }
                    }
                }
                else
                {
                    waitTime -= Time.smoothDeltaTime;

                    if (timeToWait <= 0)
                        timeToWait = Random.Range(1, WaitRaid);
                }
            }
            else
            {
                base.MoveForward();

                if (HopOn && groundContact)
                    rigidBody.AddForce(Vector3.up * (Hop * 10));
            }
        }

        public override void OnCollisionEnter(Collision other)
        {
            base.OnCollisionEnter(other);
            if (other.collider is TerrainCollider)
            {
                groundContact = true;
            }
        }

        public override void OnCollisionExit(Collision other)
        {
            base.OnCollisionExit(other);
            if (other.collider is TerrainCollider)
            {
                groundContact = false;
            }
        }

        public override void HelpPlayer(bool help)
        {
            base.HelpPlayer(help);
        }
    }
}