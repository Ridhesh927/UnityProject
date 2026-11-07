
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

   
void Update()
{
    movement = Vector2.zero;

    if (Keyboard.current == null)
        return;

    if (Keyboard.current.wKey.isPressed ||
        Keyboard.current.upArrowKey.isPressed)
        movement.y += 1f;

    if (Keyboard.current.sKey.isPressed ||
        Keyboard.current.downArrowKey.isPressed)
        movement.y -= 1f;

    if (Keyboard.current.dKey.isPressed ||
        Keyboard.current.rightArrowKey.isPressed)
        movement.x += 1f;

    if (Keyboard.current.aKey.isPressed ||
        Keyboard.current.leftArrowKey.isPressed)
        movement.x -= 1f;

    movement = movement.normalized;

    // Apply movement using your existing speed variable.
    Vector3 position = transform.position;
    position += (Vector3)(movement * speed * Time.deltaTime);

    // Keep the player inside the playable area.
    position.x = Mathf.Clamp(position.x, -7f, 7f);
    position.y = Mathf.Clamp(position.y, -4f, 4f);

    transform.position = position;
}


    void FixedUpdate()
    {
        rb.MovePosition(
            rb.position + movement * speed * Time.fixedDeltaTime
        );
    }
}
