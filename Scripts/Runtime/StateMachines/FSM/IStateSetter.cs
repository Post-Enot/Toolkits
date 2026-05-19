namespace PostEnot.Toolkits.StateMachines.FSM
{
    public interface IStateSetter<TStateBase, TContext> where TStateBase : StateBase<TStateBase, TContext>
    {
        public void SetState<TState>() where TState : TStateBase, new();
    }
}
