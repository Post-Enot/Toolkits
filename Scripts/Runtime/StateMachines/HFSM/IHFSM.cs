using System;
using System.Collections.Generic;

namespace PostEnot.Toolkits.StateMachines.HFSM
{
    public interface IHFSM<TStateBase, TContext, TMessageHandler>
        : IStateSetter<TStateBase, TContext>, IDisposable
        where TStateBase : StateBase<TStateBase, TContext>
        where TMessageHandler : StateBase<TStateBase, TContext>.MessageHandlerBase, new()
    {
        public TContext Context { get; }
        public TStateBase ActiveState { get; }
        public TStateBase RootState { get; }
        public IEnumerable<TStateBase> FromRootToActive { get; }
        public IEnumerable<TStateBase> FromActiveToRoot { get; }
        public IReadOnlyCollection<TStateBase> States { get; }
        public TMessageHandler MessageHandler { get; }
        public bool IsDisposed { get; }

        public event StateChangedAction<TStateBase, TContext> StateChanged;
        public event SelfTransitionAttempedAction<TStateBase, TContext> SelfTransitionAttemped;
    }
}
