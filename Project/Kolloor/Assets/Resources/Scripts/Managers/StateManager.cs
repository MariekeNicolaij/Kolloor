using UnityEngine;
using System.Collections;
using AI.States;
using AI;

namespace Managers
{
    [System.Serializable]
    public class StateManager
    {
        private State currentState;

        private BaseAI owner;

        private State defaultState;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner"> the owner of this StateManager </param>
        /// <param name="defaultState"> the default state in which the owner will start</param>
        public StateManager(BaseAI owner, State defaultState)
        {
            this.defaultState = defaultState;
            this.defaultState.Owner = owner;
            this.owner = owner;
            currentState = defaultState;
        }

        public void start()
        {
            currentState.Enter();
        }

        public void Update()
        {
            currentState.Execute();
        }

        /// <summary>
        /// change the state to a new state
        /// </summary>
        /// <param name="newState"> the new state</param>
        public void ChangeState(State newState)
        {
            if (newState == null)
                return;

            newState.Owner = owner;
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }

        public void SwitchToDefault()
        {
            currentState.Exit();
            currentState = defaultState;
            currentState.Owner = owner;
            currentState.Enter();
        }
    }
}