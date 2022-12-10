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
    private bool _isCrouching = false;
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
        _isCrouching = Input.GetKey(KeyCode.LeftControl);

        if (_isCrouching)
        {
            HandleCrouch();
        }
        else
        {
            HandleStand();
        }

        Vector3 movementVector = (transform.right * horizontalInput) + (transform.forward * verticalInput);
        if (_isCrouching)
        {
            _characterController.Move(Vector3.ClampMagnitude(movementVector, 1.0f) * _playerStats.CrouchingMovementSpeed * Time.deltaTime);
        }
        else if (_isWalking)
        {
            _characterController.Move(Vector3.ClampMagnitude(movementVector, 1.0f) * _playerStats.WalkingMovementSpeed * Time.deltaTime);
        }
        else
        {
            _characterController.Move(Vector3.ClampMagnitude(movementVector, 1.0f) * _playerStats.MovementRunningSpeed * Time.deltaTime);
        }
    }

    private void HandleCrouch()
    {
        if (_characterController.height > _playerStats.CrouchHeightY)
        {
            UpdateCharacterHeight(_playerStats.CrouchHeightY);
            if (_characterController.height - 0.05f <= _playerStats.CrouchHeightY)
            {
                _characterController.height = _playerStats.CrouchHeightY;
            }
        }
    }

    private void UpdateCharacterHeight(float newHeight)
    {
        _characterController.height = Mathf.Lerp(_characterController.height, newHeight,
            _playerStats.CrouchSpeed * Time.deltaTime);
    }

    private void HandleStand()
    {
        if (_characterController.height < _playerStats.StandingHeightY)
        {
            float lastHeight = _characterController.height;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.up, out hit, _playerStats.StandingHeightY))
            {
                if (hit.distance < _playerStats.StandingHeightY - _playerStats.CrouchHeightY)
                {
                    UpdateCharacterHeight(_playerStats.CrouchHeightY + hit.distance);
                    return;
                }
                else
                {
                    UpdateCharacterHeight(_playerStats.StandingHeightY);
                }
            }
            else
            {
                UpdateCharacterHeight(_playerStats.StandingHeightY);
            }

            if (_characterController.height + 0.05f >= _playerStats.StandingHeightY)
            {
                _characterController.height = _playerStats.StandingHeightY;
            }

            transform.position += new Vector3(0, (_characterController.height - lastHeight) / 2, 0);
        }
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}