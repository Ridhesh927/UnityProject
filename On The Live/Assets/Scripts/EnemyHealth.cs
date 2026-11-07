
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;

    [Header("Hit Feedback")]
    public float hitFlashDuration = 0.08f;

    [Header("Knockback")]
    public float knockbackForce = 2.5f;
    public float knockbackDuration = 0.12f;

    [Header("Death Effect")]
    public int deathParticleCount = 18;
    public Color deathParticleColor = new Color(1f, 0.3f, 0.08f);

    private int currentHealth;
    private bool isDead;
    private WaveManager waveManager;

    private Rigidbody2D rb;
    private float knockbackEndTime;

    private SpriteRenderer[] spriteRenderers;
    private Color[] originalColors;
    private Coroutine flashCoroutine;

    void Awake()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();

        spriteRenderers =
            GetComponentsInChildren<SpriteRenderer>(true);

        originalColors = new Color[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            originalColors[i] = spriteRenderers[i].color;
        }
    }

    public void SetWaveManager(WaveManager manager)
    {
        waveManager = manager;
    }

    public void TakeDamage(int damage)
    {
        if (isDead || damage <= 0)
            return;

        currentHealth = Mathf.Clamp(
            currentHealth - damage, 0, maxHealth);

        Debug.Log("Enemy HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(HitFlash());
    }

    IEnumerator HitFlash()
    {
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr != null)
                sr.color = Color.white;
        }

        yield return new WaitForSeconds(hitFlashDuration);

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null)
                spriteRenderers[i].color = originalColors[i];
        }

        flashCoroutine = null;
    }

    public void ApplyKnockback(Vector2 direction)
    {
        if (isDead || rb == null)
            return;

        rb.linearVelocity = direction.normalized * knockbackForce;
        knockbackEndTime = Time.time + knockbackDuration;
    }

    public bool IsBeingKnockedBack()
    {
        return !isDead && Time.time < knockbackEndTime;
    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        SpawnDeathEffect();

        if (waveManager != null)
            waveManager.RegisterKill();

        Destroy(gameObject);
    }

    void SpawnDeathEffect()
    {
        GameObject effect = new GameObject("EnemyDeathEffect");
        effect.transform.position = transform.position;

        ParticleSystem particles =
            effect.AddComponent<ParticleSystem>();

        particles.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = particles.main;
        main.duration = 0.15f;
        main.loop = false;
        main.playOnAwake = false;
        main.startLifetime = 0.4f;
        main.startSpeed = 2.2f;
        main.startSize = 0.09f;
        main.startColor = deathParticleColor;
        main.gravityModifier = 0.1f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.stopAction = ParticleSystemStopAction.Destroy;

        var emission = particles.emission;
        emission.enabled = true;
        emission.SetBursts(new[]
        {
            new ParticleSystem.Burst(
                0f,
                (short)Mathf.Clamp(deathParticleCount, 1, 100))
        });

        var shape = particles.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.12f;

        var particleRenderer =
            particles.GetComponent<ParticleSystemRenderer>();

        particleRenderer.sortingOrder = 30;

        Shader shader =
            Shader.Find("Universal Render Pipeline/Particles/Unlit");

        if (shader == null)
            shader = Shader.Find("Particles/Standard Unlit");

        if (shader != null)
            particleRenderer.material = new Material(shader);

        particles.Play();
        Destroy(effect, 2f);
    }
}
