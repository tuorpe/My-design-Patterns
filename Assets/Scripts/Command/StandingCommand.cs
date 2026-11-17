using UnityEngine;

namespace Command
{
    public class StandingCommand : ICommand
    {
        private readonly Transform _target;
        private readonly KeyboardPressIndicator _indicator;
        
        public StandingCommand(Transform target, float jumpForce, KeyboardPressIndicator indicator = null)
        {
            _target = target;
            _indicator = indicator;
        }

        public void Execute()
        {
            _target.localScale = new Vector3(_target.localScale.x, _target.localScale.y * 2, _target.localScale.z);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Undo()
        {
            _target.localScale = new Vector3(_target.localScale.x, _target.localScale.y / 2, _target.localScale.z);
            _indicator?.ShowMessage("Standing undone!!!");
        }
    }
}
