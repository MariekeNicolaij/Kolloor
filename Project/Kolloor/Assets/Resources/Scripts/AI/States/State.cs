using UnityEngine;
using System.Collections;

public abstract class State
{
    public Animal animal;
    public BaseAI owner;
    public Player player { get { return Player.instance; } }
    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void Stop(State state) { }
}