using UnityEngine;
using System.Collections;

namespace AI
{
    public class GroundWaterBaseAI : BaseAI
    {
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

        protected bool stop = false;
        #endregion

        public NavMeshAgent agent;

        [HideInInspector]
        public bool DestinationSet = false;

        private bool EnableAgent = false;

        protected override void Start()
        {
            base.Start();

            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
                agent = gameObject.AddComponent<NavMeshAgent>();

            agent.speed = MovementSpeed;

            timeTillWait = Random.Range(1, WaitRaid);
        }

        protected override void Update()
        {
            base.Update();

            if (EnableAgent)
            {
                if (rigidBody.IsSleeping())
                {
                    enableAfterDrop();
                }
            }
        }

        public override void Move()
        {
            if (!agent.isOnNavMesh)
                Respawn();

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

            Debug.DrawLine(transform.position, lookAt, Color.blue);

            if (!DestinationSet)
            {
                DestinationSet = true;
                try
                {
                    agent.SetDestination(lookAt);
                }
                catch (System.Exception exc)
                {
                    Debug.Log(this + "got exception: " + exc);
                }
            }
            else
                agent.Resume();
        }

        public override void LookAt(Vector3 placeToLookTo)
        {
            lookAt = placeToLookTo;
        }

        public override void LookAt(GameObject objectToLookTo)
        {
            LookAt(objectToLookTo.transform.position);
        }

        protected virtual void enableAfterDrop()
        {

            rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            agent.enabled = true;
            stateManager.SwitchToDefault();
            EnableAgent = false;
            DestinationSet = false;
        }

        public override void PickUp()
        {
            base.PickUp();
            agent.enabled = false;
        }
        public override void DropDown()
        {
            PickedUp = false;
            EnableAgent = true;
        }

        protected override IEnumerator RespawnParticles()
        {
            yield return base.RespawnParticles();

            if (EnableAgent)
            {
                enableAfterDrop();
            }
        }
    }
}