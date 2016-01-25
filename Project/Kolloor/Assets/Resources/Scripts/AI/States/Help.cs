using UnityEngine;
using System.Collections;

public class Help : State
{
    public override void Start()
    {
        
    }

    public override void Update()
    {
       
    }

    /// <summary>
    /// Stops current state and changes to given state
    /// </summary>
    /// <param name="state"></param>
    public override void Stop(State state)
    {
        owner.help = false;
        owner.stateManager.ChangeState(state);
    }
}