
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current == null || Camera.main == null)
            return;

        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        Vector3 mouseWorldPosition =
            Camera.main.ScreenToWorldPoint(
                new Vector3(
                    mousePosition.x,
                    mousePosition.y,
                    -Camera.main.transform.position.z
                )
            );

        Vector2 direction =
            mouseWorldPosition - transform.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            float angle =
                Mathf.Atan2(direction.y, direction.x)
                * Mathf.Rad2Deg;

            transform.rotation =
                Quaternion.Euler(0f, 0f, angle);
        }
    }
}
