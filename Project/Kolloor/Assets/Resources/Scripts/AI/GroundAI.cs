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
        [Range(1, 10)]
        public float HopHeight = 5;

        public float HopSpeed = 1;

        private GameObject HoppingObject;
        private Rigidbody HoppingObjectRigidBody;

        private bool up = true;

        private bool groundContact = true;

        private bool addedForce = false;
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

        protected NavMeshAgent agent;

        private bool EnableAgent = false;

        protected override void Start()
        {
            type = AITypes.GroundAI;
            base.Start();
            timeTillWait = Random.Range(1, WaitRaid);

            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
                agent = gameObject.AddComponent<NavMeshAgent>();

            if (HopOn)
                HoppingObject = transform.GetChild(0).gameObject;

            HoppingObjectRigidBody = HoppingObject.GetComponent<Rigidbody>();
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

        public override void Move()
        {
            if (StopRandom)
            {
                if (stop && groundContact)
                {
                    counter += Time.smoothDeltaTime;
                    if (counter >= waitFor)
                    {
                        stop = false;
                        counter = 0;
                        timeTillWait = Random.Range(1, WaitRaid);
                    }
                    agent.Stop();
                }
                else
                {
                    if (!stop)
                    {
                        counter += Time.smoothDeltaTime;
                        if (counter >= timeTillWait)
                        {
                            stop = true;
                            counter = 0;
                            waitFor = Random.Range(MinRandomStopTime, MaxRandomStopTime);
                        }
                    }
                    MoveForward();
                }
            }
            else {
                MoveForward();
            }
        }



        protected override void MoveForward()
        {
            float hop = Time.smoothDeltaTime * HopSpeed;

            //Debug.Log("smoothdeltatime = " + Time.smoothDeltaTime);
            if (HopOn)
            {
                Debug.Log(up);
                if (up)
                {
                    if (groundContact)
                        groundContact = false;

                    Vector3 pos = HoppingObject.transform.position;
                    pos.y += hop;
                    HoppingObject.transform.position = pos;

                    if (HoppingObject.transform.position.y >= transform.position.y + HopHeight)
                    {
                        //HoppingObjectRigidBody.velocity = Vector3.zero;
                        up = false;
                        //addedForce = false;
                    }
                }
                else
                {
                    Vector3 pos = HoppingObject.transform.position;
                    pos.y -= hop;
                    HoppingObject.transform.position = pos;

                    if (HoppingObject.transform.position.y <= transform.position.y)
                    {
                        //HoppingObjectRigidBody.velocity = Vector3.zero;
                        groundContact = true;
                        up = true;
                        //addedForce = false;
                    }
                }
            }


            if (agent.destination != lookAt)
                agent.SetDestination(lookAt);
            else
                agent.Resume();
        }

        public override void LookAt(Vector3 placeToLookTo)
        {
            lookAt = placeToLookTo;
        }

        public override void LookAt(GameObject objectToLookTo)
        {
            base.LookAt(objectToLookTo);
        }

        public override void HelpPlayer(bool help)
        {
            base.HelpPlayer(help);
        }

        public override void PickUp()
        {
            base.PickUp();
            agent.enabled = false;
        }

        public override void DropDown()
        {
            EnableAgent = true;
        }
    }
}