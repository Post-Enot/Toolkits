using System;
using System.Collections.Generic;

namespace PostEnot.Toolkits.StateMachines.HFSM
{
    internal sealed class InnerHFSM<TStateBase, TContext, TMessageHandler>
        : IHFSM<TStateBase, TContext, TMessageHandler>
        where TStateBase : StateBase<TStateBase, TContext>
        where TMessageHandler : StateBase<TStateBase, TContext>.MessageHandlerBase, new()
    {
        private sealed class StateDelegator : IStateSetter<TStateBase, TContext>
        {
            public StateDelegator(
                StateCollection<TStateBase, TContext> states,
                StateBase<TStateBase, TContext>.MessageHandlerBase messageHandler)
            {
                _states = states;
                _messageHandler = messageHandler;
            }

            private readonly StateCollection<TStateBase, TContext> _states;
            private readonly StateBase<TStateBase, TContext>.MessageHandlerBase _messageHandler;

            private TStateBase _state;

            public void SetState<TState>() where TState : TStateBase, new()
            {
                _state = _states.GetOrCreateState<TState>(_messageHandler);
                _state.DelegateTransit(this);
            }

            public TStateBase ExtractState()
            {
                TStateBase state = _state;
                _state = null;
                return state;
            }
        }

        private InnerHFSM(TContext context, IStateSetter<TStateBase, TContext> stateSetter)
        {
            Context = context;
            MessageHandler.Context = context;
            _stateDelegator = new StateDelegator(_states, MessageHandler);
            _stateSetter = stateSetter;
        }

        public TContext Context { get; private set; }
        public TStateBase ActiveState { get; private set; }
        public TStateBase RootState
        {
            get
            {
                TStateBase root = ActiveState;
                while (root.ParentState != null)
                {
                    root = root.ParentState;
                }
                return root;
            }
        }
        public IReadOnlyCollection<TStateBase> States => _states;
        public IEnumerable<TStateBase> FromRootToActive
        {
            get
            {
                int version = _version;
                TStateBase root = RootState;
                while (root != null)
                {
                    yield return root;
                    if (version != _version)
                    {
                        yield break;
                    }
                    root = root.ActiveChildState;
                }
            }
        }
        public IEnumerable<TStateBase> FromActiveToRoot
        {
            get
            {
                int version = _version;
                TStateBase active = ActiveState;
                while (active != null)
                {
                    yield return active;
                    if (version != _version)
                    {
                        yield break;
                    }
                    active = active.ParentState;
                }
            }
        }
        public TMessageHandler MessageHandler { get; } = new();
        public bool IsDisposed { get; private set; }

        public event StateChangedAction<TStateBase, TContext> StateChanged;
        public event SelfTransitionAttempedAction<TStateBase, TContext> SelfTransitionAttemped;

        private readonly StateCollection<TStateBase, TContext> _states = new();
        private readonly StateDelegator _stateDelegator;
        private readonly IStateSetter<TStateBase, TContext> _stateSetter;

        private int _version;

        public void SetState<TState>() where TState : TStateBase, new()
        {
            TStateBase state = _states.GetOrCreateState<TState>(MessageHandler);
            state.DelegateTransit(_stateDelegator);
            TStateBase newState = _stateDelegator.ExtractState() ?? state;
            if (newState == ActiveState)
            {
                SelfTransitionAttemped?.Invoke(ActiveState);
                return;
            }
            unchecked
            {
                _version += 1;
            }
            TStateBase previousState = ActiveState;
            TStateBase lca = LCA(ActiveState, newState);
            ExitLCA(ActiveState, lca);
            ActiveState = newState;
            EnterLCA(ActiveState, lca);
            StateChanged?.Invoke(previousState, newState);
        }

        public void Dispose()
        {
            ExitLCA(ActiveState, null);
            foreach (TStateBase state in _states)
            {
                MessageHandler.HandleOnDispose(state);
            }
            IsDisposed = true;
        }

        public static InnerHFSM<TStateBase, TContext, TMessageHandler> Instantiate<TState>(
            TContext context,
            IStateSetter<TStateBase, TContext> stateSetter)
            where TState : TStateBase, new()
        {
            InnerHFSM<TStateBase, TContext, TMessageHandler> hfsm = new(context, stateSetter);
            hfsm.SetState<TState>();
            return hfsm;
        }

        private void EnterLCA(TStateBase to, TStateBase lca, TStateBase activeChild = null)
        {
            if (to == lca)
            {
                if (to != null)
                {
                    to.ActiveChildState = activeChild;
                }
                return;
            }
            EnterLCA(to.ParentState, lca, to);
            MessageHandler.HandleOnEnter(to, _stateSetter);
            to.ActiveChildState = activeChild;
        }

        private void ExitLCA(TStateBase from, TStateBase lca)
        {
            while (from != lca)
            {
                MessageHandler.HandleOnExit(from);
                from.ActiveChildState = null;
                from = from.ParentState;
            }
        }

        private static TStateBase LCA(TStateBase a, TStateBase b)
        {
            int iterCount = 0;
            TStateBase ptrA = a;
            TStateBase ptrB = b;
            while (ptrA != ptrB)
            {
                ptrA = ptrA == null ? b : ptrA.ParentState;
                ptrB = ptrB == null ? a : ptrB.ParentState;
                iterCount += 1;
                if (iterCount == int.MaxValue)
                {
                    throw new InvalidOperationException($"HFSM infinite loop detected: {a} -> {b}.");
                }
            }
            return ptrA;
        }
    }
}
