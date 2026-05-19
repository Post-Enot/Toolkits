using System;
using System.Collections.Generic;

namespace PostEnot.Toolkits.StateMachines.FSM
{
    /// <summary>
    /// Определяет контракт для конечного автомата (FSM) с состояниями на основе
    /// <typeparamref name="TStateBase"/> и контекстом <typeparamref name="TContext"/>.
    /// </summary>
    /// <typeparam name="TStateBase">Базовый тип состояний, унаследованный от
    /// <see cref="FSM{TStateBase, TContext}.State"/>.</typeparam>
    /// <typeparam name="TContext">Тип контекста, общего для всех состояний.</typeparam>
    public interface IFSM<TStateBase, TContext, TMessageHandler> : IDisposable, IStateSetter<TStateBase, TContext>
        where TStateBase : StateBase<TStateBase, TContext>
        where TMessageHandler : StateBase<TStateBase, TContext>.MessageHandlerBase, new()
    {
        /// <summary>
        /// Текущее активное состояние.
        /// </summary>
        public TStateBase ActiveState { get; }
        /// <summary>
        /// Контекст, общий для всех состояний.
        /// </summary>
        public TContext Context { get; }
        /// <summary>
        /// Коллекция всех инициализированных состояний.
        /// </summary>
        public IReadOnlyCollection<TStateBase> States { get; }
        /// <summary>
        /// <see langword="true"/>, если экземпляр FSM уже утилизирован; иначе <see langword="false"/>.
        /// </summary>
        public bool IsDisposed { get; }
        public TMessageHandler MessageHandler { get; }

        /// <summary>
        /// Событие изменения состояния.
        /// </summary>
        public event StateChangedAction<TStateBase, TContext> StateChanged;
        /// <summary>
        /// Событие, вызываемое при попытке перехода в состояние, которое уже является активным.
        /// </summary>
        public event SelfTransitionAttempedAction<TStateBase, TContext> SelfTransitionAttemped;

        /// <summary>
        /// Проверяет, было ли инициализировано состояние указанного типа.
        /// </summary>
        /// <typeparam name="TState">Тип проверяемого состояния.</typeparam>
        /// <returns><see langword="true"/>, если состояние уже инициализировано; иначе <see langword="false"/>.</returns>
        public bool IsStateInitialized<TState>() where TState : TStateBase, new();

        /// <summary>
        /// Инициализирует состояние указанного типа, не активируя его.
        /// </summary>
        /// <typeparam name="TState">Тип инициализируемого состояния.</typeparam>
        /// <returns><see langword="true"/>, если состояние было создано и инициализировано;
        /// <see langword="false"/>, если оно уже существует.</returns>
        public bool InitializeState<TState>() where TState : TStateBase, new();

        /// <summary>
        /// Утилизирует состояние указанного типа, если оно не является активным.
        /// </summary>
        /// <typeparam name="TState">Тип утилизируемого состояния.</typeparam>
        /// <returns><see langword="true"/>, если состояние было утилизировано;
        /// <see langword="false"/>, если оно не существовало или является активным.</returns>
        public bool DisposeState<TState>() where TState : TStateBase, new();

        /// <summary>
        /// Утилизирует все состояния, кроме текущего активного.
        /// </summary>
        public void DisposeInactiveStates();
    }
}
