using UnityEngine;
using System;
using System.Collections.Generic;




public class GameStateManager : Manager<GameStateManager>
{
    private State _currentState = null;
    private Stack<State> m_pActiveStates;
    private Dictionary<string, Type> registeredStates;
    State temp;

    //state im currently in
    public State CurrentState
    {
        get
        {
            return _currentState;
        }
    }



    //create stack of states that are active
    private GameStateManager()
    {
        m_pActiveStates = new Stack<State>();
    }


    //destructor stuff?
    protected override void Terminate()
    {
    }


    //run through each active state
    public void Update()
    {
        float deltaTime = Time.deltaTime;
        foreach (State state in m_pActiveStates)
        {
            if (m_pActiveStates.Count > 0)
            {
                state.Process.Invoke(deltaTime);
                //if blocking dont update states under it
                if (state.IsBlocking)
                {
                    break;
                }
            }
            break;
        }
    }
    /// <summary>
    /// Test to see if a state is already present 
    /// </summary>
    /// <returns>The state if it exists, null otherwise</returns>
    /// <param name="a_stateName">The name of the state to locate.</param>
    public State StateExists(string a_stateName)
    {
        foreach (State state in m_pActiveStates)
        {
            string pName = state.StateName;
            if (pName != null && pName == a_stateName)
            {
                return state;
            }
        }
        return null;
    }


    /// <summary>
    /// call to enter the desired state
    /// </summary>
    /// <returns>true once state is entered, false if could not find state</returns>
    /// <param name="a_stateName">The name of the state to enter.</param>
    public bool EnterState(string a_stateName)
    {
        State pState = StateExists(a_stateName);
        if (pState != null) //Our state is already in the list of active States
        {
            
            PopToState(pState);
            return true;
        }
        //if not in stack of actives
        else
        {
            //create instance and put in stack
            if (registeredStates.ContainsKey(a_stateName))
            {
                State nextState = Activator.CreateInstance(registeredStates[a_stateName], a_stateName) as State;
                PushState(nextState);
                return true;
            }
        }
        return false;
    }



    //check to see if state is in active state
    //pop to it
    private void PopToState(State a_state)
    {
        temp = m_pActiveStates.Peek(); //= current state
        //while we not at desired state or stack != 0
        while (m_pActiveStates.Count != 0 && m_pActiveStates.Peek() != a_state)
        {
            //keep popping
            m_pActiveStates.Pop();
        }
        //current state = top of state
        _currentState = m_pActiveStates.Peek();

        //this is messy but works

        temp.Process.Invoke(0.0f); //get previous current state and invoke its update function, this will set its process to leave
        temp.Process.Invoke(0.0f); //now reinvoke to run through leave function

        temp = null;
    }


    //push state to stack
    private void PushState(State a_state)
    {
        //if states != 0 then get the current state
        if (m_pActiveStates.Count != 0)
        {
            temp = m_pActiveStates.Peek(); //this is get state before one we about to add
        }
        m_pActiveStates.Push(a_state); //push state we want to active states
        _currentState = m_pActiveStates.Peek(); //set current state to one we just added
        if (temp != null)
        {
            temp.Process.Invoke(0.0f);  //get previous current state and invoke its update function, this will set its process to leave
            temp.Process.Invoke(0.0f);  //now reinvoke to run through leave function
            temp = null;
        }
    }

    //basic pop state of stack
    private void PopState()
    {
        //pop
        m_pActiveStates.Pop();
        //current state = next state on stack
        _currentState = m_pActiveStates.Peek();
    }

    //dictionary to hold all possible states we have defined
    public void RegisterState<T>(string a_stateName)
    where T : State
    {
        if (registeredStates == null) //if registered states empty
        {
            registeredStates = new Dictionary<string, Type>(); //create a new dictionary of states
        }
        registeredStates.Add(a_stateName, typeof(T)); //add a state to dictionary
    }
}