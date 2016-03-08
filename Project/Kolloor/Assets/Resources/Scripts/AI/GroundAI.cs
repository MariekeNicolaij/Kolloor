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

        protected bool agentOn = true;
        protected NavMeshAgent agent;

        private bool stop = false;

        private float waitTime = 0;
        private float timeToWait = 0;

        private bool groundContact = false;

        protected override void Start()
        {
            type = AITypes.GroundAI;
            base.Start();
            timeToWait = Random.Range(1, WaitRaid);

            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
                agent = gameObject.AddComponent<NavMeshAgent>();
        }

        protected override void Update()
        {
            base.Update();
        }

        public override void Move()
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
                        MoveForward();
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
                MoveForward();
            }
        }

        protected override void MoveForward()
        {
            if (HopOn)
            {
                if (groundContact && agentOn)
                {
                    SetAgent(false);
                    rigidBody.AddForce(Vector3.up * (Hop * 10));
                    base.MoveForward();
                }
                else if (groundContact)
                {
                    SetAgent(true);
                    agent.SetDestination(lookAt);
                }
                else
                {
                    base.MoveForward();
                }
            }
            else
            {
                agent.SetDestination(lookAt);
            }
        }

        /// <summary>
        /// sets agent on or of
        /// </summary>
        /// <param name="onOrOff"> true == on && false = off </param>
        private void SetAgent(bool onOrOff)
        {
            agent.enabled = onOrOff;
            agentOn = onOrOff;
        }

        //public override void LookAt(Vector3 objectToLookTo)
        //{
        //    base.LookAt(objectToLookTo);
        //    agent.Move()
        //}

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