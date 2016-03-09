using UnityEngine;
using System.Collections;
using Managers;
using System.Linq;

namespace AI.States
{
    public class HelpState : State
    {
        private Player player;
        private PuzzleObject puzzleObject;
        private bool playerFound = false;
        private NavMeshPath path;

        private int corner = 0;

        private bool saveStopRandom = false;
        private Vector3 playerPos;

        public override void Enter()
        {
            path = new NavMeshPath();

            player = Player.instance;

            NavMesh.CalculatePath(Owner.transform.position, player.transform.position, NavMesh.AllAreas, path);

            if (Owner is GroundAI)
            {
                saveStopRandom = ((GroundAI)Owner).StopRandom;
                ((GroundAI)Owner).StopRandom = false;
            }
        }

        public override void Execute()
        {
            Debug.DrawLine(Owner.transform.position, path.corners[corner], Color.red);

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
            }

            if (Vector3.Distance(path.corners[corner], Owner.transform.position) < Owner.maxPointDistance && corner != path.corners.Length - 1)
            {
                corner++;
            }
            else if (Vector3.Distance(player.transform.position, Owner.transform.position) < Owner.maxPointDistance + 1 && !playerFound)
            {
                playerFound = true;
                puzzleObject = PuzzleObjectManager.instance.GetClosestObject(Owner.transform.position);
                Debug.Log(puzzleObject);
                NavMesh.CalculatePath(Owner.transform.position, puzzleObject.transform.position, NavMesh.AllAreas, path);
                corner = 0;
            }
            else if (Vector3.Distance(path.corners[corner], Owner.transform.position) < Owner.maxPointDistance && !playerFound)
            {
                NavMesh.CalculatePath(Owner.transform.position, player.transform.position, NavMesh.AllAreas, path);
                corner = 0;
            }
            else if (playerFound && Vector3.Distance(puzzleObject.transform.position, Owner.transform.position) < (Owner.maxPointDistance * 4))
            {

            }
            else
            {
                Vector3 VecToLookAt = path.corners[corner];
                VecToLookAt.y = Owner.transform.position.y;

                Owner.LookAt(VecToLookAt);
                Owner.Move();
            }

        }
        public override void Exit() { }
    }
}