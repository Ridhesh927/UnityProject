
using UnityEngine;
using UnityEditor;
using TMPro;

public class ArrangeArenaHUD
{
    [MenuItem("Tools/ON THE LIVE/Arrange HUD")]
    public static void ArrangeHUD()
    {
        Canvas canvas = Object.FindAnyObjectByType<Canvas>(
            FindObjectsInactive.Include
        );

        if (canvas == null)
        {
            Debug.LogError("No Canvas found.");
            return;
        }

        RectTransform healthBar = FindUI(
            canvas.transform, "HealthBarBackground");
        RectTransform wave = FindUI(canvas.transform, "WaveText");
        RectTransform enemies = FindUI(canvas.transform, "EnemiesText");
        RectTransform kills = FindUI(canvas.transform, "KillsText");

        if (healthBar == null || wave == null ||
            enemies == null || kills == null)
        {
            Debug.LogError("Check HUD object names in the Hierarchy.");
            return;
        }

        SetRect(healthBar, 20, -25, 250, 25);

        RectTransform fill = FindUI(
            healthBar, "HealthBarFill");

        if (fill != null)
        {
            Undo.RecordObject(fill, "Arrange Health Fill");
            fill.anchorMin = Vector2.zero;
            fill.anchorMax = Vector2.one;
            fill.offsetMin = Vector2.zero;
            fill.offsetMax = Vector2.zero;
            EditorUtility.SetDirty(fill);
        }

        // Separate the three labels by 40 units.
        SetLabel(wave,    20, -65);
        SetLabel(enemies, 20, -105);
        SetLabel(kills,   20, -145);

        Debug.Log("ON THE LIVE HUD layout fixed.");
    }

    private static RectTransform FindUI(
        Transform root, string objectName)
    {
        foreach (RectTransform item in
            root.GetComponentsInChildren<RectTransform>(true))
        {
            if (item.name == objectName)
                return item;
        }

        return null;
    }

    private static void SetRect(
        RectTransform rect, float x, float y,
        float width, float height)
    {
        Undo.RecordObject(rect, "Arrange HUD");

        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = new Vector2(x, y);
        rect.sizeDelta = new Vector2(width, height);

        EditorUtility.SetDirty(rect);
    }

    private static void SetLabel(
        RectTransform rect, float x, float y)
    {
       SetRect(rect, x, y, 320, 40);
       
        TMP_Text text = rect.GetComponent<TMP_Text>();

        if (text != null)
        {
            Undo.RecordObject(text, "Format HUD Text");
            text.alignment = TextAlignmentOptions.TopLeft;
             text.textWrappingMode = TextWrappingModes.NoWrap;
            text.fontSize = 28;
            EditorUtility.SetDirty(text);
        }
    }
}
