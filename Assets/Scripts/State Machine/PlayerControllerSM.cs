using System.Collections;
using System.Collections.Generic;
using Command;
using Observer;
using State_Machine.PlayerStates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace State_Machine
{
    public class PlayerControllerSM : StateMachineMB, IControllable
    {
        [Range(0.1f, 2f)] public float moveAmount = 1f;
        [Range(1f, 100f)] public float jumpForce = 5f;
        [Range(1f, 10f)] public float moveSpeed = 5f;
        [Range(0.01f, 1f)] public float crouchScale = 0.5f; // The scale of the player when crouching
        public float bobbingAmplitude = 0.1f; // The amplitude of the bobbing idle motion
        public float bobbingFrequency = 1f; // The frequency of the bobbing idle motion
    
        [Header("Legend Gameobjects")]
        [SerializeField] private KeyboardPressIndicator commandIndicator;
        [SerializeField] private KeyboardPressIndicator keyboardIndicator;

        [Header("Debug")] [SerializeField] private string currentState;
        // Commands
        private ICommand _commandMovement;
        private ICommand _commandUp;
        private ICommand _commandDown;
        private ICommand _commandLeft;
        private ICommand _commandRight;
        private ICommand _commandJump;
        private ICommand _commandCrouch;
        private ICommand _commandLast;
        private ICommand _commandNext;
        private ICommand _commandEsc;
    
        private Rigidbody _rb;
        // Undo history stack
        private Stack<ICommand> _undoStack;
        private Stack<ICommand> _redoStack;
        public Vector3 targetPosition;
        public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }
        public Vector3 startPosition;

        private bool _isReplaying;
        [SerializeField] private int coins;
        [SerializeField] private int score;
        [SerializeField] private int kills;

        enum PlayerState
        {
            STANDING,
            JUMPING,
            CROUCHED
        }

        [SerializeField] private PlayerState state;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _commandUp = new Command.MoveForwardCommand(this, commandIndicator);
            _commandDown = new Command.MoveBackwardCommand(this, commandIndicator);
            _commandLeft = new Command.MoveLeftCommand(this, commandIndicator);
            _commandRight = new Command.MoveRightCommand(this, commandIndicator);
            _commandCrouch = new Command.CrouchCommand(this, commandIndicator);
            //_commandSpace = new Command.JumpCommand(_rb, jumpForce, commandIndicator);
            _commandJump = new Command.JumpCommand(this, commandIndicator);
            _commandLast = new DoNothingCommand();
            _commandNext = new DoNothingCommand();
            _commandEsc = new QuitCommand(); //Command.QuitCommand();
            _undoStack = new Stack<ICommand>();
            _redoStack = new Stack<ICommand>();
            // Set the target position to the current position of the player for replay
            startPosition = transform.position;
            targetPosition = transform.position;
            if(PlaybackIndicator.instance != null)
                PlaybackIndicator.instance.ShowMessage("Recording", true);
            FindFirstObjectByType<Spawner>().ItemSpawned += OnItemSpawned;
            state = PlayerState.JUMPING;
            ChangeState(new PlayerIdle(this));
        }
    
        private void OnDisable()
        {
            var spawner = FindFirstObjectByType<Spawner>();
            if(spawner) spawner.ItemSpawned -= OnItemSpawned;
            var obstacles = FindObjectsByType<Obstacle>(FindObjectsSortMode.None);
            foreach (var obstacle in obstacles)
            {
                switch (obstacle.type)
                {
                    case ObstacleType.Enemy:
                        obstacle.OnObstacleHit -= EnemyWasHit;
                        break;
                    case ObstacleType.Coin:
                        obstacle.OnObstacleHit -= CoinWasCollected;
                        break;
                }
            }
        }
    
        // Update is called once per frame
        protected override void Update()
        {
            if(!GameManagerSM.Instance.isPlaying())
                return;
            base.Update();
            currentState = state.ToString();
            // Handle inputs & move the player
            InputHandler();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            // Set y position to the current y position of the player to allow only horizontal movement
            targetPosition.y = transform.position.y;
            // Move the player to the target position smoothly
            //transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
        }

        private void CoinWasCollected(Obstacle obstacle)
        {
            coins++;
            score += obstacle.value;
        }
    
        private void EnemyWasHit(Obstacle obstacle)
        {
            kills++;
            score += obstacle.value;
        }

        private void OnItemSpawned(Obstacle item)
        {
            switch (item.type)
            {
                case ObstacleType.Enemy:
                    item.OnObstacleHit += EnemyWasHit;
                    break;
                case ObstacleType.Coin:
                    item.OnObstacleHit += CoinWasCollected;
                    break;
            }
        }
        private void Move(Vector3 direction)
        {
            transform.Translate(direction * moveAmount);
        }

        private void Jump()
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        private void InputHandler()
        {
            if(_isReplaying)     // Do not allow keyboard input while replaying
                return;
            // State machine implementation with command pattern
            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
                CallCommand(_commandUp);
            if (Keyboard.current.downArrowKey.wasPressedThisFrame)
                CallCommand(_commandDown);
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
                CallCommand(_commandLeft);
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
                CallCommand(_commandRight);
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                CallCommand(_commandJump);
            if (Keyboard.current.cKey.wasPressedThisFrame)
                CallCommand(_commandCrouch);
            // Static Input handling
            #region Static Inputs
            // Note: The following code is commented out because we are now using the Command pattern to handle input and movement.
            // if(Keyboard.current.upArrowKey.wasPressedThisFrame) Move(Vector3.forward);
            // if(Keyboard.current.downArrowKey.wasPressedThisFrame) Move(Vector3.back);
            // if(Keyboard.current.leftArrowKey.wasPressedThisFrame) Move(Vector3.left);
            // if(Keyboard.current.rightArrowKey.wasPressedThisFrame) Move(Vector3.right);
            // Jump
            //if(Keyboard.current.spaceKey.wasPressedThisFrame) Jump();
            #endregion
            // State Machine implementation
            #region State Machine implementation
            /*switch (state)
            {
                case PlayerState.STANDING:
                    // Handle standing state
                    if (Keyboard.current.spaceKey.wasPressedThisFrame)
                    {
                        state = PlayerState.JUMPING;
                        _commandJump.Execute();
                        _commandLast = _commandJump;
                    }
                    if(Keyboard.current.cKey.wasPressedThisFrame)
                    {
                        state = PlayerState.CROUCHED;
                        _commandCrouch.Execute();
                        _commandLast = _commandCrouch;
                        Debug.Log("Crouched");
                    }
                    break;
                case PlayerState.JUMPING:
                    // Handle jumping state
                    
                    break;
                case PlayerState.CROUCHED:
                    // Handle crouched state
                    if(Keyboard.current.cKey.wasPressedThisFrame)
                    {
                        _commandStanding.Execute();
                        _commandLast = _commandStanding;
                        transform.localScale = new Vector3(1f, 1f, 1f);
                        Debug.Log("Standing");
                        state = PlayerState.STANDING;
                    }
                    break;
            }*/
            
            
            #endregion
        
            #region Command pattern implementation
            /*
            // Note: The following code is the command pattern implementation for handling input and movement.
            // Create a forwad-command for up arrow by default
            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                keyboardIndicator?.ShowMessage("Up arrow");
                CallCommand(_commandUp);
            }
            // Create a backward-command for down arrow by default
            if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                keyboardIndicator?.ShowMessage("Down arrow");
                CallCommand(_commandDown);
            }
            // Create a left-command for left arrow by default
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                keyboardIndicator?.ShowMessage("Left arrow");
                CallCommand(_commandLeft);
            }
            // Create a right-command for right arrow by default
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                keyboardIndicator?.ShowMessage("Right arrow");
                CallCommand(_commandRight);
            }
            // Create a jump-command for space key by default
            /*if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                keyboardIndicator?.ShowMessage("Space key");
                CallCommand(_commandSpace);
            }*/
            
            if(Keyboard.current.zKey.wasPressedThisFrame)
            {
                if(_undoStack.Count == 0)
                    return;
                keyboardIndicator?.ShowMessage("Undo", .5f);
                var poppedCommand = _undoStack.Pop();
                poppedCommand.Undo();
                if(poppedCommand is not DoNothingCommand)
                    _redoStack.Push(poppedCommand);
            }
            
            if(Keyboard.current.xKey.wasPressedThisFrame)
            {
                if(_redoStack.Count == 0)
                    return;
                keyboardIndicator?.ShowMessage("Redo", .5f);
                var poppedCommand = _redoStack.Pop();
                poppedCommand.Execute();
                _undoStack.Push(poppedCommand);
            }

            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                keyboardIndicator?.ShowMessage("R pressed", .5f);
                StartCoroutine(StartReplay());
            }
            
            #endregion
        
        }
    
        private void CallCommand(ICommand command)
        {
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();
        }

        private IEnumerator StartReplay()
        {
            _isReplaying = true;
            // Reset the player position to the start position
            startPosition.y = transform.position.y; // Keep the current y position to avoid falling through the ground
            targetPosition = Vector3.zero;
            transform.position = startPosition;
            var replayStack = new Stack<ICommand>(_undoStack);
            PlaybackIndicator.instance.ShowMessage("Replaying", false);
            yield return new WaitForSeconds(1f);
            // Replay the commands in the replay stack
            while (replayStack.Count > 0)
            {
                var command = replayStack.Pop();
                command.Execute();
                yield return new WaitForSeconds(0.5f);
            }
            _isReplaying = false;
            PlaybackIndicator.instance.ShowMessage("Recording", true);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Ground")) 
                return;
            if(state == PlayerState.JUMPING)
                state = PlayerState.STANDING;
            Debug.Log("Standing");
        }
        [ContextMenu("Swap Commands/ForwardBackward")]
        private void SwapForwardBackwardCommands()
        {
            (_commandUp, _commandDown) = (_commandDown, _commandUp);
        }
        [ContextMenu("Swap Commands/LeftRight")]
        private void SwapLeftRightCommands()
        {
            (_commandLeft, _commandRight) = (_commandRight, _commandLeft);
        }

        void OnDrawGizmos()
        {
            // Draw a ray downward to visualize the ground check
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector3.down * 1f);
        }
    }
    

}
