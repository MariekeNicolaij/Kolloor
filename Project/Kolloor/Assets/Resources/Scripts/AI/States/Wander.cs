using UnityEngine;
using System.Collections;

public class Wander : State
{
    float wanderTime;
    float minTime = 8, maxTime = 16;


    public override void Start()
    {
        owner.direction = owner.transform.forward;
        SetWanderTime();
    }

    void SetWanderTime()
    {
        wanderTime = Random.Range(minTime, maxTime);
    }

    public override void Update()
    {
        WanderTimer();
        Move();
        Hop();
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
    /// Change state to idle if time < 0
    /// </summary>
    void WanderTimer()
    {
        wanderTime -= Time.smoothDeltaTime;

        if (wanderTime < 0)
            Stop(new Idle());
    }

    /// <summary>
    /// Moves the AI
    /// </summary>
    void Move()
    {
        if (owner.agent)
        {
            owner.agent.SetDestination(owner.transform.position + owner.direction * owner.moveSpeed * Time.smoothDeltaTime);
            //Debug.Log(owner.transform.position + owner.direction * owner.moveSpeed * Time.smoothDeltaTime);
        }
    }

    /// <summary>
    /// Hops the AI
    /// </summary>
    void Hop()
    {
        //owner.hoppiness
    }
}