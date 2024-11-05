using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce = 5f; // Adjust jump force as needed
    private Animator animator;
    private FixedJoystick fixedJoystick;
    private Rigidbody rb;
    private Camera mainCamera;
    private JumpButton jumpButton;
    private AttackButton attackButton;
    public bool canAttack = true; // To check if the player can attack
    private bool isGrounded = true;
    private float initialY; // Store the initial Y position

    private void OnEnable()
    {
        fixedJoystick = FindObjectOfType<FixedJoystick>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        attackButton = FindObjectOfType<AttackButton>();
        jumpButton = FindObjectOfType<JumpButton>();
        initialY = transform.position.y;
    }

    public float animationSpeed = 1;

    private void FixedUpdate()
    {
        Move();
        Jump();
        Attack();

        UpdateAnimationSpeed();
    }

    private void Move()
    {
        float xValue = fixedJoystick.Horizontal;
        float yValue = fixedJoystick.Vertical;

        // Obtener las direcciones hacia adelante y a la derecha de la cámara, sin componente Y
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = mainCamera.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        // Crear el movimiento ajustado basado en la cámara
        Vector3 adjustedMovement = (cameraRight * xValue + cameraForward * yValue).normalized;

        // Aplicar la velocidad al Rigidbody en función de la dirección ajustada
        rb.velocity = adjustedMovement * speed;

        // Actualizar la rotación del jugador para que mire en la dirección de movimiento
        if (adjustedMovement != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(adjustedMovement);
        }

        // Actualizar parámetros de animación
        bool isMoving = xValue != 0 || yValue != 0;
        animator.SetBool("Moving", isMoving);
        animator.SetFloat("Velocity", rb.velocity.magnitude / speed);
    }

    private void Jump()
    {
        if (jumpButton.pressed && isGrounded)
        {
            isGrounded = false; // Set to false when jumping
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetInteger("Jumping", 1);
            animator.SetInteger("Trigger Number", 1);
            animator.SetTrigger("Trigger");
            rb.useGravity = true;
            canAttack = false;
        }

        // Check if the player is falling
        if (rb.velocity.y < 0 && !isGrounded)
        {
            animator.SetInteger("Jumping", 2);
            animator.SetInteger("Trigger Number", 1);
            animator.SetTrigger("Trigger");
        }

        // Check if the player has landed
        if (transform.position.y <= initialY && !isGrounded)
        {
            isGrounded = true; // Set grounded to true when player is at or below initial Y
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Reset vertical velocity
            rb.useGravity = false;
            animator.SetInteger("Jumping", 0);
            animator.SetInteger("Trigger Number", 1);
            animator.SetTrigger("Trigger");
            canAttack = true;
        }
    }

    private void Attack()
    {
        if (canAttack && attackButton.pressed)
        {
            canAttack = false; // Prevent further attacks until the current one finishes
            animator.SetInteger("Trigger Number", 2);
            animator.SetTrigger("Trigger");
        }
    }

    private void UpdateAnimationSpeed()
    {
        animator.SetFloat("Animation Speed", animationSpeed);
    }
}
