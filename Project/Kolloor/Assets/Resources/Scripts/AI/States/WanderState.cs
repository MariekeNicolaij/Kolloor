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
        private LayerMask areas = new LayerMask();

        private GroundWaterBaseAI groundWaterOwner;
        private AirAI airAI;

        private bool hasPath = false;

        public override void Enter()
        {
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

                    areas = 1 << NavMesh.GetAreaFromName("Water");

                    Owner.posToWalkTo = Owner.aiManager.GetRandomPoint(Owner.transform.position, Owner.type, areas);

                    Owner.LookAt(Owner.posToWalkTo);
                    Owner.Move();
                    break;
                case AITypes.AirAI:
                    airAI = Owner.GetComponent<AirAI>();

                    Debug.Log(Owner.aiManager.GetRandomPoint(Owner.transform.position, Owner.type, NavMesh.AllAreas));

                    Owner.posToWalkTo = Owner.aiManager.GetRandomPoint(Owner.transform.position, Owner.type, NavMesh.AllAreas);

                    Owner.posToWalkTo.y += Random.Range(airAI.MinFlighHight, airAI.MaxFlighHight);
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
                    Debug.Log("There should not be an baseAI in game");
                    break;
                case AITypes.GroundAI:
                    GroundWaterExecute();
                    break;
                case AITypes.WaterAI:
                    GroundWaterExecute();
                    break;
                case AITypes.AirAI:
                    AirExecute();
                    break;
                default:
                    break;
            }
        }

        private void AirExecute()
        {
            if (Vector3.Distance(Owner.transform.position, Owner.posToWalkTo) <= Owner.maxPointDistance)
            {
                Owner.posToWalkTo = Owner.aiManager.GetRandomPoint(Owner.transform.position, Owner.type, areas);
                Owner.posToWalkTo.y += Random.Range(airAI.MinFlighHight, airAI.MaxFlighHight);
            }
            else
            {
                Owner.LookAt(Owner.posToWalkTo);
                Owner.Move();
            }
        }

        private void GroundWaterExecute()
        {
            if (Vector3.Distance(Owner.transform.position, groundWaterOwner.agent.pathEndPosition) <= Owner.maxPointDistance && hasPath)
            {
                Owner.posToWalkTo = Owner.aiManager.GetRandomPoint(Owner.transform.position, Owner.type, areas);
                groundWaterOwner.DestinationSet = false;

                if (hasPath)
                    hasPath = false;
            }
            else
            {
                Owner.LookAt(Owner.posToWalkTo);
                Owner.Move();

                if (!hasPath)
                    hasPath = true;
            }
        }

        public override void Exit()
        {
            groundWaterOwner.DestinationSet = false;
        }
    }
}