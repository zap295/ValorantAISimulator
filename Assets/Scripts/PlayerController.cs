using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   
    [SerializeField] private Camera playerCamera; 
    private float _mouseSensitivity;
    private float _xRotation = default;
    private bool _isWalking = false;
    private bool _isJumping = false;
    private bool _isGrounded = false;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask groundLayer;
    private Vector3 _jumpVelocity = Vector3.zero;

    private PlayerStats _playerStats;
    private CharacterController _characterController;
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerStats = GetComponent<PlayerStats>();
        _mouseSensitivity = _playerStats.MouseSensitivity;
        playerCamera = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        _isGrounded = Physics.CheckSphere(groundCheckTransform.position, 0.4f, groundLayer);
        HandleJumpInput();
        HandleMovement();
        HandleMouseLook();
    }

    private void HandleJumpInput()
    {
        bool isTryingToJump = Input.GetKeyDown(KeyCode.Space);
        if (isTryingToJump && _isGrounded)
        {
            _isJumping = true;
        }
        else
        {
            _isJumping = false;
        }

        if (_isGrounded && _jumpVelocity.y < 0f)
        {
            _jumpVelocity.y = -2f;
        }
        if (_isJumping)
        {
            _jumpVelocity.y = Mathf.Sqrt(_playerStats.JumpHeight * -2f * _playerStats.Gravity);
        }

        _jumpVelocity.y += _playerStats.Gravity * Time.deltaTime;
        _characterController.Move(_jumpVelocity * Time.deltaTime);
    }

    private void HandleMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        _isWalking = Input.GetKey(KeyCode.LeftShift);
        
        Vector3 movementVector = (transform.right * horizontalInput) + (transform.forward * verticalInput);
        float movementSpeed = _isWalking ? _playerStats.WalkingMovementSpeed : _playerStats.MovementRunningSpeed;
        _characterController.Move(Vector3.ClampMagnitude(movementVector,1.0f) * movementSpeed * Time.deltaTime);
       
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        
        playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0f,0f);
        transform.Rotate(Vector3.up * mouseX);
        
    }
}
