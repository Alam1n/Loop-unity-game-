using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 400f;
    public float dashDistance = 5f;
    public float dashDuration = 0.15f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private PlayerInputActions inputActions;
    private float moveInput;
    private bool isGrounded;
    private bool isDashing;
    private bool jumpPressed;
    private bool dashPressed;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<float>();
        inputActions.Player.Move.canceled += ctx => moveInput = 0f;

        inputActions.Player.Jump.performed += ctx => jumpPressed = true;
        inputActions.Player.Dash.performed += ctx => dashPressed = true;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (!groundCheck)
        {
            GameObject gc = new GameObject("GroundCheck");
            gc.transform.SetParent(transform);
            gc.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheck = gc.transform;
        }
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (jumpPressed && isGrounded && !isDashing)
        {
            rb.AddForce(Vector2.up * jumpForce);
        }

        if (dashPressed && !isDashing)
        {
            StartCoroutine(Dash());
        }

        jumpPressed = false;
        dashPressed = false;
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        Vector2 newPosition = rb.position + Vector2.right * moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private System.Collections.IEnumerator Dash()
    {
        isDashing = true;
        float dashSpeed = dashDistance / dashDuration;
        float elapsed = 0f;

        Vector2 direction = new Vector2(moveInput, 0).normalized;
        if (direction == Vector2.zero) direction = Vector2.right; // default right if no input

        while (elapsed < dashDuration)
        {
            rb.MovePosition(rb.position + direction * dashSpeed * Time.fixedDeltaTime);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        isDashing = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
