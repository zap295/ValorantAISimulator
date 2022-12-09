
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 1000f;
    [SerializeField] private float _movementRunningSpeed = 6f;
    [SerializeField] private float _walkingMovementSpeed = 3f;
    [SerializeField] private float _gravity = -21f;
    [SerializeField] private float _jumpHeight = 1.5f;
    public float MouseSensitivity => _mouseSensitivity;
    public float MovementRunningSpeed => _movementRunningSpeed;

    public float WalkingMovementSpeed => _walkingMovementSpeed;

    public float Gravity => _gravity;

    public float JumpHeight => _jumpHeight;
}
