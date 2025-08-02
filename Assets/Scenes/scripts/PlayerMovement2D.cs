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

    private bool tryPickup = false;

    private PickupItem heldItem;

    private float lastXDir = 1f; // default to right
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
            if (moveInput.x != 0)
                lastXDir = Mathf.Sign(moveInput.x); // stores -1 or 1
        };
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;


        inputActions.Player.Jump.performed += ctx => jumpQueued = true;
        inputActions.Player.Dash.performed += ctx => dashQueued = true;
        inputActions.Player.Interact.performed += ctx => tryPickup = true;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Start()
    {
        rb.freezeRotation = true;
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
            Debug.Log("JUMP!");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (dashQueued && !isDashing)
        {
            StartCoroutine(DashRoutine());
        }

        if (tryPickup)
        {
            TryPickupNearbyItem();
            tryPickup = false;
        }

        jumpQueued = false;
        dashQueued = false;
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        float moveX = moveInput.x;
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;
        float dashSpeed = dashDistance / dashDuration;
        float elapsed = 0f;

        Vector2 dashDir = new Vector2(lastXDir, 0f);

        while (elapsed < dashDuration)
        {
            rb.linearVelocity = dashDir * dashSpeed;
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

    void TryPickupNearbyItem()
    {
        if (heldItem != null) return; // Don't pick up another if already holding one

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.5f);
        foreach (var hit in hits)
        {
            var pickup = hit.GetComponent<PickupItem>();
            if (pickup != null)
            {
                pickup.AttachToPlayer(transform.Find("HoldPoint"));
                heldItem = pickup; // Keep track of it
                break;
            }
        }
    }

    public void DropItem()
    {
        
        if (heldItem != null)
        {
            heldItem.transform.parent = null;
            heldItem.transform.position = transform.position + transform.forward * 1f;


            Rigidbody rb = heldItem.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;

            Collider col = heldItem.GetComponent<Collider>();
            col.enabled = true;

            heldItem = null;
        }
    }


}

