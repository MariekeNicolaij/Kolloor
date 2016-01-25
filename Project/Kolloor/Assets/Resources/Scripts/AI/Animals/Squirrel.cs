using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Squirrel : BaseAI
{
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Update()
    {
        base.Update();
        Turn();
    }

    void Turn()
    {
        turnTime -= Time.smoothDeltaTime;

        if (turnTime < 0)
        {
            direction = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            Debug.Log(direction);
            agent.SetDestination(direction);
            turnTime = Random.Range(minWaitTurnTime, maxWaitTurnTime);
        }
    }
}