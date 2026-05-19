namespace PostEnot.Toolkits.StateMachines.FSM
{
    /// <summary>
    /// Представляет метод, вызываемый при изменении состояния конечного автомата.
    /// </summary>
    /// <typeparam name="TStateBase">Базовый тип состояний конечного автомата.</typeparam>
    /// <typeparam name="TContext">Тип контекста, общего для всех состояний.</typeparam>
    /// <param name="previousState">Состояние, из которого выполняется переход.</param>
    /// <param name="newState">Состояние, в которое выполняется переход.</param>
    public delegate void StateChangedAction<TStateBase, TContext>(TStateBase previousState, TStateBase newState)
        where TStateBase : StateBase<TStateBase, TContext>;

    /// <summary>
    /// Представляет метод, вызываемый при попытке перехода в текущее активное состояние (самопереход).
    /// </summary>
    /// <typeparam name="TStateBase">Базовый тип состояний конечного автомата.</typeparam>
    /// <typeparam name="TContext">Тип контекста, общего для всех состояний.</typeparam>
    /// <param name="state">Состояние, в которое пытались перейти (оно же текущее активное состояние).</param>
    public delegate void SelfTransitionAttempedAction<TStateBase, TContext>(TStateBase state)
        where TStateBase : StateBase<TStateBase, TContext>;
}
