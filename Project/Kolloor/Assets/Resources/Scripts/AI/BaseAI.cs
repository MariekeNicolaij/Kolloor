using UnityEngine;
using System.Collections;

public class BaseAI : MonoBehaviour
{
    public StateManager stateManager = new StateManager();
    [HideInInspector]   public NavMeshAgent agent;

    [HideInInspector]   public Vector3 direction;

    public bool isUnderWater;

    [Range(1, 10)] public float moveSpeed = 2;
    [Range(1, 10)]      public float hoppiness = 5.5f;
    [HideInInspector]   public float turnTime = 4;
    [Range(1, 10)]      public float minWaitTurnTime = 2;
    [Range(1, 20)]      public float maxWaitTurnTime = 8;

    public bool help;
    float activateHelpTime = 60;


    protected virtual void Start()
    {
        stateManager.Start();
    }

    protected virtual void Update()
    {
        stateManager.Update();

        HelpTimer();
        HelpPlayer();
    }

    void HelpPlayer()
    {
        if (help)
            stateManager.ChangeState(new Help());
    }

    void HelpTimer()
    {
        activateHelpTime -= Time.smoothDeltaTime;

        if (activateHelpTime < 0)
            help = true;
    }
}