using UnityEngine;
using System.Collections;

namespace AI.States
{
    public class WanderState : State
    {
        public BaseAI Owner;

        public override void Enter() { }

        public override void Execute()
        {
            Owner.Move(Vector3.forward);
        }

        public override void Exit() { }
    }
}