using UnityEngine;
using State_Machine;
namespace Command
{
    public class CrouchCommand : ICommand
    {
        private readonly PlayerControllerSM _target;
        private IState _previousState;
        private readonly KeyboardPressIndicator _indicator;
        
        public CrouchCommand(PlayerControllerSM target, KeyboardPressIndicator indicator = null)
        {
            _indicator = indicator;
            _target = target;
        }

        public void Execute()
        {
            _previousState = _target.CurrentState;
            _target.CurrentState.HandleCommand(this);
            _indicator?.ShowMessage("Crouch toggled!");
        }

        public void Undo()
        {
            _target.ChangeState(_previousState);
            _indicator?.ShowMessage("Crouch toggle undone!");
        }
    }
}
