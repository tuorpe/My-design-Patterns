using System;
using Command;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace State_Machine.PlayerStates
{
    public class PlayerIdle: IState
    {

        State_Machine.PlayerControllerSM _target;
        private float _bobbingAmplitude; // The amplitude of the bobbing motion
        private float _bobbingFrequency; // The frequency of the bobbing motion
        public PlayerIdle(PlayerControllerSM _target)
        {
            this._target = _target;
            _bobbingAmplitude = _target.bobbingAmplitude;
            _bobbingFrequency = _target.bobbingFrequency;
        }

        public void Enter()
        {
            Debug.Log("Entered Idle State");
        }

        public void Tick()
        {
            // Update the idle state
            _target.transform.localScale =  Vector3.one + Vector3.up * (Mathf.Sin(Time.time * _bobbingFrequency) * _bobbingAmplitude); 
        }

        public void FixedTick()
        {
            //
        }

        public void Exit()
        {
            Debug.Log("Exited Idle State");
        }

        public void HandleCommand(ICommand command)
        {
            // Check if the command is allowed in the idle state
            switch (command)
            {
                case CrouchCommand crouchCommand:
                    _target.ChangeState(new PlayerCrouch(_target));
                    break;
                case JumpCommand jumpCommand:
                    _target.ChangeState(new PlayerJump(_target));
                    break;
                case MoveBackwardCommand backwardCommand:
                    _target.ChangeState(new PlayerMove(_target, Vector2.down));
                    break;
                case MoveForwardCommand forwardCommand:
                    _target.ChangeState(new PlayerMove(_target, Vector2.up));
                    break;
                case MoveLeftCommand leftCommand:
                    _target.ChangeState(new PlayerMove(_target, Vector2.left));
                    break;
                case MoveRightCommand rightCommand:
                    _target.ChangeState(new PlayerMove(_target, Vector2.right));
                    break;
                case QuitCommand:
                    break;
                default:
                    // Handle other commands or ignore them
                    break;
            }
        }
    }
}
