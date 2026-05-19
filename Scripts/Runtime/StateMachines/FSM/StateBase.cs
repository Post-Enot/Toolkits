using System;

namespace PostEnot.Toolkits.StateMachines.FSM
{
    /// <summary>
    /// Базовый абстрактный класс для всех состояний конечного автомата.
    /// </summary>
    /// <typeparam name="TStateBase">Конкретный тип состояния, унаследованный от <see cref="StateBase{TStateBase, TContext}"/>.
    /// Используется для обеспечения типобезопасности переходов.</typeparam>
    /// <typeparam name="TContext">Тип контекста, общего для всех состояний.</typeparam>
    public abstract class StateBase<TStateBase, TContext> where TStateBase : StateBase<TStateBase, TContext>
    {
        public class MessageHandlerBase
        {
            public TContext Context { get; internal set; }

            public virtual void HandleOnInit(TStateBase state)
            {
                state.Context = Context;
                state.OnInit();
            }

            public virtual void HandleOnEnter(TStateBase state, IStateSetter<TStateBase, TContext> stateSetter)
            {
                state._stateSetter = stateSetter;
                state.OnEnter();
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

        /// <summary>
        /// Контекст, общий для всех состояний.
        /// </summary>
        public TContext Context { get; private set; }

        private IStateSetter<TStateBase, TContext> _stateSetter;

        /// <summary>
        /// Вызывается при инициализации состояния.
        /// Выполнение перехода (<see cref="TransitTo{TState}"/>) в данном методе невозможно.
        /// </summary>
        protected virtual void OnInit() { }

        /// <summary>
        /// Вызывается при активации состояния.
        /// </summary>
        protected virtual void OnEnter() { }

        /// <summary>
        /// Вызывается при деактивации состояния.
        /// Выполнение перехода (<see cref="TransitTo{TState}"/>) в данном методе
        /// приведёт к исключению <see cref="InvalidOperationException"/>.
        /// </summary>
        protected virtual void OnExit() { }

        /// <summary>
        /// Вызывается при утилизации состояния.
        /// Выполнение перехода (<see cref="TransitTo{TState}"/>) в данном методе
        /// приведёт к исключению <see cref="InvalidOperationException"/>.
        /// </summary>
        protected virtual void OnDispose() { }

        /// <summary>
        /// Выполняет переход к другому состоянию. Должен вызываться только когда
        /// состояние активно (между <see cref="OnEnter"/> и <see cref="OnExit"/>).
        /// </summary>
        /// <typeparam name="TState">Тип целевого состояния. Должен иметь конструктор без параметров.</typeparam>
        /// <exception cref="InvalidOperationException">Если состояние не привязано к активному FSM.</exception>
        protected virtual void TransitTo<TState>() where TState : TStateBase, new()
        {
            if (_stateSetter == null)
            {
                throw new InvalidOperationException(
                            "Cannot perform transition because the state is not attached to an FSM. " +
                            $"Ensure that {nameof(TransitTo)} is called only after the state has been entered " +
                            "(OnEnter) and before it is exited (OnExit).");
            }
            _stateSetter.SetState<TState>();
        }
    }
}
