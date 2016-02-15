using UnityEngine;
using System.Collections;
using AI.States;

namespace AI
{
    public class BaseAI : MonoBehaviour
    {
        protected StateManager stateManager;

        [HideInInspector]
        public NavMeshAgent agent;

        protected virtual void Start()
        {
            agent = this.GetComponent<NavMeshAgent>();
            stateManager = new StateManager(this, new WanderState());
        }

        protected virtual void Update()
        {
            stateManager.Update();
        }

        public void GoTo(Vector3 position)
        {
            agent.SetDestination(position);
        }

        public void Move(Vector3 dir)
        {
            agent.Move(dir);
        }
    }
}