
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Level2EnvironmentDecorator : EditorWindow
{
    private const string EnvironmentPath = "Assets/Sprites/Environment";
    private const string GroupName = "Level2_EnvironmentDecorations";

    [MenuItem("Tools/ON THE LIVE/Decorate Level 2")]
    static void OpenWindow()
    {
        GetWindow<Level2EnvironmentDecorator>("Level 2 Decorator");
    }

    void OnGUI()
    {
        GUILayout.Label("LEVEL 2 - HAZARD ZONE",
            EditorStyles.boldLabel);

        EditorGUILayout.HelpBox(
            "Automatically places existing Kenney rocks, plants and crystals " +
            "around the arena edges. Decorations have no colliders.",
            MessageType.Info);

        if (GUILayout.Button("BUILD LEVEL 2 DECORATIONS",
            GUILayout.Height(40)))
        {
            BuildDecorations();
        }

        if (GUILayout.Button("REMOVE LEVEL 2 DECORATIONS"))
        {
            RemoveDecorations();
        }
    }

    void BuildDecorations()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog(
                "Stop Play Mode",
                "Stop Play mode before decorating the scene.",
                "OK");
            return;
        }

        GameObject arena = GameObject.Find("ArenaFloor");

        if (arena == null)
        {
            EditorUtility.DisplayDialog(
                "Arena Not Found",
                "Create the Level 2 Arena first. No ArenaFloor was found.",
                "OK");
            return;
        }

        List<Sprite> sprites = new List<Sprite>();
        string[] guids = AssetDatabase.FindAssets(
            "t:Sprite", new[] { EnvironmentPath });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

            if (sprite != null)
                sprites.Add(sprite);
        }

        if (sprites.Count == 0)
        {
            EditorUtility.DisplayDialog(
                "No Sprites Found",
                "No Sprite assets were found in " + EnvironmentPath,
                "OK");
            return;
        }

        sprites.Sort((a, b) => string.Compare(
            a.name, b.name, System.StringComparison.Ordinal));

        Transform oldGroup = arena.transform.Find(GroupName);

        if (oldGroup != null)
        {
            if (!EditorUtility.DisplayDialog(
                "Replace Decorations",
                "Replace the existing Level 2 decoration group?",
                "Replace", "Cancel"))
                return;

            Undo.DestroyObjectImmediate(oldGroup.gameObject);
        }

        GameObject group = new GameObject(GroupName);
        Undo.RegisterCreatedObjectUndo(group, "Build Level 2 Decorations");
        group.transform.SetParent(arena.transform, false);

        // Positions are in ArenaFloor local space.
        // Keep the center clear for combat.
        Vector2[] positions =
        {
            new Vector2(-6.5f,  3.5f),
            new Vector2(-5.5f,  3.6f),
            new Vector2(-6.4f,  2.6f),

            new Vector2( 5.5f,  3.6f),
            new Vector2( 6.5f,  3.5f),
            new Vector2( 6.4f,  2.5f),

            new Vector2(-6.5f, -3.5f),
            new Vector2(-5.5f, -3.6f),
            new Vector2(-6.4f, -2.6f),

            new Vector2( 5.5f, -3.6f),
            new Vector2( 6.5f, -3.5f),
            new Vector2( 6.4f, -2.5f),

            new Vector2(-6.7f,  0.8f),
            new Vector2( 6.7f, -0.8f),
            new Vector2(-3.0f,  4.0f),
            new Vector2( 3.0f, -4.0f)
        };

        Random.State previousRandomState = Random.state;
        Random.InitState(2026);

        for (int i = 0; i < positions.Length; i++)
        {
            Sprite sprite = sprites[Random.Range(0, sprites.Count)];

            GameObject item = new GameObject(
                "Environment_" + sprite.name);

            Undo.RegisterCreatedObjectUndo(item, "Add Environment Decoration");
            item.transform.SetParent(group.transform, false);
            item.transform.localPosition =
                new Vector3(positions[i].x, positions[i].y, 0f);

            float scale = Random.Range(0.8f, 1.15f);
            item.transform.localScale = Vector3.one * scale;
            item.transform.localRotation =
                Quaternion.Euler(0f, 0f, Random.Range(-25f, 25f));

            SpriteRenderer renderer = item.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = -1;
        }

        Random.state = previousRandomState;

        Selection.activeGameObject = group;
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

        Debug.Log(
            "Level 2 decorations created using " + sprites.Count +
            " available environment sprites.");
    }

    void RemoveDecorations()
    {
        GameObject arena = GameObject.Find("ArenaFloor");

        if (arena == null)
            return;

        Transform group = arena.transform.Find(GroupName);

        if (group != null)
        {
            Undo.DestroyObjectImmediate(group.gameObject);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("Level 2 decorations removed.");
        }
    }
}
