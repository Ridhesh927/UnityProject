
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelCompleteUI : MonoBehaviour
{
    [Header("References")]
    public WaveManager waveManager;

    [Header("Colors")]
    public Color overlayColor = new Color(0.015f, 0.04f, 0.09f, 0.88f);
    public Color accentColor = new Color(0.05f, 0.85f, 1f, 1f);
    public Color buttonColor = new Color(0.02f, 0.35f, 0.5f, 1f);

    private GameObject panel;
    private TMP_Text subtitleText;
    private bool hasShown;
    private float previousTimeScale = 1f;

    void Start()
    {
        if (waveManager == null)
            waveManager = FindAnyObjectByType<WaveManager>();

        if (waveManager == null)
        {
            Debug.LogError("LevelCompleteUI: WaveManager not found.");
            enabled = false;
            return;
        }

        CreatePanel();
    }

    void Update()
    {
        if (!hasShown && waveManager.LevelComplete)
            ShowCompletion();
    }

    void CreatePanel()
    {
        // Full-screen dark overlay.
        panel = new GameObject(
            "LevelCompletePanel",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Image));

        panel.transform.SetParent(transform, false);

        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        Image overlay = panel.GetComponent<Image>();
        overlay.color = overlayColor;
        overlay.raycastTarget = true;

        // Find a TMP font already used by the HUD.
        TMP_FontAsset font = TMP_Settings.defaultFontAsset;

        foreach (TMP_Text existingText in
                 GetComponentsInChildren<TMP_Text>(true))
        {
            if (existingText.font != null)
            {
                font = existingText.font;
                break;
            }
        }

        // Main title.
        TMP_Text title = CreateText(
            "LevelCompleteTitle",
            "LEVEL 1 COMPLETE!",
            font,
            48,
            accentColor,
            new Vector2(0.5f, 0.5f),
            new Vector2(0f, 95f),
            new Vector2(800f, 90f));

        title.fontStyle = FontStyles.Bold;

        // Subtitle.
        subtitleText = CreateText(
            "LevelCompleteSubtitle",
            "ALL WAVES CLEARED",
            font,
            24,
            Color.white,
            new Vector2(0.5f, 0.5f),
            new Vector2(0f, 35f),
            new Vector2(700f, 50f));

        // Next Level button.
        GameObject buttonObject = new GameObject(
            "NextLevelButton",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Image),
            typeof(Button));

        buttonObject.transform.SetParent(panel.transform, false);

        RectTransform buttonRect =
            buttonObject.GetComponent<RectTransform>();

        buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
        buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
        buttonRect.pivot = new Vector2(0.5f, 0.5f);
        buttonRect.anchoredPosition = new Vector2(0f, -45f);
        buttonRect.sizeDelta = new Vector2(280f, 65f);

        Image buttonImage = buttonObject.GetComponent<Image>();
        buttonImage.color = buttonColor;

        Button button = buttonObject.GetComponent<Button>();
        ColorBlock buttonColors = button.colors;
        buttonColors.normalColor = buttonColor;
        buttonColors.highlightedColor = accentColor;
        buttonColors.pressedColor = new Color(0.02f, 0.2f, 0.3f, 1f);
        button.colors = buttonColors;

        TMP_Text buttonLabel = CreateText(
            "ButtonLabel",
            "NEXT LEVEL",
            font,
            25,
            Color.white,
            new Vector2(0.5f, 0.5f),
            Vector2.zero,
            new Vector2(270f, 60f));

        buttonLabel.transform.SetParent(buttonObject.transform, false);

        RectTransform labelRect = buttonLabel.rectTransform;
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;
        buttonLabel.fontStyle = FontStyles.Bold;

        button.onClick.AddListener(GoToNextLevel);

        // Keep the panel hidden until the final wave is cleared.
        panel.SetActive(false);
    }

    TMP_Text CreateText(
        string objectName,
        string content,
        TMP_FontAsset font,
        float fontSize,
        Color color,
        Vector2 anchor,
        Vector2 position,
        Vector2 size)
    {
        GameObject textObject = new GameObject(
            objectName,
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(TextMeshProUGUI));

        textObject.transform.SetParent(panel.transform, false);

        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        TMP_Text text = textObject.GetComponent<TextMeshProUGUI>();
        text.text = content;
        text.font = font;
        text.fontSize = fontSize;
        text.color = color;
        text.alignment = TextAlignmentOptions.Center;
        text.textWrappingMode = TextWrappingModes.NoWrap;
        text.raycastTarget = false;

        return text;
    }

    void ShowCompletion()
    {
        hasShown = true;
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        panel.SetActive(true);

        Debug.Log("LEVEL 1 COMPLETE — all waves defeated!");
    }

    void GoToNextLevel()
    {
        // Level 2 will be connected once its scene exists.
        if (Application.CanStreamedLevelBeLoaded("Level2"))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Level2");
        }
        else
        {
            subtitleText.text =
                "LEVEL 2 IS NEXT — SCENE NOT CREATED YET";

            Debug.Log(
                "Create the Level2 scene and add it to Build Settings " +
                "to enable level progression.");
        }
    }

    void OnDestroy()
    {
        Time.timeScale = previousTimeScale;
    }
}
