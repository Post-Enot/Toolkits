using System.Collections.Generic;

namespace PostEnot.Toolkits.StateMachines.FSM
{
    /// <summary>
    /// Предоставляет реализацию конечного автомата.
    /// </summary>
    /// <typeparam name="TStateBase">
    /// Базовый тип состояний, унаследованный от <see cref="StateBase{TStateBase, TContext}"/>.
    /// </typeparam>
    /// <typeparam name="TContext">Тип контекста, общего для всех состояний.</typeparam>
    public class FSM<TStateBase, TContext, TMessageHandler, TFSM> : IFSM<TStateBase, TContext, TMessageHandler>
        where TStateBase : StateBase<TStateBase, TContext>
        where TMessageHandler : StateBase<TStateBase, TContext>.MessageHandlerBase, new()
        where TFSM : FSM<TStateBase, TContext, TMessageHandler, TFSM>, new()
    {
        /// <inheritdoc/>
        public TContext Context => _realization.Context;
        /// <inheritdoc/>
        public TStateBase ActiveState => _realization.ActiveState;
        /// <inheritdoc/>
        public IReadOnlyCollection<TStateBase> States => _realization.States;
        /// <inheritdoc/>
        public bool IsDisposed => _realization.IsDisposed;
        public TMessageHandler MessageHandler => _realization.MessageHandler;

        /// <inheritdoc/>
        public event StateChangedAction<TStateBase, TContext> StateChanged
        {
            add => _realization.StateChanged += value;
            remove => _realization.StateChanged -= value;
        }
        /// <inheritdoc/>
        public event SelfTransitionAttempedAction<TStateBase, TContext> SelfTransitionAttemped
        {
            add => _realization.SelfTransitionAttemped += value;
            remove => _realization.SelfTransitionAttemped -= value;
        }

        private IFSM<TStateBase, TContext, TMessageHandler> _realization;

        /// <inheritdoc/>
        public void SetState<TState>() where TState : TStateBase, new() => _realization.SetState<TState>();

        /// <inheritdoc/>
        public bool IsStateInitialized<TState>() where TState : TStateBase, new() => _realization.IsStateInitialized<TState>();

        /// <inheritdoc/>
        public bool InitializeState<TState>() where TState : TStateBase, new() => _realization.InitializeState<TState>();

        /// <inheritdoc/>
        public bool DisposeState<TState>() where TState : TStateBase, new() => _realization.DisposeState<TState>();

        /// <inheritdoc/>
        public void DisposeInactiveStates() => _realization.DisposeInactiveStates();

        /// <inheritdoc/>
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            try
            {
                _realization.Dispose();
            }
            finally
            {
                _realization = new DisposedFSM<TStateBase, TContext, TMessageHandler>();
            }
        }

        /// <summary>
        /// Создаёт новый экземпляр конечного автомата с указанным начальным состоянием и контекстом.
        /// </summary>
        /// <typeparam name="TState">Тип начального состояния.</typeparam>
        /// <param name="context">Контекст, общий для всех состояний.</param>
        /// <returns>Новый экземпляр конечного автомата.</returns>
        public static TFSM Instantiate<TState>(TContext context) where TState : TStateBase, new()
        {
            TFSM fsm = new();
            fsm._realization = InnerFSM<TStateBase, TContext, TMessageHandler>.Instantiate<TState>(context, fsm);
            return fsm;
        }
    }

    public class FSM<TStateBase, TContext, TMessageHandler>
        : FSM<TStateBase, TContext, TMessageHandler, FSM<TStateBase, TContext, TMessageHandler>>
        where TStateBase : StateBase<TStateBase, TContext>
        where TMessageHandler : StateBase<TStateBase, TContext>.MessageHandlerBase, new() { }

    public class FSM<TStateBase, TContext>
        : FSM<TStateBase, TContext, StateBase<TStateBase, TContext>.MessageHandlerBase, FSM<TStateBase, TContext>>
        where TStateBase : StateBase<TStateBase, TContext> { }
}
