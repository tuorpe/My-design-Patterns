using State_Machine;
using UnityEngine;

namespace Command
{
    public class JumpCommand : ICommand
    {
        private readonly PlayerControllerSM _target;
        private readonly KeyboardPressIndicator _indicator;
        
        public JumpCommand(PlayerControllerSM target, KeyboardPressIndicator indicator = null)
        {
            _target = target;
            _indicator = indicator;
        }

        public void Execute()
        {
            _target.CurrentState.HandleCommand(this);
            _indicator?.ShowMessage("Jump!");
        }

        public void Undo()
        {
            _indicator?.ShowMessage("Jump can not be undone!");
        }
    }
}
