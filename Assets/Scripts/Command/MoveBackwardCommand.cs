using UnityEngine;

namespace Command
{
    public class MoveBackwardCommand : ICommand
    {
        private readonly float _moveAmount;
        private readonly PlayerController _target;
        private readonly KeyboardPressIndicator _indicator;
        public MoveBackwardCommand(PlayerController target, float moveAmount, KeyboardPressIndicator indicator = null)
        {
            _target = target;
            _moveAmount = moveAmount;
            _indicator = indicator;
        }

        public void Execute()
        {
            _target.targetPosition += Vector3.back * _moveAmount;
            _indicator?.ShowMessage("Move backward!");
        }

        public void Undo()
        {
            _target.targetPosition += Vector3.forward * _moveAmount;
            _indicator?.ShowMessage("Move backward undone!");
        }
    }
}
