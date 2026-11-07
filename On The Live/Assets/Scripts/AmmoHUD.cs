
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AmmoHUD : MonoBehaviour
{
    [Header("Display")]
    public Vector2 screenPadding = new Vector2(35f, 100f);
    public int fontSize = 30;

    private TextMeshProUGUI ammoText;
    private PlayerShoot playerShoot;

    void Awake()
    {
        ammoText = GetComponent<TextMeshProUGUI>();

        // Position the ammo display at the top-right.
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(1f, 1f);

        rect.anchoredPosition = new Vector2(
            -screenPadding.x, -screenPadding.y);

        rect.sizeDelta = new Vector2(300f, 60f);

        ammoText.alignment = TextAlignmentOptions.Right;
        ammoText.fontSize = fontSize;
        ammoText.fontStyle = FontStyles.Bold;
        ammoText.textWrappingMode = TextWrappingModes.NoWrap;
        ammoText.text = "AMMO -- / --";
    }

    void Start()
    {
        FindPlayer();
    }

    void FindPlayer()
    {
        PlayerShoot[] shooters =
            FindObjectsByType<PlayerShoot>();

        if (shooters.Length > 0)
            playerShoot = shooters[0];
    }

    void Update()
    {
        if (playerShoot == null)
        {
            FindPlayer();
            return;
        }

        if (playerShoot.IsReloading)
        {
            ammoText.text = "RELOADING...";
            ammoText.color = new Color(1f, 0.75f, 0.2f);
        }
        else
        {
            ammoText.text =
                "AMMO  " + playerShoot.CurrentAmmo +
                " / " + playerShoot.MagazineSize;

            ammoText.color = playerShoot.CurrentAmmo <= 3
                ? new Color(1f, 0.3f, 0.25f)
                : Color.white;
        }
    }
}
