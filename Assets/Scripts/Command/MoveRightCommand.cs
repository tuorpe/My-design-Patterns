using UnityEngine;

namespace Command
{
    public class MoveRightCommand : ICommand
    {
        private readonly float _moveAmount;
        private readonly PlayerController _target;
        private readonly KeyboardPressIndicator _indicator;
        
        public MoveRightCommand(PlayerController target, float moveAmount, KeyboardPressIndicator indicator = null)
        {
            _target = target;
            _moveAmount = moveAmount;
            _indicator = indicator;
        }

        public void Execute()
        {
            _target.targetPosition += Vector3.right * _moveAmount;
            _indicator?.ShowMessage("Move right!");
        }

        public void Undo()
        {
            _target.targetPosition += Vector3.left * _moveAmount;
            _indicator?.ShowMessage("Move right undone!");
        }
    }
}
