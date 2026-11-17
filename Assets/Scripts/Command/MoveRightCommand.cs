using State_Machine;
using UnityEngine;
namespace Command
{
    public class MoveRightCommand : ICommand
    {
        private readonly PlayerControllerSM _target;
        private IState _previousState;
        private Vector3 _previousPosition;
        private readonly KeyboardPressIndicator _indicator;
        
        public MoveRightCommand(PlayerControllerSM target, KeyboardPressIndicator indicator = null)
        {
            _target = target;
            _indicator = indicator;
        }

        public void Execute()
        {
            _previousState = _target.CurrentState;
            _previousPosition = _target.transform.position;
            _target.CurrentState.HandleCommand(this);
            _indicator?.ShowMessage("Move right!");
        }

        public void Undo()
        {
            _target.transform.position = _previousPosition;
            _target.ChangeState(_previousState);
            _indicator?.ShowMessage("Move right undone!");
        }
    }
}
