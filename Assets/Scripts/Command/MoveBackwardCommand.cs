using State_Machine;
using UnityEngine;
namespace Command
{
    public class MoveBackwardCommand : ICommand
    {
        private Vector2 moveDirection;
        public float _moveAmount;
        private readonly PlayerControllerSM _target;
        private IState _previousState;
        private Vector3 _previousPosition;
        private readonly KeyboardPressIndicator _indicator;
        
        public MoveBackwardCommand(PlayerControllerSM target, KeyboardPressIndicator indicator = null)
        {
            _target = target;
            _indicator = indicator;
        }

        public void Execute()
        {
            _previousState = _target.CurrentState;
            _previousPosition = _target.transform.position;
            _target.CurrentState.HandleCommand(this);
            _indicator?.ShowMessage("Move backward!");
        }

        public void Undo()
        {
            _target.transform.position = _previousPosition;
            _target.ChangeState(_previousState);
            _indicator?.ShowMessage("Move backward undone!");
        }
    }
}
