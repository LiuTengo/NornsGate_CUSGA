using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private GameController _gameController;

    public CameraController _cameraController;

    public PlayerInput playerInput;
    CharacterController _characterController;
    Animator _animator;

    string _isWalkingHash = "isWalk";
    string _isRunningHash = "isRun";
    string _isJumpingHash = "isJump";

    // variables to store player input values
    Vector2 _currentMovementInput;
    Vector3 _currentMovement;
    Vector3 _currentRunMovement;
    Vector3 _appliedMovement;
    Vector3 _cameraRelativeMovement;

    bool _isMovementPressed = false;
    bool _isRunPressed = false;
    bool _isJumpPressed = false;

    // contants
    float _rotationFactorPerFrame = 15f;
    public float _normalMoveSpeed = 1.3f;
    public float _runMoveSpeed = 3f;
//    int zero = 0;

    // _gravity variables
    float _gravity = -9.8f;

    // jumping variables
    public float MaxJumpTime = 0.5f;
    public float MaxJumpHeight = 1f;
    float _initialJumpVelocity;
    public bool _requireNewJumpPress = false;
    string _jumpCountHash = "jumpCount";
    string _isFallingHash = "isFalling";

    public int _jumpCount = 0;
    Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
    Coroutine _currentJumpResetRoutine = null;

    // state variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    // getters and setters
    public CharacterController CharacterController { get { return _characterController; } }
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Animator Animator { get { return _animator; } }
    public Coroutine CurrentJumpResetRoutine { get { return _currentJumpResetRoutine; } set { _currentJumpResetRoutine = value; } }
    public Dictionary<int, float> InitialJumpVelocities { get { return _initialJumpVelocities; } }
    public Dictionary<int, float> JumpGravities { get { return _jumpGravities; } }
    public Vector2 CurrentMovementInput { get { return _currentMovementInput; } }
    public string JumpCountHash { get { return _jumpCountHash; } }
    public int JumpCount{ get { return _jumpCount; } set { _jumpCount = value; } }
    public string IsJumpingHash { get { return _isJumpingHash; } }
    public string IsWalkingHash { get { return _isWalkingHash; } }
    public string IsFallingHash { get { return _isFallingHash; } }
    public string IsRunningHash { get { return _isRunningHash; } }
    public bool IsJumpPressed { get { return _isJumpPressed; } }
    public bool IsMovementPressed { get { return _isMovementPressed; } }
    public bool IsRunPressed { get { return _isRunPressed; } }
    public bool RequireNewJumpPress { get { return _requireNewJumpPress; } set { _requireNewJumpPress = value; } }
    public float Gravity { get { return _gravity; } }
    public float CurrentMovementX { get { return _currentMovement.x; } set { _currentMovement.x = value; } }
    public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }
    public float CurrentMovementZ { get { return _currentMovement.z; } set { _currentMovement.z = value; } }
    public float AppliedMovementZ { get { return _appliedMovement.z; } set { _appliedMovement.z = value; } }
    public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
    public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }

    public float NormalMoveSpeed { get { return _normalMoveSpeed; } }
    public float RunMoveSpeed { get { return _runMoveSpeed; } }

    void Awake()
    {
        playerInput = new PlayerInput();
        _animator = GetComponentInChildren<Animator>();
        _characterController = GetComponent<CharacterController>();

        // set the parameter hash references
        // _isFallingHash = Animator.StringToHash("isFalling");

        // setup state
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
        
        playerInput.CharacterController.Move.started += OnMovementInput;
        playerInput.CharacterController.Move.canceled += OnMovementInput;
        playerInput.CharacterController.Move.performed += OnMovementInput;

        playerInput.CharacterController.Run.started += OnRun;
        playerInput.CharacterController.Run.canceled += OnRun;

        playerInput.CharacterController.Jump.started += OnJump;
        playerInput.CharacterController.Jump.canceled += OnJump;

        SetupJumpVariables();
    }

    void SetupJumpVariables()
    {
        float timeToApex = MaxJumpTime / 2;
        float initialGravity = (-2 * MaxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * MaxJumpHeight) / timeToApex;

        float secondJumpGravity = (-2 * (MaxJumpHeight + 0)) / Mathf.Pow(timeToApex * 1f, 2);
        float secnodJumpVelocity = (2 * (MaxJumpHeight + 0)) / (timeToApex * 1.25f);
        float thirdJumpGravity = (-2 * (MaxJumpHeight + 0)) / Mathf.Pow(timeToApex * 1f, 2);
        float thirdJumpVelocity = (2 * (MaxJumpHeight + 0)) / (timeToApex * 1f);

        _initialJumpVelocities.Add(1, _initialJumpVelocity);
        _initialJumpVelocities.Add(2, secnodJumpVelocity);
        _initialJumpVelocities.Add(3, thirdJumpVelocity);

        _jumpGravities.Add(0, initialGravity);
        _jumpGravities.Add(1, initialGravity);
        _jumpGravities.Add(2, secondJumpGravity);
        _jumpGravities.Add(3, thirdJumpGravity);
    }

    Vector3 ConvertToCameraSpace(Vector3 vectorTORotate)
    {
        float currentYvalue = vectorTORotate.y;
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 cameraForwardZProduct = vectorTORotate.z * cameraForward;
        Vector3 cameraRightXProduct = vectorTORotate.x * cameraRight;

        Vector3 vectorRotateToCameraSpace = cameraForwardZProduct + cameraRightXProduct;
        vectorRotateToCameraSpace.y = currentYvalue;
        return vectorRotateToCameraSpace;
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = _cameraRelativeMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = _cameraRelativeMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (_isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        _characterController.Move(_appliedMovement * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        // Need to fix (It now conflicts with the code for the camera./)
        HandleRotation();
        _currentState.UpdataStates();

         _cameraRelativeMovement = ConvertToCameraSpace(_appliedMovement);

        _characterController.Move(_cameraRelativeMovement * Time.deltaTime);


    }

    //callback handler function for jump buttons
    void OnJump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
        _requireNewJumpPress = false;
    }

    //callback handler function for run buttons
    void OnRun(InputAction.CallbackContext context)
    {
        _isRunPressed = context.ReadValueAsButton();
    }

    //callback hander function to set the player input values
    void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.z = _currentMovementInput.y * NormalMoveSpeed;
        _currentRunMovement.z = _currentMovementInput.y * RunMoveSpeed;
        _currentMovement.x = _currentMovementInput.x * NormalMoveSpeed;
        _currentRunMovement.x = _currentMovementInput.x * RunMoveSpeed;

        if(_cameraController._camPos == 1)
            _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
        if (_cameraController._camPos == 2)
            _isMovementPressed = _currentMovementInput.x != 0;
        if (_cameraController._camPos == 3)
            _isMovementPressed =  _currentMovementInput.x != 0;


    }

    public void OnEnable()
    {
        playerInput.CharacterController.Enable();
    }

    public void OnDisable()
    {
        playerInput.CharacterController.Disable();
    }

}
