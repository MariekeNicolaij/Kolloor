using UnityEngine;
using System.Collections;

[System.Serializable]
public class StateManager
{
    public Animal animal;
    public BaseAI owner;
    public State currentState;

    public void Start()
    {
        ChangeState(new Wander());
    }

    public void Update()
    {
        if (currentState != null)
            currentState.Update();
    }

    public void ChangeState(State state)
    {
        if (state == null && currentState == null)
            return;

        state.owner = owner;
        state.animal = animal;
        currentState = state;
        currentState.Start();
    }
}