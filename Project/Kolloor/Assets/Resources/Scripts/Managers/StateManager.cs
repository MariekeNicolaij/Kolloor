using UnityEngine;
using System.Collections;
using AI.States;
using AI;

[System.Serializable]
public class StateManager
{
<<<<<<< HEAD
    public BaseAI owner;
    public State currentState;
=======
    private State currentState;
>>>>>>> 3c93cf62f4023bf64b86420fb15a4de3f6353500

    private State defaultState;
    private BaseAI owner;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="owner"> the owner of this StateManager </param>
    /// <param name="defaultState"> the default state in which the owner will start</param>
    public StateManager(BaseAI owner, State defaultState)
    {
        this.defaultState = defaultState;
        this.defaultState.owner = owner;
        this.owner = owner;
        this.currentState = defaultState;
    }

    public void Update()
    {
        currentState.Execute();
    }

    /// <summary>
    /// change the state to a new state
    /// </summary>
    /// <param name="newState"> the new state</param>
    public void ChangeState(State newState)
    {
        if (newState == null)
            return;

<<<<<<< HEAD
        state.owner = owner;
        currentState = state;
        currentState.Start();
=======
        newState.owner = owner;
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
>>>>>>> 3c93cf62f4023bf64b86420fb15a4de3f6353500
    }
}