
using UnityEngine;

[DisallowMultipleComponent]
public class CharacterVisualSizer : MonoBehaviour
{
    [Header("Character Appearance")]
    [Min(1f)]
    public float sizeMultiplier = 3f;

    private void Awake()
    {
        // Enlarge the character sprite.
        // Compensate for the collider changes so physics stays consistent.
        transform.localScale *= sizeMultiplier;

        foreach (Collider2D col in GetComponents<Collider2D>())
        {
            if (col is BoxCollider2D box)
            {
                box.size /= sizeMultiplier;
                box.offset /= sizeMultiplier;
            }
            else if (col is CapsuleCollider2D capsule)
            {
                capsule.size /= sizeMultiplier;
                capsule.offset /= sizeMultiplier;
            }
            else if (col is CircleCollider2D circle)
            {
                circle.radius /= sizeMultiplier;
                circle.offset /= sizeMultiplier;
            }
        }
    }
}
