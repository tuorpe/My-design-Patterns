using UnityEngine;

namespace Command
{
    public class JumpCommand : ICommand
    {
        private readonly float _jumpForce;
        private readonly Rigidbody _targetRigidbody;
        private readonly KeyboardPressIndicator _indicator;
        
        public JumpCommand(Rigidbody targetRigidbody, float jumpForce, KeyboardPressIndicator indicator = null)
        {
            _targetRigidbody = targetRigidbody;
            _jumpForce = jumpForce;
            _indicator = indicator;
        }

        public void Execute()
        {
            // Check if the object is on the ground. Can still jump if at the apex
            if (_targetRigidbody.linearVelocity.y < 0.1f)
            {
                _targetRigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                _indicator?.ShowMessage("Jumped!");
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Undo()
        {
            Debug.Log("Undo can not be implemented for JumpCommand.");
            _indicator?.ShowMessage("Jump undone!!!");
        }
    }
}
