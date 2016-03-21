using UnityEngine;
using System.Collections;
using AI.States;
using System.Linq;

namespace AI
{
    public class GroundAI : BaseAI
    {
        #region hoper
        public bool HopOn = true;

        public Animation HopAnimation;
        #endregion

        #region Random AI Stopper
        public bool StopRandom = true;

        [Range(0, 1)]
        public float MinRandomStopTime = 0;
        [Range(1, 2)]
        public float MaxRandomStopTime = 1;
        [Range(1, 10)]
        public float WaitRaid = 2;

        private float waitFor = 0;
        private float timeTillWait = 0;
        private float counter = 0;

        private bool stop = false;
        #endregion

        public NavMeshAgent agent;

        private bool EnableAgent = false;

        [HideInInspector]
        public bool destinationset = false;

        protected override void Start()
        {
            type = AITypes.GroundAI;
            base.Start();
            timeTillWait = Random.Range(1, WaitRaid);

            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
                agent = gameObject.AddComponent<NavMeshAgent>();

            agent.speed = MovementSpeed;

            if (HopOn)
                if (!HopAnimation)
                    HopAnimation = GetComponent<Animation>();
        }

        protected override void Update()
        {
            base.Update();

            if (EnableAgent)
            {
                if (Collisions.Any(item => item.GetComponent<Terrain>()))
                {
                    agent.enabled = true;
                    stateManager.SwitchToDefault();
                    EnableAgent = false;
                }
            }
        }

        public void Stop()
        {
            agent.Stop();

            if (HopAnimation.isPlaying)
                HopAnimation.Stop();
        }

        public override void Move()
        {
            if (StopRandom)
            {
                if (stop)
                {
                    counter += Time.smoothDeltaTime;
                    if (counter >= waitFor)
                    {
                        stop = false;
                        counter = 0;
                        timeTillWait = Random.Range(1, WaitRaid);
                    }
                    agent.Stop();

                    if (HopAnimation.isPlaying)
                        HopAnimation.wrapMode = WrapMode.Once;
                }
                else
                {

                    counter += Time.smoothDeltaTime;
                    if (counter >= timeTillWait)
                    {
                        stop = true;
                        counter = 0;
                        waitFor = Random.Range(MinRandomStopTime, MaxRandomStopTime);
                    }

                    MoveForward();
                }
            }
            else
            {
                MoveForward();
            }
        }


        protected override void MoveForward()
        {
            if (HopOn && !HopAnimation.isPlaying)
            {
                HopAnimation.wrapMode = WrapMode.Loop;
                HopAnimation.Play();
            }

            Debug.DrawLine(transform.position, lookAt, Color.blue);

            Debug.Log(agent.destination);

            if (!destinationset)
            {
                destinationset = true;
                agent.SetDestination(lookAt);
            }
            else
                agent.Resume();
        }

        public override void LookAt(Vector3 placeToLookTo)
        {
            lookAt = placeToLookTo;
            //if (agent.hasPath)
            //    transform.LookAt(agent.nextPosition);
        }

        public override void LookAt(GameObject objectToLookTo)
        {
            LookAt(objectToLookTo.transform.position);
        }

        public override void HelpPlayer(bool help)
        {
            base.HelpPlayer(help);
        }

        public override void PickUp()
        {
            base.PickUp();
            agent.enabled = false;

            if (HopAnimation.isPlaying)
                HopAnimation.wrapMode = WrapMode.Once;
        }

        public override void DropDown()
        {
            EnableAgent = true;
        }
    }
}