using UnityEngine;


public class MoveSquare : MonoBehaviour
{
    // The speed of your square. You can change this number!
    public float speed = 5.0f;

    void Update()
    {
        // Check if you are pressing the Arrow Keys or W/A/S/D
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Apply the movement
        transform.Translate(new Vector3(moveX, moveY, 0) * speed * Time.deltaTime);
    }
}