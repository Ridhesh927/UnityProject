using UnityEngine;

public class MoveCube : MonoBehaviour
{
    public float speed = 5.0f;

    void Update()
    {
        // 1. Listen for the Keyboard
        // "Horizontal" means Left/Right Arrow keys (or A and D)
        float moveX = Input.GetAxis("Horizontal");
        
        // "Vertical" means Up/Down Arrow keys (or W and S)
        float moveZ = Input.GetAxis("Vertical");

        // 2. Calculate the direction
        // We create a new vector based on your inputs
        Vector3 moveDirection = new Vector3(moveX, 0, moveZ);

        // 3. Apply the movement
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
}