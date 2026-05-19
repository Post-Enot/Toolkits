namespace PostEnot.Toolkits.StateMachines.HFSM
{
    public interface IStateGetter<TStateBase, TContext> where TStateBase : StateBase<TStateBase, TContext>
    {
        public TStateBase GetOrCreateState<TState>(
            StateBase<TStateBase, TContext>.MessageHandlerBase messageHandler) where TState : TStateBase, new();
    }
}
