using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NewPlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    private float moveSpeed;

    [Header("Ground Drag")]
    public float groundDrag = 5f;

    [Header("Jumping")]
    public float jumpForce = 6f;
    public float jumpCooldown = 0.25f;
    private bool readyToJump = true;

    [Header("Air Control")]
    public float airMultiplier = 0.4f;

    [Header("Gravity")]
    public float extraGravity = 20f;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    private bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle = 45f;
    private RaycastHit slopeHit;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;

        Cursor.lockState = CursorLockMode.Locked;
        moveSpeed = walkSpeed;
    }

    private void Update()
    {
        grounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            playerHeight * 0.5f + 0.3f,
            whatIsGround
        );

        HandleInput();
        HandleSprint();
        SpeedControl();

        // ✔ Ground drag restored (only when grounded)
        rb.drag = grounded ? groundDrag : 0f;
    }

    private void FixedUpdate()
    {
        // Extra gravity = weighty falling (does NOT fight drag)
        rb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);

        MovePlayer();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Jump only on key down (no flying)
        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void HandleSprint()
    {
        moveSpeed = Input.GetKey(sprintKey) ? sprintSpeed : walkSpeed;
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // On slope
        if (grounded && OnSlope())
        {
            Vector3 slopeDirection =
                Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;

            rb.AddForce(slopeDirection * moveSpeed * 10f, ForceMode.Force);

            // Keep player glued to slope
            rb.AddForce(Vector3.down * 30f, ForceMode.Force);
        }
        // Flat ground
        else if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        // In air
        else
        {
            rb.AddForce(
                moveDirection.normalized * moveSpeed * 10f * airMultiplier,
                ForceMode.Force
            );
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(
                limitedVelocity.x,
                rb.velocity.y,
                limitedVelocity.z
            );
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(
            transform.position,
            Vector3.down,
            out slopeHit,
            playerHeight * 0.5f + 0.5f
        ))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle > 0f && angle <= maxSlopeAngle;
        }
        return false;
    }
}
