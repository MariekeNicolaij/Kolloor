﻿using UnityEngine;
using AI.States;
using System.Collections.Generic;
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

        #region Out of world Care taking
        [Range(-100, 0)]
        public float MaxFallDepth = -10;

        private Vector3 startPos;
        #endregion

        [HideInInspector]
        public AITypes type = AITypes.BaseAI;

        protected Rigidbody rigidBody;

        protected Vector3 lookAt;

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
        }

        protected virtual void Update()
        {
            stateManager.Update();

            if (transform.position.y <= MaxFallDepth)
                Respawn();
        }

        protected void Respawn()
        {
            rigidBody.velocity = Vector3.zero;
            transform.position = startPos;
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
            stateManager.ChangeState(new IdleState());
        }

        public virtual void DropDown()
        {
            stateManager.SwitchToDefault();
        }

    }
}