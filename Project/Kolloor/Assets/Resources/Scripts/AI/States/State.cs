using UnityEngine;
using System.Collections;

namespace AI.States
{
    public abstract class State
    {
        public BaseAI Owner;
        public virtual void Enter() { }
        public virtual void Execute() { }
        public virtual void Exit() { }
    }
}