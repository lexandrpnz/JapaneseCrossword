using System.Collections.Generic;

namespace JCClasses
{
    public class State<TCommand, TState>
    {
        public TState Next(TCommand command)
        {
            return transitions[command];
        }

        public ICollection<TState> GetNext()
        {
            return transitions.Values;
        }

        public void AddTransitions(TCommand command, TState state)
        {
            transitions.Add(command, state);
        }

        private Dictionary<TCommand, TState> transitions = new Dictionary<TCommand, TState>();
    }
}
