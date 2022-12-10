
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 1000f;
    [SerializeField] private float _movementRunningSpeed = 6f;
    [SerializeField] private float _walkingMovementSpeed = 3f;
    [SerializeField] private float _gravity = -21f;
    [SerializeField] private float _jumpHeight = 1.5f;
    [SerializeField] private float _crouchingMovementSpeed = 2f;
    [SerializeField] private float _crouchHeightY = 0.5f;
    [SerializeField] private float _standingHeightY = 2f;
    [SerializeField] private float _crouchSpeed = 10f;

    public float CrouchSpeed => _crouchSpeed;
    public float StandingHeightY => _standingHeightY;
    public float CrouchHeightY => _crouchHeightY;
    public float MouseSensitivity => _mouseSensitivity;
    public float MovementRunningSpeed => _movementRunningSpeed;

    public float WalkingMovementSpeed => _walkingMovementSpeed;

    public float Gravity => _gravity;

    public float JumpHeight => _jumpHeight;
    
    public float CrouchingMovementSpeed => _crouchingMovementSpeed;
}
