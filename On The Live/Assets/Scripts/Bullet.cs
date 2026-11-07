
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 12f;
    public float lifetime = 2f;
    public int damage = 1;

    [Header("Visual Trail")]
    public bool enableTrail = true;
    public float trailTime = 0.12f;
    public float trailWidth = 0.07f;
    public Color trailColor = new Color(0.2f, 0.85f, 1f, 1f);

    private Rigidbody2D rb;
    private bool hasHit;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.linearVelocity = transform.right * speed;
        Destroy(gameObject, lifetime);

        if (enableTrail)
            CreateTrail();
    }

    void CreateTrail()
    {
        TrailRenderer trail = gameObject.AddComponent<TrailRenderer>();

        trail.time = trailTime;
        trail.startWidth = trailWidth;
        trail.endWidth = 0f;
        trail.startColor = trailColor;
        trail.endColor = new Color(
            trailColor.r, trailColor.g, trailColor.b, 0f);

        trail.minVertexDistance = 0.02f;
        trail.numCapVertices = 2;
        trail.sortingLayerName = "Default";
        trail.sortingOrder = 25;

        Shader shader = Shader.Find("Sprites/Default");

        if (shader != null)
            trail.material = new Material(shader);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit || other.CompareTag("Player"))
            return;

        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();

        if (enemy != null)
{
    hasHit = true;

    Vector2 knockbackDirection =
        (enemy.transform.position - transform.position).normalized;

    enemy.TakeDamage(damage);
    enemy.ApplyKnockback(knockbackDirection);

    Destroy(gameObject);
}
        // Preserve the existing behavior: bullets disappear on other
        // trigger contacts too.
        hasHit = true;
        Destroy(gameObject);
    }
}
