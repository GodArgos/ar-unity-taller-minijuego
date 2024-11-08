using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce = 5f; // Adjust jump force as needed
    [SerializeField] private float attackRange = 5f;
    private Animator animator;
    private FixedJoystick fixedJoystick;
    private Rigidbody rb;
    private Camera mainCamera;
    private AttackButton attackButton;
    public bool canAttack = true; // To check if the player can attack

    private void OnEnable()
    {
        fixedJoystick = FindObjectOfType<FixedJoystick>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        attackButton = FindObjectOfType<AttackButton>();
    }

    public float animationSpeed = 1;

    private void FixedUpdate()
    {
        if (fixedJoystick != null && attackButton != null)
        {
            if (canAttack)
            {
                Move();
            }
            
            Attack();

            UpdateAnimationSpeed();
        }
        else
        {
            fixedJoystick = FindObjectOfType<FixedJoystick>();
            attackButton = FindObjectOfType<AttackButton>();
        }
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

    private void Attack()
    {
        if (canAttack && attackButton.pressed)
        {
            canAttack = false; // Prevent further attacks until the current one finishes
            animator.SetInteger("Trigger Number", 2);
            animator.SetTrigger("Trigger");
        }
    }

    public void AttackQuery()
    {
        // Define the radius of the sphere
        float sphereRadius = 0.025f;

        // Calculate the position of the sphere in front of the player
        Vector3 spherePosition = transform.position;

        // Set the Y position to half of the player's Y scale
        spherePosition.y = transform.position.y + (GetComponent<CapsuleCollider>().bounds.size.y / 2);

        // Perform the sphere cast
        RaycastHit hit;
        if (Physics.SphereCast(spherePosition, sphereRadius, transform.forward, out hit, attackRange))
        {
            BreakOnHit breakable = hit.transform.GetComponent<BreakOnHit>();
            if (breakable != null)
            {
                breakable.Hit(hit.point);
            }
        }
    }

    private void UpdateAnimationSpeed()
    {
        animator.SetFloat("Animation Speed", animationSpeed);
    }

    private void OnDrawGizmos()
    {
        // Calculate the position of the sphere in front of the player
        Vector3 spherePosition = transform.position + transform.forward * attackRange;

        // Set the Y position to half of the player's Y scale
        spherePosition.y = transform.position.y + (GetComponent<CapsuleCollider>().bounds.size.y / 2);

        // Draw the sphere for visual debugging
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(spherePosition, 0.025f);
    }
}
