using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform playerCamera; // Ссылка на камеру игрока
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform checkGroundTransform;
    [SerializeField] private LayerMask groundMask;

    [Header("Settings")]
    [SerializeField] private float checkRadiusSphere = 0.2f;
    [SerializeField] private float gravity = -14f;
    [SerializeField] private float speed = 4F;
    [SerializeField] private float speedRun = 7f;
    [SerializeField] private float jumpHeight = 1f;
    [Range(1, 100)]
    [SerializeField] private float sensitivity = 50f;

    private float rotationX;
    Vector3 move;
    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        RotateCamera();
        Move();
        Velocity();

    }

    private void RotateCamera()
    {
  
        // Получаем движение мыши
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime * 5;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime * 5;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90, 90);

        // Применяем поворот к игроку и камере
        playerCamera.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(0, mouseX, 0);
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        move = transform.forward * moveY + transform.right * moveX;

        if (Input.GetKey(KeyCode.LeftShift) && (moveX != 0 || moveY != 0))
        {
            characterController.Move(move * speedRun * Time.deltaTime);
        }
        else 
        {
            characterController.Move(move * speed * Time.deltaTime);
        }

    }

    private void Velocity()
    {
        isGrounded = Physics.CheckSphere(checkGroundTransform.position, checkRadiusSphere, groundMask);

        if (isGrounded && velocity.y < 0) 
        {
            velocity.y = -2f;
        }

        velocity.y += Time.deltaTime * gravity;

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        characterController.Move(velocity * Time.deltaTime);
    }
}
