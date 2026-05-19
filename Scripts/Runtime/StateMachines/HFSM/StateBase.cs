using System;

namespace PostEnot.Toolkits.StateMachines.HFSM
{
    public abstract class StateBase<TStateBase, TContext> where TStateBase : StateBase<TStateBase, TContext>
    {
        public readonly ref struct InitParentContext
        {
            internal InitParentContext(
                IStateGetter<TStateBase, TContext> stateGetter,
                MessageHandlerBase messageHandler)
            {
                _stateGetter = stateGetter;
                _messageHandler = messageHandler;
            }

            public readonly TStateBase Root => null;

            private readonly IStateGetter<TStateBase, TContext> _stateGetter;
            private readonly MessageHandlerBase _messageHandler;

            public readonly TStateBase Parent<TState>() where TState : TStateBase, new()
                => _stateGetter.GetOrCreateState<TState>(_messageHandler);
        }

        public class MessageHandlerBase
        {
            public TContext Context { get; internal set; }

            public virtual void HandleOnInit(TStateBase state, IStateGetter<TStateBase, TContext> stateGetter)
            {
                state.Context = Context;
                InitParentContext initParentContext = new(stateGetter, this);
                state.ParentState = state.InitParent(initParentContext);
                state.OnInit();
            }

            public virtual void HandleOnEnter(TStateBase state, IStateSetter<TStateBase, TContext> stateSetter)
            {
                state.OnEnter();
                state._stateSetter = stateSetter;
            }

            public virtual void HandleOnExit(TStateBase state)
            {
                state._stateSetter = null;
                state.OnExit();
            }

            public virtual void HandleOnDispose(TStateBase state)
            {
                state._stateSetter = null;
                state.OnDispose();
            }
        }

        public TContext Context { get; private set; }
        public TStateBase ParentState { get; private set; }
        public TStateBase ActiveChildState { get; internal set; }

        public bool IsActive => _stateSetter != null;

        private IStateSetter<TStateBase, TContext> _stateSetter;

        protected virtual TStateBase InitParent(InitParentContext context) => context.Root;

        protected virtual void OnInit() { }

        protected virtual void OnEnter() { }

        protected virtual void OnExit() { }

        protected virtual void OnDispose() { }

        protected virtual void DelegateTransit() { }

        internal void DelegateTransit(IStateSetter<TStateBase, TContext> stateSetter)
        {
            IStateSetter<TStateBase, TContext> temp = _stateSetter;
            _stateSetter = stateSetter;
            DelegateTransit();
            _stateSetter = temp;
        }

        protected void TransitTo<TState>() where TState : TStateBase, new()
        {
            if (_stateSetter == null)
            {
                throw new InvalidOperationException(
                            $"Cannot perform transition because the state {GetType().FullName} is not attached to an HFSM. " +
                            "Ensure that TransitTo is called only after the state has been entered " +
                            "(OnEnter) and before it is exited (OnExit).");
            }
            _stateSetter.SetState<TState>();
        }
    }
}
