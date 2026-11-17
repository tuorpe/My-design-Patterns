using State_Machine;
using UnityEngine;

namespace Command
{
    public class MoveLeftCommand : ICommand
    {
        private readonly PlayerControllerSM _target;
        private IState _previousState;
        private Vector3 _previousPosition;
        private readonly KeyboardPressIndicator _indicator;
        
        public MoveLeftCommand(PlayerControllerSM target, KeyboardPressIndicator indicator = null)
        {
            _target = target;
            _indicator = indicator;
        }

        public void Execute()
        {
            _previousState = _target.CurrentState;
            _previousPosition = _target.transform.position;
            _target.CurrentState.HandleCommand(this);
            _indicator?.ShowMessage("Move left!");
        }

        public void Undo()
        {
            _target.transform.position = _previousPosition;
            _target.ChangeState(_previousState);
            _indicator?.ShowMessage("Move left undone!");
        }
    }
}
