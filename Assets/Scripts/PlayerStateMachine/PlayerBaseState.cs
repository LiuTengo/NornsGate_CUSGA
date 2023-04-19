public abstract class PlayerBaseState
{
    private bool _isRootState = false;
    private PlayerStateMachine _ctx;
    private PlayerStateFactory _factory;
    private PlayerBaseState _currentSuperState;
    private PlayerBaseState _currentSubState;

    protected bool IsRootState { set { _isRootState = value; } }
    protected PlayerStateMachine Ctx { get { return _ctx; } }
    protected PlayerStateFactory Factory { get { return _factory; } }

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }

    public abstract void EnterState();

    public abstract void UpdataState();

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    public abstract void InitializeSubState();

    public void UpdataStates()
    {
        
        UpdataState();
        
        if(_currentSubState != null)
        {
            _currentSubState.UpdataStates();
        }
        
        
    }

    /*
    public void ExitStates()
    {
        ExitState();

        if (_currentSubState != null)
        {
            _currentSubState.ExitState();
        }
    }
    */

    protected void SwitchState(PlayerBaseState newState)
    {
        // current state exits state
        ExitState();

        // new state enter state
        newState.EnterState();
        
        if (_isRootState)
        {
            // switch current state of context
            _ctx.CurrentState = newState;
        }
        
        else if (_currentSuperState != null)
        {
            // set the current super states sub state to the new state
            _currentSuperState.SetSubState(newState);
        }
        
    }

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
