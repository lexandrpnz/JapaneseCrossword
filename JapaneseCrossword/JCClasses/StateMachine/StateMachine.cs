using System;
using System.Collections.Generic;

namespace JCClasses
{
    class StateMachine<TState, TCommand> where TState:IComparable
    {
        public TState Next(TCommand symbol)
        {
            CurrentState = states[CurrentState].Next(symbol);
            return CurrentState;
        }

        public bool IsEnd()
        {
            return 0 == CurrentState.CompareTo(EndState);
        }

        public void Add(TState current, TCommand command, TState next)
        {
            if(!states.ContainsKey(current))
            {
                states.Add(current, new State<TCommand, TState>());
            }
            states[current].AddTransitions(command, next);
        }

        public TState EndState { get; set; }
        public TState CurrentState { get; set; }
        private Dictionary<TState, State<TCommand, TState>> states = new Dictionary<TState, State<TCommand, TState>>();
    }
}
