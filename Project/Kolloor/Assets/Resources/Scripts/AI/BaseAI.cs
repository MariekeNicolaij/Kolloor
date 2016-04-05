using UnityEngine;
using AI.States;
using System.Collections;
using Managers;

namespace AI
{
    public class BaseAI : MonoBehaviour
    {
        #region Moving Properties
        [Range(1, 10)]
        public float MovementSpeed = 5;

        [Range(0, 2)]
        public float maxPointDistance = 1;

        [HideInInspector]
        public Vector3 posToWalkTo = Vector3.zero;
        #endregion

        private GameObject Water;

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

        [HideInInspector]
        public StateManager stateManager;

        #region respawn and Out of world Care taking
        [Range(-100, 0)]
        public float MaxFallDepth = -10;

        private Vector3 startPos;
        public float TimeToRespawn = .2f;
        public float RespawnEffectTime = .5f;
        #endregion

        [HideInInspector]
        public AITypes type = AITypes.BaseAI;

        protected Rigidbody rigidBody;

        protected Vector3 lookAt;

        protected ParticleSystem pSystem;

        protected bool PickedUp = false;

        protected virtual void Start()
        {
            stateManager = new StateManager(this, new WanderState());

            if (!aiManager)
            {
                aiManager = AIManager.instance;
                if (!aiManager) // for if there is no aimanager
                    Debug.LogError("there is no aimanager in this world!");
            }

            rigidBody = GetComponent<Rigidbody>();

            ID = aiManager.Register(this, type);

            stateManager.start();

            startPos = transform.position;

            pSystem = GetComponentInChildren<ParticleSystem>(true);
        }

        protected virtual void Update()
        {
            stateManager.Update();

            if (transform.position.y <= MaxFallDepth)
                Respawn();
        }

        protected void Respawn()
        {
            //if (!PickedUp)
            {
                pSystem.gameObject.SetActive(true);
                pSystem.startColor = Color.cyan;

                StartCoroutine(RespawnParticles());
            }
        }

        protected IEnumerator RespawnParticles()
        {
            float i = 0;
            while (i <= TimeToRespawn)
            {
                i += Time.deltaTime;
                yield return null;
            }

            rigidBody.velocity = Vector3.zero;
            transform.position = startPos;

            while (i <= RespawnEffectTime)
            {
                i += Time.deltaTime;
                yield return null;
            }
            pSystem.gameObject.SetActive(false);
        }

        public virtual void Move()
        {
            MoveForward();
        }

        /// <summary>
        /// for moving Forward
        /// </summary>
        protected virtual void MoveForward()
        {
            transform.Translate((Vector3.forward * Time.deltaTime) * MovementSpeed);
        }

        /// <summary>
        /// For looking to an place
        /// </summary>
        /// <param name="objectToLookTo">place to look to</param>
        public virtual void LookAt(Vector3 placeToLookTo)
        {
            transform.LookAt(placeToLookTo);
            lookAt = placeToLookTo;
        }

        /// <summary>
        /// For looking to an object
        /// </summary>
        /// <param name="objectToLookTo">object to look to</param>
        public virtual void LookAt(GameObject objectToLookTo)
        {
            LookAt(objectToLookTo.transform.position);
        }

        public virtual void HelpPlayer(bool help)
        {
            if (help)
                stateManager.ChangeState(new HelpState());
            else
                stateManager.SwitchToDefault();
        }

        public virtual void PickUp()
        {
            PickedUp = true;
            stateManager.ChangeState(new IdleState());
        }

        public virtual void DropDown()
        {
            PickedUp = false;
            stateManager.SwitchToDefault();
        }

    }
}