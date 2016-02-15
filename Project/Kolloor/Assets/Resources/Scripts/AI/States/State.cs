using UnityEngine;
using System.Collections;

namespace AI.States
{
<<<<<<< HEAD
    public BaseAI owner;
    public Player player { get { return Player.instance; } }
    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void Stop(State state) { }
=======
    public abstract class State
    {
        public BaseAI owner;
        public virtual void Enter() { }
        public virtual void Execute() { }
        public virtual void Exit() { }
    }
>>>>>>> 3c93cf62f4023bf64b86420fb15a4de3f6353500
}