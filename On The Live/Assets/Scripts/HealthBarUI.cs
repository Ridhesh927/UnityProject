
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image fillImage;

    void Update()
    {
        if (playerHealth == null || fillImage == null)
            return;

        float healthPercent =
            (float)playerHealth.GetCurrentHealth()
            / playerHealth.maxHealth;

        fillImage.fillAmount = healthPercent;
    }
}
