using System;
using Command;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace State_Machine.PlayerStates
{
    public class PlayerDown: IState
    {

        State_Machine.PlayerControllerSM _target;
        private float _bobbingAmplitude; // The amplitude of the bobbing motion
        private float _bobbingFrequency; // The frequency of the bobbing motion
        public PlayerDown(PlayerControllerSM _target)
        {
            this._target = _target;
            _bobbingAmplitude = _target.bobbingAmplitude;
            _bobbingFrequency = _target.bobbingFrequency;
        }

        public void Enter()
        {
            Debug.Log("Entered Crouching State");
        }

        public void Tick()
        {
            // Nothing to do here, just wait for the player to be revived
        }

        public void FixedTick()
        {
            //
        }

        public void Exit()
        {
            Debug.Log("Exited Crouching State");
        }

        public void HandleCommand(ICommand command)
        {
            // Check if the command is allowed in the idle state
            switch (command)
            {
                case CrouchCommand crouchCommand:
                    _target.ChangeState(new PlayerCrouch(_target));
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
            }
        }
    }
}
