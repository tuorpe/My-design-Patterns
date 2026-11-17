using Command;
using UnityEngine;
using State_Machine;
using State_Machine.PlayerStates;

public class PlayerJump: IState
{
    private PlayerControllerSM _target;
    private Rigidbody _rb;
    private bool _isJumping = false;
    private float groundCheckDistance = 1f; // Distance to check for ground
    public PlayerJump(PlayerControllerSM target)
    {
        // Initialize the jump state with the player controller and state machine
        _target = target;
    }

    public void Enter()
    {
        Debug.Log("Entered Jump State");
        _rb = _target.GetComponent<Rigidbody>();
        if (!_rb) 
            return;
        _rb.AddForce(Vector3.up * _target.jumpForce, ForceMode.Impulse);
    }

    public void Tick()
    {
        if (IsGrounded())
        {
            if (!_isJumping)    // If the player has not yet jumped enough to leave the ground, we can just return and wait for the next frame
                return; 
            _isJumping = false;
            _target.RevertState();
        }
        else    // If the player is not grounded anymore, set the jumping flag to true to wait for the next time they land on the ground
            _isJumping = true;
    }

    private bool IsGrounded()
    {
        // Check if the player is grounded by casting a ray downward
        return Physics.Raycast(_target.transform.position, Vector3.down, groundCheckDistance);
    }
    public void FixedTick()
    {
        //
    }

    public void HandleCommand(ICommand command)
    {
        // When jumping we do not want to handle any commands, so we can just ignore them
    }
    public void Exit()
    {
        Debug.Log("Exited Jump State");
    }
    
}
