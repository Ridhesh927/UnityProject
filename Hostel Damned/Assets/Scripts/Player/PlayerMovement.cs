using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;


    private Rigidbody2D rb;
    private bool isGrounded;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckGround();
        HandleMovement();
        HandleJump();
    }
    private void CheckGround()
{
    isGrounded = Physics2D.OverlapCircle(
        groundCheck.position,
        groundCheckRadius,
        LayerMask.GetMask("Default")
    );
}

    private void HandleMovement()
{
    float horizontalInput = 0f;

    if (Keyboard.current.aKey.isPressed)
    {
        horizontalInput = -1f;
    }
    else if (Keyboard.current.dKey.isPressed)
    {
        horizontalInput = 1f;
    }

    rb.linearVelocity = new Vector2(
        horizontalInput * moveSpeed,
        rb.linearVelocity.y
    );

    FlipPlayer(horizontalInput);
}
    private void FlipPlayer(float horizontalInput)
{
    if (horizontalInput > 0)
    {
        transform.localScale = new Vector3(-0.8f, 1.5f, 1f);
    }
    else if (horizontalInput < 0)
    {
        transform.localScale = new Vector3(0.8f, 1.5f, 1f);
    }
}
    private void HandleJump()
{
    if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
    {
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            jumpForce
        );
    }
}
}