using UnityEngine;
using System.Collections;

public class Idle : State
{
    float idleTime;
    float minTime = 2, maxTime = 4;


    public override void Start()
    {
        SetIdleTime();
    }

    void SetIdleTime()
    {
        idleTime = Random.Range(minTime, maxTime);
    }

    public override void Update()
    {
        IdleTimer();
    }

    /// <summary>
    /// Stops current state and changes to given state
    /// </summary>
    /// <param name="state"></param>
    public override void Stop(State state)
    {
        owner.stateManager.ChangeState(state);
    }

    /// <summary>
    /// Change state to wander if time < 0
    /// </summary>
    void IdleTimer()
    {
        idleTime -= Time.smoothDeltaTime;

        if (idleTime < 0)
            Stop(new Wander());
    }
}