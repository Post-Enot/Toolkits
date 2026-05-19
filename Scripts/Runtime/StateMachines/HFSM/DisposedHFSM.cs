using System;
using System.Collections.Generic;

namespace PostEnot.Toolkits.StateMachines.HFSM
{
    /// <summary>
    /// Представляет реализацию конечного автомата в состоянии «утилизирован».
    /// Все члены, кроме <see cref="Dispose"/> вызывают исключение <see cref="ObjectDisposedException"/>.
    /// </summary>
    /// <typeparam name="TStateBase">
    /// Базовый тип состояний, унаследованный от <see cref="StateBase{TStateBase, TContext}"/>.
    /// </typeparam>
    /// <typeparam name="TContext">
    /// Тип контекста, общего для всех состояний.
    /// </typeparam>
    internal sealed class DisposedHFSM<TStateBase, TContext, TMessageHandler>
        : IHFSM<TStateBase, TContext, TMessageHandler>
        where TStateBase : StateBase<TStateBase, TContext>
        where TMessageHandler : StateBase<TStateBase, TContext>.MessageHandlerBase, new()
    {
        /// <inheritdoc/>
        public TContext Context => throw new ObjectDisposedException(ObjectName);
        /// <inheritdoc/>
        public TStateBase ActiveState => throw new ObjectDisposedException(ObjectName);
        /// <inheritdoc/>
        public TStateBase RootState => throw new ObjectDisposedException(ObjectName);
        /// <inheritdoc/>
        public IReadOnlyCollection<TStateBase> States => throw new ObjectDisposedException(ObjectName);
        /// <inheritdoc/>
        public IEnumerable<TStateBase> FromRootToActive => throw new ObjectDisposedException(ObjectName);
        /// <inheritdoc/>
        public IEnumerable<TStateBase> FromActiveToRoot => throw new ObjectDisposedException(ObjectName);
        public TMessageHandler MessageHandler => throw new ObjectDisposedException(ObjectName);
        /// <inheritdoc/>
        public bool IsDisposed => true;

        /// <inheritdoc/>
        public event StateChangedAction<TStateBase, TContext> StateChanged
        {
            add => throw new ObjectDisposedException(ObjectName);
            remove => throw new ObjectDisposedException(ObjectName);
        }
        /// <inheritdoc/>
        public event SelfTransitionAttempedAction<TStateBase, TContext> SelfTransitionAttemped
        {
            add => throw new ObjectDisposedException(ObjectName);
            remove => throw new ObjectDisposedException(ObjectName);
        }

        private string ObjectName => nameof(DisposedHFSM<TStateBase, TContext, TMessageHandler>);

        /// <inheritdoc/>
        public void SetState<TState>() where TState : TStateBase, new() => throw new ObjectDisposedException(ObjectName);

        /// <inheritdoc/>
        public void Dispose() { }
    }
}
