
using UnityEngine;

namespace Command
{
    public class MoveForwardCommand : ICommand
    {
        private readonly float _moveAmount;
        private readonly PlayerController _target;
        private readonly KeyboardPressIndicator _indicator;
        
        public MoveForwardCommand(PlayerController target, float moveAmount, KeyboardPressIndicator indicator = null)
        {
            _target = target;
            _moveAmount = moveAmount;
            _indicator = indicator;
        }

        public void Execute()
        {
            _target.targetPosition += Vector3.forward * _moveAmount;
            _indicator?.ShowMessage("Moved forwards!");
        }

        public void Undo()
        {
            _target.targetPosition += Vector3.back * _moveAmount;
            _indicator?.ShowMessage("Move forwards undone!");
        }
    }
}
