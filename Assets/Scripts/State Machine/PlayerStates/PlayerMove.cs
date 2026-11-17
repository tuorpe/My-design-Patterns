using State_Machine;
using State_Machine.PlayerStates;
using UnityEngine;

public class PlayerMove: IState
{
    private Vector3 _newPosition;
    private Vector3 _previousPosition;
    private PlayerControllerSM _target;
    private float lerpAmount;
    private float _speed;

    public PlayerMove(PlayerControllerSM target, Vector2 dir)
    {
        _target = target;
        _newPosition = new Vector3(dir.x, 0, dir.y);
    }
    public void Enter()
    {
        Debug.Log("Entered Move State");
        _previousPosition = _target.transform.position;
        _newPosition = _previousPosition + _newPosition;
        lerpAmount = 0f;
    }

    public void Tick()
    {
        lerpAmount += _target.moveSpeed * Time.deltaTime;
        _target.transform.position = Vector3.Lerp(_previousPosition, _newPosition, lerpAmount);
        if(lerpAmount >= 1f)    // Check if the movement is complete
            _target.RevertState();
    }

    public void FixedTick()
    {
        //
    }

    public void HandleCommand(Command.ICommand command)
    {
        // When moving we do not want to handle any commands, so we can just ignore them
    }
    public void Exit()
    {
        Debug.Log("Exited Move State");
    }
}
