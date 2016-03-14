using UnityEngine;
using System.Collections;
using System.Linq;

namespace AI.States
{
    public class WanderState : State
    {
        private int corner = 0;
        private bool wait = false;
        private float WaitTime = 0;

        private GroundAI groundOwner;

        public override void Enter()
        {
            if (Owner.posToWalkTo == Vector3.zero)
                Owner.posToWalkTo = Owner.aiManager.CreateRandomPoint();
        }

        public override void Execute()
        {
            switch (Owner.type)
            {
                case AITypes.BaseAI:
                    break;
                case AITypes.GroundAI:
                    GroundExecute();
                    break;
                case AITypes.WaterAI:
                    break;
                default:
                    break;
            }
        }

        private void GroundExecute()
        {
            if (!groundOwner)
                groundOwner = (GroundAI)Owner;

            if (Vector3.Distance(groundOwner.transform.position, Owner.posToWalkTo) <= Owner.maxPointDistance)
            {
                Owner.posToWalkTo = Owner.aiManager.CreateRandomPoint();
            }
            else
            {
                Owner.LookAt(Owner.posToWalkTo);
                Owner.Move();
            }
        }

        public override void Exit() { }
    }
}