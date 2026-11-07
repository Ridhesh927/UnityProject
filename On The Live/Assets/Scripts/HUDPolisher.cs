
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDPolisher : MonoBehaviour
{
    public float edgePadding = 24f;

    public Color hudTextColor = new Color(0.04f, 0.04f, 0.04f, 1f);
    public Color ammoColor = new Color(0.04f, 0.04f, 0.04f, 1f);
    public Color lowAmmoColor = new Color(0.75f, 0.05f, 0.05f, 1f);

    private TextMeshProUGUI ammoText;
    private PlayerShoot playerShoot;

    
void Start()
{
    // Find the player's weapon.
    playerShoot = FindAnyObjectByType<PlayerShoot>();

    // Reuse the existing ammo text if available.
    AmmoHUD oldAmmoHud = FindAnyObjectByType<AmmoHUD>();

    if (oldAmmoHud != null)
    {
        ammoText = oldAmmoHud.GetComponent<TextMeshProUGUI>();

        // Disable old positioning and updating logic.
        oldAmmoHud.enabled = false;
    }

    // Create an ammo label if no existing text is available.
    if (ammoText == null)
    {
        GameObject ammoObject = new GameObject(
            "AmmoCounter_Auto",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(TextMeshProUGUI));

        ammoObject.transform.SetParent(transform, false);
        ammoText = ammoObject.GetComponent<TextMeshProUGUI>();

        // Reuse the font from the Wave label.
        TMP_Text referenceText = FindHudText("WaveText");

        if (referenceText != null)
            ammoText.font = referenceText.font;
    }
    else if (!ammoText.transform.IsChildOf(transform))
    {
        ammoText.transform.SetParent(transform, false);
    }

    // Position the ammo counter at the bottom-right.
    RectTransform rect = ammoText.rectTransform;
    rect.anchorMin = new Vector2(1f, 0f);
    rect.anchorMax = new Vector2(1f, 0f);
    rect.pivot = new Vector2(1f, 0f);
    rect.anchoredPosition = new Vector2(-edgePadding, edgePadding);
    rect.sizeDelta = new Vector2(320f, 50f);
    rect.localScale = Vector3.one;

    // Style the ammo counter.
    ammoText.fontSize = 26f;
    ammoText.fontStyle = FontStyles.Bold;
    ammoText.alignment = TextAlignmentOptions.Right;
    ammoText.textWrappingMode = TextWrappingModes.NoWrap;
    ammoText.color = ammoColor;
    ammoText.raycastTarget = false;

    // Make Wave, Enemies, and Kills text black.
    TMP_Text waveText = FindHudText("WaveText");
    TMP_Text enemiesText = FindHudText("EnemiesText");
    TMP_Text killsText = FindHudText("KillsText");

    
Color darkText = new Color(0.04f, 0.04f, 0.04f, 1f);

if (waveText != null)
{
    waveText.color = darkText;
    waveText.fontSize = 28f;
    waveText.fontStyle = FontStyles.Bold;
    waveText.enableAutoSizing = false;
}

if (enemiesText != null)
{
    enemiesText.color = darkText;
    enemiesText.fontSize = 28f;
    enemiesText.fontStyle = FontStyles.Bold;
    enemiesText.enableAutoSizing = false;
}

if (killsText != null)
{
    killsText.color = darkText;
    killsText.fontSize = 28f;
    killsText.fontStyle = FontStyles.Bold;
    killsText.enableAutoSizing = false;
}


    Debug.Log("HUD styling applied: dark labels and bottom-right ammo.");
}
    

    void Update()
    {
        if (ammoText == null)
            return;

        if (playerShoot == null)
            playerShoot = FindAnyObjectByType<PlayerShoot>();

        if (playerShoot == null)
        {
            ammoText.text = "AMMO -- / --";
            return;
        }

        if (playerShoot.IsReloading)
        {
            ammoText.text = "RELOADING...";
            ammoText.color = ammoColor;
        }
        else
        {
            ammoText.text =
                "AMMO  " + playerShoot.CurrentAmmo +
                " / " + playerShoot.MagazineSize;

            ammoText.color = playerShoot.CurrentAmmo <= 3
                ? lowAmmoColor
                : ammoColor;
        }
    }

    TMP_Text FindHudText(string objectName)
    {
        foreach (TMP_Text text in GetComponentsInChildren<TMP_Text>(true))
        {
            if (text.gameObject.name == objectName)
                return text;
        }

        return null;
    }
}
