
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIStyler : MonoBehaviour
{
    [Header("Title")]
    public string titleObjectName = "GameOverText";
    public int titleFontSize = 52;
    public Color titleColor = new Color(0f, 0.85f, 1f, 1f);

    [Header("Restart Button")]
    public string restartButtonName = "Restart";
    public int buttonFontSize = 26;
    public Color buttonTextColor = Color.white;

    public Color buttonColor = new Color(0.04f, 0.12f, 0.22f, 1f);

    void Start()
    {
        StyleTitle();
        StyleRestartButton();
    }

    void StyleTitle()
    {
        TMP_Text title = FindText(titleObjectName);

        if (title == null)
        {
            Debug.LogWarning(
                "GameOverUIStyler: Couldn't find title text named " +
                titleObjectName);
            return;
        }

        
    title.fontSize = titleFontSize;
    title.fontStyle = FontStyles.Bold;
title.color = new Color(0f, 0.85f, 1f, 1f); // Bright cyan
title.faceColor = new Color32(0, 217, 255, 255);
title.alpha = 1f;
title.alignment = TextAlignmentOptions.Center;
title.textWrappingMode = TextWrappingModes.NoWrap;
title.fontMaterial.SetColor(
    ShaderUtilities.ID_FaceColor,
    new Color(0f, 0.85f, 1f, 1f)
);

        RectTransform rect = title.rectTransform;
        rect.sizeDelta = new Vector2(
            Mathf.Max(rect.sizeDelta.x, 650f),
            Mathf.Max(rect.sizeDelta.y, 90f));
    }

    void StyleRestartButton()
    {
        Button button = FindButton(restartButtonName);

        if (button == null)
        {
            Debug.LogWarning(
                "GameOverUIStyler: Couldn't find Button named " +
                restartButtonName);
            return;
        }

        Image background = button.GetComponent<Image>();

        if (background != null)
{
    background.color = new Color(0.04f, 0.12f, 0.22f, 1f);
}

        TMP_Text label = button.GetComponentInChildren<TMP_Text>(true);

        
if (label != null)
{
    label.fontSize = buttonFontSize;
    label.fontStyle = FontStyles.Bold;
    label.color = Color.white;
    label.faceColor = Color.white;
    label.alpha = 1f;
    label.alignment = TextAlignmentOptions.Center;
    label.textWrappingMode = TextWrappingModes.NoWrap;
}

        RectTransform buttonRect = button.GetComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(240f, 68f);

        ColorBlock colors = button.colors;
        colors.normalColor = buttonColor;
        colors.highlightedColor = new Color(0f, 0.85f, 1f, 1f);
        colors.pressedColor = new Color(0.03f, 0.4f, 0.6f, 1f);
        colors.selectedColor = colors.highlightedColor;
        button.colors = colors;
    }

    TMP_Text FindText(string objectName)
    {
        foreach (TMP_Text text in FindObjectsByType<TMP_Text>())
        {
            if (text.gameObject.name == objectName ||
                text.text == "YOU WERE ELIMINATED")
                return text;
        }

        return null;
    }

    Button FindButton(string objectName)
    {
        foreach (Button button in FindObjectsByType<Button>())
        {
            if (button.gameObject.name == objectName ||
                button.GetComponentInChildren<TMP_Text>(true)?.text == "RESTART")
                return button;
        }

        return null;
    }
}
