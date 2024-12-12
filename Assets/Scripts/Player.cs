using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform playerCamera; // —сылка на камеру игрока
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform checkGroundTransform;
    [SerializeField] private LayerMask groundMask;

    [Header("Settings")]
    [SerializeField] private float Maxstamina = 100f;
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
    private float stamina;
    public event Action<float> StaminaChanged;
    private float currentStamina;

    public float MaxHp = 100;
    public float CurrentHp;
    public event Action<float> HealthChanged;
    public DethScript dethScript;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        dethScript = FindObjectOfType<DethScript>();
        stamina = Maxstamina;
        CurrentHp = MaxHp;
    }

    void Update()
    {
        RotateCamera();
        Move();
        Velocity();
        if (stamina < 100)
        {
            stamina += 6 * Time.deltaTime;
            currentStamina = (float)stamina / Maxstamina;
            StaminaChanged.Invoke(currentStamina);
        }
    }

    private void RotateCamera()
    {
  
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime * 5;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime * 5;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90, 90);

        playerCamera.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(0, mouseX, 0);
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        move = transform.forward * moveY + transform.right * moveX;

        if (Input.GetKey(KeyCode.LeftShift) && (moveX != 0 || moveY != 0) && stamina > 0)
        {
            characterController.Move(move * speedRun * Time.deltaTime);
            stamina -= 25 * Time.deltaTime;
            currentStamina = (float) stamina / Maxstamina;
            StaminaChanged.Invoke(currentStamina);
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

        if (Input.GetKey(KeyCode.Space) && isGrounded && stamina >= 30)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            stamina -= 900 * Time.deltaTime;
            currentStamina = (float)stamina / Maxstamina;
            StaminaChanged.Invoke(currentStamina);

        }

        characterController.Move(velocity * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        CurrentHp -= damage;

        if (CurrentHp <= 0)
        {
            HealthChanged.Invoke(0);
            Cursor.lockState = CursorLockMode.None;
            dethScript.gameOver();

        }
        else
        {
            float currentHeal = (float)CurrentHp / MaxHp;
            HealthChanged.Invoke(currentHeal);
        }

    }

}
