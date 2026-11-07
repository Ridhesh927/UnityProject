
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;

    [Header("Contact Damage")]
    public int contactDamage = 10;
    public float damageCooldown = 1f;

    private Transform player;
    private Rigidbody2D rb;
    private EnemyHealth health;
    private float nextDamageTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();
    }

    void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("EnemyAI: Player tag is missing!");
        }
    }

    void FixedUpdate()
    {
        if (player == null || rb == null)
            return;

        // Let physics move the enemy during knockback.
        if (health != null && health.IsBeingKnockedBack())
            return;

        Vector2 direction =
            ((Vector2)player.position - rb.position).normalized;

        rb.MovePosition(
            rb.position + direction * speed * Time.fixedDeltaTime
        );
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (Time.time < nextDamageTime)
            return;

        PlayerHealth playerHealth =
            collision.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(contactDamage);
            nextDamageTime = Time.time + damageCooldown;
        }
    }
}
