using System;
using System.Collections;
using System.Collections.Generic;

namespace PostEnot.Toolkits.StateMachines.HFSM
{
    internal sealed class StateCollection<TStateBase, TContext>
        : IReadOnlyCollection<TStateBase>, IStateGetter<TStateBase, TContext>
        where TStateBase : StateBase<TStateBase, TContext>
    {
        public int Count => _states.Count;
        public IReadOnlyCollection<Type> Types => _states.Keys;

        private readonly Dictionary<Type, TStateBase> _states = new();

        public bool IsStateInitialized<TState>()
        {
            Type type = typeof(TState);
            return _states.ContainsKey(type);
        }

        public bool InitializeState<TState>(
            StateBase<TStateBase, TContext>.MessageHandlerBase messageHandler) where TState : TStateBase, new()
        {
            Type type = typeof(TState);
            if (_states.ContainsKey(type))
            {
                return false;
            }
            TState state = new();
            messageHandler.HandleOnInit(state, this);
            _states.Add(type, state);
            return true;
        }

        public TStateBase GetOrCreateState<TState>(
            StateBase<TStateBase, TContext>.MessageHandlerBase messageHandler) where TState : TStateBase, new()
        {
            Type type = typeof(TState);
            if (_states.TryGetValue(type, out TStateBase state))
            {
                return state;
            }
            state = new TState();
            messageHandler.HandleOnInit(state, this);
            _states.Add(type, state);
            return state;
        }

        public bool DisposeState(Type type, StateBase<TStateBase, TContext>.MessageHandlerBase messageHandler)
        {
            if (_states.Remove(type, out TStateBase state))
            {
                messageHandler.HandleOnDispose(state);
                return true;
            }
            return false;
        }

        public IEnumerator<TStateBase> GetEnumerator() => _states.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
