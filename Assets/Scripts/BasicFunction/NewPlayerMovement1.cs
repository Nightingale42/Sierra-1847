using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NewPlayerMovement : MonoBehaviour
{
    // ---------------- MOVEMENT ----------------
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    private float moveSpeed;

    [Header("Drag")]
    public float groundDrag = 5f;

    // ---------------- JUMP ----------------
    [Header("Jump")]
    public float jumpForce = 6f;
    public float jumpCooldown = 0.25f;
    private bool readyToJump = true;

    [Header("Air Control")]
    public float airMultiplier = 0.4f;

    // ---------------- GROUND CHECK ----------------
    [Header("Ground Check")]
    public float playerHeight = 2f;
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
    private bool grounded;

    // ---------------- INPUT ----------------
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    public Transform orientation;

    // ---------------- PRIVATE ----------------
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;

    // ======================================================

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        moveSpeed = walkSpeed;
    }

    private void Update()
    {
        grounded = IsGrounded();

        HandleInput();
        HandleSprint();
        SpeedControl();

        rb.drag = grounded ? groundDrag : 0f;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // ======================================================
    // INPUT
    // ======================================================

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // ✅ Jump only once, only when grounded
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

    // ======================================================
    // MOVEMENT
    // ======================================================

    private void MovePlayer()
    {
        moveDirection =
            orientation.forward * verticalInput +
            orientation.right * horizontalInput;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
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
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    // ======================================================
    // JUMP
    // ======================================================

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    // ======================================================
    // GROUND CHECK
    // ======================================================

    private bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        float distance = (playerHeight * 0.5f) + 0.2f;

        return Physics.SphereCast(
            origin,
            groundCheckRadius,
            Vector3.down,
            out _,
            distance,
            whatIsGround
        );
    }
}
