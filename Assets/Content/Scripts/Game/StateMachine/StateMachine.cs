public class StateMachine {
    public BaseState state;

    public void SetState(BaseState newState) {
        if(newState != state) {
            state?.ExitState();
            state = newState;
            state.Initialize();
            state.EnterState();
        }
    }
}