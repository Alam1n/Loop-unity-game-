using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float dashDistance = 5f;
    public float dashDuration = 0.15f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private PlayerInputActions inputActions;

    private Vector2 moveInput;
    private bool isGrounded;
    private bool isDashing;
    private bool jumpQueued;
    private bool dashQueued;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => jumpQueued = true;
        inputActions.Player.Dash.performed += ctx => dashQueued = true;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Start()
    {
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

        if (jumpQueued && isGrounded && !isDashing)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (dashQueued && !isDashing)
        {
            StartCoroutine(DashRoutine());
        }

        jumpQueued = false;
        dashQueued = false;
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        float moveX = moveInput.x;
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;
        float dashSpeed = dashDistance / dashDuration;
        float elapsed = 0f;

        Vector2 dashDir = moveInput.x == 0 ? Vector2.right : new Vector2(moveInput.x, 0).normalized;

        while (elapsed < dashDuration)
        {
            rb.velocity = dashDir * dashSpeed;
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
