using Command;
using UnityEngine;
using State_Machine;
using State_Machine.PlayerStates;

public class PlayerCrouch : IState
{
    private PlayerControllerSM _target;
    private Vector3 _startScale;
    private Vector3 _endScale;
    private float _crouchDuration = 0.5f; // Duration of the crouch transition
    private float _crouchTimer = 0f; // Timer to track the crouch transition
    
    public PlayerCrouch(PlayerControllerSM target)
    {
        // Initialize the crouch state with the player controller and state machine
        _target = target;
    }
    public void Enter()
    {
        _startScale = _target.transform.localScale;
        // Calculate the end scale based on the current scale and the crouch scale defined in the player controller
        _endScale = new Vector3(_startScale.x, _startScale.y <= _target.crouchScale ? 1f : _target.crouchScale, _startScale.z);
        Debug.Log("Entered Crouching State");
    }

    public void Tick()
    {
        //
        if (_crouchTimer < _crouchDuration)
        {
            _crouchTimer += Time.deltaTime;
            var t = Mathf.Clamp01(_crouchTimer / _crouchDuration);
            _target.transform.localScale = Vector3.Lerp(_startScale, _endScale, t);
        }
        else
        {
            if (_target.transform.localScale.y >= 1f)
                _target.ChangeState(new PlayerIdle(_target));
            else
                _target.ChangeState(new PlayerDown(_target));
        }
    }

    public void FixedTick()
    {
        //
    }
    
    public void HandleCommand(ICommand command)
    {
        // When crouching we do not want to handle any commands, so we can just ignore them
    }

    public void Exit()
    {
        Debug.Log("Exited Crouching State");
    }

}
