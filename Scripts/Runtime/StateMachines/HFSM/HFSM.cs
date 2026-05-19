using System.Collections.Generic;

namespace PostEnot.Toolkits.StateMachines.HFSM
{
    public class HFSM<TStateBase, TContext, TMessageHandler, THFSM>
        : IHFSM<TStateBase, TContext, TMessageHandler>
        where TStateBase : StateBase<TStateBase, TContext>
        where TMessageHandler : StateBase<TStateBase, TContext>.MessageHandlerBase, new()
        where THFSM : HFSM<TStateBase, TContext, TMessageHandler, THFSM>, new()
    {
        public TContext Context => _realization.Context;
        public TStateBase ActiveState => _realization.ActiveState;
        public TStateBase RootState => _realization.RootState;
        public IEnumerable<TStateBase> FromRootToActive => _realization.FromRootToActive;
        public IEnumerable<TStateBase> FromActiveToRoot => _realization.FromActiveToRoot;
        public IReadOnlyCollection<TStateBase> States => _realization.States;
        public TMessageHandler MessageHandler => _realization.MessageHandler;
        public bool IsDisposed => _realization.IsDisposed;

        public event StateChangedAction<TStateBase, TContext> StateChanged
        {
            add => _realization.StateChanged += value;
            remove => _realization.StateChanged -= value;
        }
        public event SelfTransitionAttempedAction<TStateBase, TContext> SelfTransitionAttemped
        {
            add => _realization.SelfTransitionAttemped += value;
            remove => _realization.SelfTransitionAttemped -= value;
        }

        private IHFSM<TStateBase, TContext, TMessageHandler> _realization;

        public void SetState<TState>() where TState : TStateBase, new() => _realization.SetState<TState>();

        public void Dispose()
        {
            try
            {
                _realization.Dispose();
            }
            finally
            {
                _realization = new DisposedHFSM<TStateBase, TContext, TMessageHandler>();
            }
        }

        /// <summary>
        /// Создаёт новый экземпляр конечного автомата с заданным контекстом и начальным состоянием.
        /// </summary>
        /// <typeparam name="TState">Тип начального состояния.</typeparam>
        /// <param name="context">Контекст, общий для всех состояний.</param>
        /// <returns>Новый экземпляр конечного автомата.</returns>
        public static THFSM Instantiate<TState>(TContext context) where TState : TStateBase, new()
        {
            THFSM hfsm = new();
            hfsm._realization = InnerHFSM<TStateBase, TContext, TMessageHandler>.Instantiate<TState>(context, hfsm);
            return hfsm;
        }
    }

    public class HFSM<TStateBase, TContext, TMessageHandler>
        : HFSM<TStateBase, TContext, TMessageHandler, HFSM<TStateBase, TContext, TMessageHandler>>
        where TStateBase : StateBase<TStateBase, TContext>
        where TMessageHandler : StateBase<TStateBase, TContext>.MessageHandlerBase, new() { }

    public class HFSM<TStateBase, TContext>
        : HFSM<TStateBase, TContext, StateBase<TStateBase, TContext>.MessageHandlerBase, HFSM<TStateBase, TContext>>
        where TStateBase : StateBase<TStateBase, TContext> { }
}
