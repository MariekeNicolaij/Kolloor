using UnityEngine;
using AI.States;
using System.Collections.Generic;
using Managers;

namespace AI
{
    public class BaseAI : MonoBehaviour
    {
        [Range(1, 10)]
        public int MovementSpeed = 5;

        [Range(0, 2)]
        public float maxPointDistance = 1;

        public int ID
        {
            private set;
            get;
        }

        public AIManager aiManager
        {
            private set;
            get;
        }

        [Range(0, 5)]
        public float MinWaitTime = 2;

        [Range(3, 7)]
        public float MaxWaitTime = 5;

        [HideInInspector]
        public StateManager stateManager;

        [Range(-100, 0)]
        public float MaxFallDepth = -10;

        [HideInInspector]
        public bool Holded = false;

        protected Rigidbody rigidBody;

        protected List<GameObject> Collisions = new List<GameObject>();

        protected NavMeshAgent agent;

        [HideInInspector]
        public NavMeshPath path;

        private Vector3 startPos;

        protected virtual void Start()
        {
            stateManager = new StateManager(this, new WanderState());

            if (aiManager == null)
            {
                aiManager = AIManager.instance; ;
            }

            rigidBody = GetComponent<Rigidbody>();

            ID = aiManager.Register(this);

            stateManager.start();

            startPos = transform.position;
        }

        protected virtual void Update()
        {
            stateManager.Update();

            if (transform.position.y < MaxFallDepth)
                Respawn();
        }

        private void Respawn()
        {
            rigidBody.velocity = Vector3.zero;
            transform.position = startPos;
        }

        /// <summary>
        /// for moving Forward
        /// </summary>
        public virtual void MoveForward()
        {
            if (agent == null)
                transform.Translate(Vector3.forward * MovementSpeed * Time.deltaTime);
        }

        /// <summary>
        /// For looking at an object
        /// </summary>
        /// <param name="objectToLookTo">object to look to</param>
        public virtual void LookAt(Vector3 objectToLookTo)
        {
            transform.LookAt(objectToLookTo);
        }

        public virtual void OnCollisionEnter(Collision other)
        {
            if (!Collisions.Contains(other.gameObject))
            {
                Collisions.Add(other.gameObject);
            }
        }

        public virtual void OnCollisionExit(Collision other)
        {
            if (Collisions.Contains(other.gameObject))
            {
                Collisions.Remove(other.gameObject);
            }
        }

        public virtual void HelpPlayer(bool help)
        {
            if (help)
                stateManager.ChangeState(new HelpState());
            else
                stateManager.SwitchToDefault();
        }
    }
}