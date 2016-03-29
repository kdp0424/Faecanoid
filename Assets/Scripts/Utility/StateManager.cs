using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class StateManager<T>
{

    public string displayedStateName = "";

    public Action OnChanged;

    private Dictionary<object, State> stateLookup;

    private T _value;
    public T value
    {
        get
        {
            return _value;
        }
        set
        {
            if (state == stateLookup[value]) return;
            Debug.Log("State is: " + _value + " || Changing state to: " + value);
            _value = value;
            state = stateLookup[_value];
        }
    }

    public Dictionary<T, State> values
    {
        get; private set;
    }

    private State _state;
    [SerializeField]
    public State state
    {
        get
        {
            return _state;
        }
        set
        {
            if (value == null)
            {
                Debug.LogError("Attempting to enter null state. Ensure state has been constructed before assigning it to a StateManager.");
            }

            if (_state == value)
            {
                Debug.Log("Not performing redundant state change.");
                return;
            }

            if (_state != null && _state.OnExit != null)
            {
                _state.OnExit();
                "Calling OnExit for state".DebugLogJustin();
            }
            else
            {
                "OnExit for state is null".DebugLogDom();
            }

            var nextState = value;

            _state = nextState;

            if (_state.OnEnter != null) _state.OnEnter();

            if (OnChanged != null) OnChanged();
        }
    }

    public static StateManager<T> CreateNew()
    {
        var stateManager = new StateManager<T>();

        stateManager.Initialize();

        return stateManager;
    }

    private StateManager<T> Initialize()
    {
        //Define States
        var values = Enum.GetValues(typeof(T));
        if (values.Length < 1) { throw new ArgumentException("Enum provided to Initialize must have at least 1 visible definition"); }

        this.values = new Dictionary<T, State>();

        stateLookup = new Dictionary<object, State>();
        for (int i = 0; i < values.Length; i++)
        {
            var mapping = new State((Enum)values.GetValue(i));
            stateLookup.Add(mapping.state, mapping);
            this.values.Add((T)values.GetValue(i), stateLookup[mapping.state]);
        }

        this.value = default(T);

        return new StateManager<T>();
    }

    public void ChangeState(T value)
    {
        Debug.Log("Changing state to: " + value);
        state = stateLookup[value];
    }

    public State GetState(T value)
    {
        return stateLookup[value];
    }

    public class State
    {
        public object state;

        public Action OnEnter;

        public Action OnExit;

        public State(object state)
        {
            this.state = state;
        }

    }
}