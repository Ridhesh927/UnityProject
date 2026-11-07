
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [Header("Weapon")]
    public GameObject bulletPrefab;
    public float fireRate = 0.25f;
    public float spawnDistance = 0.6f;

    [Header("Ammo")]
    public int magazineSize = 12;
    public float reloadTime = 1.5f;
    public bool autoReloadWhenEmpty = true;

    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading;

    public int CurrentAmmo => currentAmmo;
    public int MagazineSize => magazineSize;
    public bool IsReloading => isReloading;

    void Awake()
    {
        currentAmmo = magazineSize;
    }

    void Update()
    {
        if (Mouse.current == null || Keyboard.current == null)
            return;

        // Press R to reload.
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            StartReload();
        }

        // Don't shoot while reloading.
        if (isReloading)
            return;

        // Shoot while holding the left mouse button.
        if (Mouse.current.leftButton.isPressed &&
            Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                Shoot();
                currentAmmo--;
                nextFireTime = Time.time + fireRate;

                Debug.Log("Ammo: " + currentAmmo + "/" + magazineSize);

                if (currentAmmo <= 0 && autoReloadWhenEmpty)
                {
                    StartReload();
                }
            }
            else if (autoReloadWhenEmpty)
            {
                StartReload();
            }
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("PlayerShoot: Bullet Prefab is not assigned.");
            return;
        }

        Vector3 spawnPosition =
            transform.position +
            transform.right * spawnDistance;

        Instantiate(
            bulletPrefab,
            spawnPosition,
            transform.rotation
        );
    }

    public void StartReload()
    {
        if (isReloading || currentAmmo >= magazineSize)
            return;

        StartCoroutine(ReloadRoutine());
    }

    IEnumerator ReloadRoutine()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magazineSize;
        isReloading = false;

        Debug.Log("Reload complete! Ammo: " +
                  currentAmmo + "/" + magazineSize);
    }
}
