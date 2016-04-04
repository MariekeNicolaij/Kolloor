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

        GroundAI GroundOwner;

        private bool saveStopRandom = false;
        private Vector3 playerPos;

        public override void Enter()
        {
            player = Player.instance;

            Owner.posToWalkTo = player.transform.position;
            Owner.LookAt(player.transform.position);

            playerPos = player.transform.position;

            GroundOwner = Owner.GetComponent<GroundAI>();

            saveStopRandom = GroundOwner.StopRandom;
            GroundOwner.StopRandom = false;
        }

        public override void Execute()
        {
            Debug.DrawLine(Owner.transform.position, player.transform.position, Color.red);

            if (playerPos != player.transform.position)
            {
                GroundOwner.DestinationSet = false;
                Owner.posToWalkTo = player.transform.position;
                Owner.LookAt(player.transform.position);
            }

            if (Vector3.Distance(Owner.transform.position, player.transform.position) <= Owner.maxPointDistance && !playerFound)
            {
                playerFound = true;
                GroundOwner.DestinationSet = false;
                puzzleObject = PuzzleObjectManager.instance.GetClosestObject(Owner.transform.position);
                Owner.LookAt(puzzleObject.transform.position);
                Owner.posToWalkTo = puzzleObject.transform.position;
            }
            else if (playerFound && Vector3.Distance(Owner.transform.position, puzzleObject.transform.position) <= Owner.maxPointDistance)
            {
                Owner.stateManager.ChangeState(new IdleState());
            }

            Owner.Move();
        }

        public override void Exit()
        {
            GroundOwner.DestinationSet = false;
            GroundOwner.StopRandom = saveStopRandom;
        }
    }
}