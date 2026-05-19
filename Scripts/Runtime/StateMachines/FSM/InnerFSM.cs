using System;
using System.Collections.Generic;

namespace PostEnot.Toolkits.StateMachines.FSM
{
    /// <summary>
    /// Внутренняя реализация FSM, управляющая коллекцией состояний и активным состоянием.
    /// </summary>
    /// <typeparam name="TStateBase">Базовый тип состояний, унаследованный от
    /// <see cref="FSM{TStateBase, TContext}.State"/>.</typeparam>
    /// <typeparam name="TContext">Тип контекста, общего для всех состояний.</typeparam>
    internal sealed class InnerFSM<TStateBase, TContext, TMessageHandler> : IFSM<TStateBase, TContext, TMessageHandler>
        where TStateBase : StateBase<TStateBase, TContext>
        where TMessageHandler : StateBase<TStateBase, TContext>.MessageHandlerBase, new()
    {
        private InnerFSM(TContext context, IStateSetter<TStateBase, TContext> stateSetter)
        {
            Context = context;
            MessageHandler.Context = context;
            _stateSetter = stateSetter;
        }

        /// <inheritdoc/>
        public TStateBase ActiveState { get; private set; }
        /// <inheritdoc/>
        public TContext Context { get; }
        /// <inheritdoc/>
        public IReadOnlyCollection<TStateBase> States => _stateCollection;
        /// <inheritdoc/>
        public bool IsDisposed { get; private set; }
        public TMessageHandler MessageHandler { get; } = new();

        /// <inheritdoc/>
        public event StateChangedAction<TStateBase, TContext> StateChanged;
        /// <inheritdoc/>
        public event SelfTransitionAttempedAction<TStateBase, TContext> SelfTransitionAttemped;

        private readonly StateCollection<TStateBase, TContext> _stateCollection = new();
        private readonly IStateSetter<TStateBase, TContext> _stateSetter;

        /// <inheritdoc/>
        public void SetState<TState>() where TState : TStateBase, new()
        {
            if (ActiveState is TState)
            {
                SelfTransitionAttemped?.Invoke(ActiveState);
                return;
            }
            TStateBase newState = _stateCollection.GetOrCreateState<TState>(MessageHandler);
            TStateBase previousState = ActiveState;
            MessageHandler.HandleOnExit(previousState);
            ActiveState = newState;
            MessageHandler.HandleOnEnter(ActiveState, _stateSetter);
            StateChanged?.Invoke(previousState, newState);
        }

        /// <inheritdoc/>
        public bool IsStateInitialized<TState>() where TState : TStateBase, new() => _stateCollection.IsStateInitialized<TState>();

        /// <inheritdoc/>
        public bool InitializeState<TState>() where TState : TStateBase, new() => _stateCollection.InitializeState<TState>(MessageHandler);

        /// <inheritdoc/>
        public bool DisposeState<TState>() where TState : TStateBase, new()
        {
            Type stateType = typeof(TState);
            Type currentStateType = ActiveState.GetType();
            if (stateType == currentStateType)
            {
                return false;
            }
            return _stateCollection.DisposeState(stateType, MessageHandler);
        }

        /// <inheritdoc/>
        public void DisposeInactiveStates()
        {
            HashSet<Type> types = new(_stateCollection.Types);
            Type currentStateType = ActiveState.GetType();
            _ = types.Remove(currentStateType);
            foreach (Type type in types)
            {
                _ = _stateCollection.DisposeState(type, MessageHandler);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            try
            {
                MessageHandler.HandleOnExit(ActiveState);
            }
            finally
            {
                foreach (TStateBase state in _stateCollection)
                {
                    MessageHandler.HandleOnDispose(state);
                }
                StateChanged = null;
                IsDisposed = true;
            }
        }

        /// <summary>
        /// Создаёт экземпляр <see cref="InnerFSM{TStateBase, TContext}"/> с указанным начальным состоянием и контекстом.
        /// </summary>
        /// <typeparam name="TState">Тип начального состояния. Должен иметь конструктор без параметров.</typeparam>
        /// <param name="context">Контекст, общий для всех состояний.</param>
        /// <returns>Новый экземпляр внутреннего FSM.</returns>
        public static InnerFSM<TStateBase, TContext, TMessageHandler> Instantiate<TState>(
            TContext context,
            IStateSetter<TStateBase, TContext> stateSetter)
            where TState : TStateBase, new()
        {
            InnerFSM<TStateBase, TContext, TMessageHandler> fsm = new(context, stateSetter);
            fsm.ActiveState = fsm._stateCollection.GetOrCreateState<TState>(fsm.MessageHandler);
            fsm.MessageHandler.HandleOnEnter(fsm.ActiveState, fsm);
            return fsm;
        }
    }
}
