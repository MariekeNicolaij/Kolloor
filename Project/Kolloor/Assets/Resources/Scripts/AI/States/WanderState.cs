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

        private GroundWaterBaseAI groundWaterOwner;

        public override void Enter()
        {
            LayerMask areas = new LayerMask();

            switch (Owner.type)
            {
                case AITypes.BaseAI:
                    break;
                case AITypes.GroundAI:
                    if (!groundWaterOwner)
                        groundWaterOwner = (GroundWaterBaseAI)Owner;

                    areas = 1 << NavMesh.GetAreaFromName("Terrain") << NavMesh.GetAreaFromName("Walkable");

                    Owner.posToWalkTo = Owner.aiManager.GetRandomPoint(Owner.transform.position, Owner.type, areas);
                    break;
                case AITypes.WaterAI:
                    if (!groundWaterOwner)
                        groundWaterOwner = (GroundWaterBaseAI)Owner;

                    areas = groundWaterOwner.agent.areaMask;

                    Owner.posToWalkTo = Owner.aiManager.GetRandomPoint(Owner.transform.position, Owner.type, areas);
                    break;
                default:
                    break;
            }


        }

        public override void Execute()
        {
            switch (Owner.type)
            {
                case AITypes.BaseAI:
                    break;
                case AITypes.GroundAI:
                    GroundWaterExecute();
                    break;
                case AITypes.WaterAI:
                    GroundWaterExecute();
                    break;
                default:
                    break;
            }
        }

        private void GroundWaterExecute()
        {
            if (Vector3.Distance(Owner.transform.position, Owner.posToWalkTo) <= Owner.maxPointDistance)
            {
                Owner.posToWalkTo = Owner.aiManager.GetRandomPoint(Owner.transform.position, Owner.type, NavMesh.GetAreaFromName("Terrain"));
                groundWaterOwner.destinationset = false;
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