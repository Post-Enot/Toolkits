using System;
using System.Collections.Generic;

namespace PostEnot.Toolkits.StateMachines.FSM
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
    internal sealed class DisposedFSM<TStateBase, TContext, TMessageHandler>
        : IFSM<TStateBase, TContext, TMessageHandler>
        where TStateBase : StateBase<TStateBase, TContext>
        where TMessageHandler : StateBase<TStateBase, TContext>.MessageHandlerBase, new()
    {
        /// <inheritdoc/>
        public TStateBase ActiveState => throw new ObjectDisposedException(ObjectName);
        /// <inheritdoc/>
        public TContext Context => throw new ObjectDisposedException(ObjectName);
        /// <inheritdoc/>
        public IReadOnlyCollection<TStateBase> States => throw new ObjectDisposedException(ObjectName);
        /// <inheritdoc/>
        public bool IsDisposed => true;
        public TMessageHandler MessageHandler => throw new ObjectDisposedException(ObjectName);

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

        private string ObjectName => nameof(DisposedFSM<TStateBase, TContext, TMessageHandler>);

        /// <summary>
        /// Ничего не делает, так как объект уже утилизирован.
        /// </summary>
        public void Dispose() { }

        /// <inheritdoc/>
        public void DisposeInactiveStates() => throw new ObjectDisposedException(ObjectName);

        /// <inheritdoc/>
        public bool DisposeState<TState>() where TState : TStateBase, new() => throw new ObjectDisposedException(ObjectName);

        /// <inheritdoc/>
        public bool InitializeState<TState>() where TState : TStateBase, new() => throw new ObjectDisposedException(ObjectName);

        /// <inheritdoc/>
        public bool IsStateInitialized<TState>() where TState : TStateBase, new() => throw new ObjectDisposedException(ObjectName);

        /// <inheritdoc/>
        public void SetState<TState>() where TState : TStateBase, new() => throw new ObjectDisposedException(ObjectName);
    }
}
