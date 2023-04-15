    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    PlayerInput playerInput;
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
    public float NormalMoveSpeed = 1.3f;
    public float RunMoveSpeed = 3f;
    //    int zero = 0;

    // _gravity variables
    float _gravity = -9.8f;
    float _groundedGravity = -.05f;

    // jumping variables
    public float MaxJumpTime = 0.5f;
    public float MaxJumpHeight = 1f;
    float _initialJumpVelocity;
    bool _isJumping = false;
    bool _isJumpAnimating = false;

    string _jumpCountHash = "jumpCount";
    public int _jumpCount = 0;
    Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
    Coroutine _currentJumpResetRoutine = null;

    void Awake()
    {
        playerInput = new PlayerInput();
        _animator = GetComponentInChildren<Animator>();
        _characterController = GetComponent<CharacterController>();

        playerInput.CharacterController.Move.started += OnMovementInput;
        playerInput.CharacterController.Move.canceled += OnMovementInput;
        playerInput.CharacterController.Move.performed += OnMovementInput;

        playerInput.CharacterController.Run.started += OnRun;
        playerInput.CharacterController.Run.canceled += OnRun;

        playerInput.CharacterController.Jump.started += OnJump;
        playerInput.CharacterController.Jump.canceled += OnJump;

        SetupJumpVariables();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
    }

    void OnRun(InputAction.CallbackContext context)
    {
        _isRunPressed = context.ReadValueAsButton();
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x * NormalMoveSpeed;
        _currentMovement.z = _currentMovementInput.y * NormalMoveSpeed;

        _currentRunMovement.x = _currentMovementInput.x * RunMoveSpeed;
        _currentRunMovement.z = _currentMovementInput.y * RunMoveSpeed;
        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
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

    void HandleGravity()
    {
        bool isFalling = _currentMovement.y <= 0.0f || !_isJumpPressed;
        float fallMutiplier = 2.0f;

        if (_characterController.isGrounded)
        {
            if (_isJumpAnimating)
            {
                _animator.SetBool(_isJumpingHash, false);
                _isJumpAnimating = false;
                _currentJumpResetRoutine = StartCoroutine(jumpResetRoutine());

                if (_jumpCount >= 3)
                {
                    _jumpCount = 0;
                    _animator.SetInteger(_jumpCountHash, _jumpCount);
                }
            }
            _currentMovement.y = _groundedGravity;
            _appliedMovement.y = _groundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = _currentMovement.y;
            _currentMovement.y = _currentMovement.y + (_jumpGravities[_jumpCount] * fallMutiplier * Time.deltaTime);
            _appliedMovement.y = Mathf.Max((_currentMovement.y + previousYVelocity) * .5f, -20f);
        }
        else
        {
            float previousYVelocity = _currentMovement.y;
            _currentMovement.y = _currentMovement.y + (_jumpGravities[_jumpCount] * Time.deltaTime);
            _appliedMovement.y = (_currentMovement.y + previousYVelocity) * .5f;
        }
    }

    void HandleAnimation()
    {
        bool isWalking = _animator.GetBool(_isWalkingHash);
        bool isRunning = _animator.GetBool(_isRunningHash);

        if (_isMovementPressed && !isWalking)
        {
            _animator.SetBool(_isWalkingHash, true);
        }
        else if (!_isMovementPressed && isWalking)
        {
            _animator.SetBool(_isWalkingHash, false);
        }

        if ((_isRunPressed && _isMovementPressed) && !isRunning)
        {
            _animator.SetBool(_isRunningHash, true);
        }
        else if ((!_isRunPressed || !_isMovementPressed) && isRunning)
        {
            _animator.SetBool(_isRunningHash, false);
        }
    }

    void HandleJump()
    {
        if (!_isJumping && _characterController.isGrounded && _isJumpPressed)
        {
            if (_jumpCount < 3 && _currentJumpResetRoutine != null)
            {
                StopCoroutine(_currentJumpResetRoutine);
            }
            _isJumping = true;
            _animator.SetBool(_isJumpingHash, true);
            _isJumpAnimating = true;
            _jumpCount += 1;
            _animator.SetInteger(_jumpCountHash, _jumpCount);
            _currentMovement.y = _initialJumpVelocities[_jumpCount];
            _appliedMovement.y = _initialJumpVelocities[_jumpCount];
        }
        else if (!_isJumpPressed && _isJumping && _characterController.isGrounded)
        {
            _isJumping = false;
        }
    }

    IEnumerator jumpResetRoutine()
    {
        yield return new WaitForSeconds(.5f);
        _jumpCount = 0;
    }

    void SetupJumpVariables()
    {
        float timeToApex = MaxJumpTime / 2;
        _gravity = (-2 * MaxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * MaxJumpHeight) / timeToApex;

        float secondJumpGravity = (-2 * (MaxJumpHeight + 0)) / Mathf.Pow(timeToApex * 1f, 2);
        float secnodJumpVelocity = (2 * (MaxJumpHeight + 0)) / (timeToApex * 1f);
        float thirdJumpGravity = (-2 * (MaxJumpHeight + 0) / Mathf.Pow(timeToApex * 1f, 2));
        float thirdJumpVelocity = (2 * (MaxJumpHeight + 0) / (timeToApex * 1f));

        _initialJumpVelocities.Add(1, _initialJumpVelocity);
        _initialJumpVelocities.Add(2, secnodJumpVelocity);
        _initialJumpVelocities.Add(3, thirdJumpVelocity);

        _jumpGravities.Add(0, _gravity);
        _jumpGravities.Add(1, _gravity);
        _jumpGravities.Add(2, secondJumpGravity);
        _jumpGravities.Add(3, thirdJumpGravity);
    }

    // Start is called before the first frame update
    void Start()
    {
        _characterController.Move(_appliedMovement * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        HandleAnimation();
        HandleRotation();

        if (_isRunPressed)
        {
            _appliedMovement.x = _currentRunMovement.x;
            _appliedMovement.z = _currentRunMovement.z;
        }
        else
        {
            _appliedMovement.x = _currentMovement.x;
            _appliedMovement.z = _currentMovement.z;
        }

        _cameraRelativeMovement = ConvertToCameraSpace(_appliedMovement);
        _characterController.Move(_cameraRelativeMovement * Time.deltaTime);

        HandleGravity();
        HandleJump();
    }

    private void OnEnable()
    {
        playerInput.CharacterController.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterController.Disable();
    }
}
