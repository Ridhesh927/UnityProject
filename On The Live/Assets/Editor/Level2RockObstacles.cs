
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Level2RockObstacles : EditorWindow
{
    private const string EnvironmentPath = "Assets/Sprites/Environment";
    private const string GroupName = "Level2_RockObstacles";

    [MenuItem("Tools/ON THE LIVE/Build Level 2 Rock Obstacles")]
    static void OpenWindow()
    {
        GetWindow<Level2RockObstacles>("Level 2 Rocks");
    }

    void OnGUI()
    {
        GUILayout.Label("LEVEL 2 - ROCK OBSTACLES", EditorStyles.boldLabel);

        EditorGUILayout.HelpBox(
            "Finds the Kenney rock sprites automatically and places seven " +
            "obstacles with colliders in the arena.",
            MessageType.Info);

        if (GUILayout.Button("BUILD ROCK OBSTACLES", GUILayout.Height(40)))
            BuildRocks();

        if (GUILayout.Button("REMOVE ROCK OBSTACLES"))
            RemoveRocks();
    }

    void BuildRocks()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog(
                "Stop Play Mode", "Stop Play mode first.", "OK");
            return;
        }

        GameObject arena = GameObject.Find("ArenaFloor");

        if (arena == null)
        {
            EditorUtility.DisplayDialog(
                "Arena Not Found", "No ArenaFloor exists in this scene.", "OK");
            return;
        }

        
List<Sprite> rocks = new List<Sprite>();

string[] guids = AssetDatabase.FindAssets(
    "scifiEnvironment_", new[] { EnvironmentPath });

foreach (string guid in guids)
{
    string path = AssetDatabase.GUIDToAssetPath(guid);

    if (!path.EndsWith(".png", System.StringComparison.OrdinalIgnoreCase))
        continue;

    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

    if (sprite == null)
    {
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);

        foreach (Object asset in assets)
        {
            if (asset is Sprite extractedSprite)
                sprite = extractedSprite;

            if (sprite != null)
                break;
        }
    }

    if (sprite == null)
        continue;

    if (System.Text.RegularExpressions.Regex.IsMatch(
        sprite.name,
        @"^scifiEnvironment_(03|04|05|06|07|08|17|18|19|20)$"))
    {
        rocks.Add(sprite);
    }
}

Debug.Log("Rock sprite search found " + rocks.Count + " matching sprites.");


        if (rocks.Count == 0)
        {
            Debug.LogError(
                "No rock sprites found. Check that these assets are imported " +
                "as Sprite assets under " + EnvironmentPath + ".");

            EditorUtility.DisplayDialog(
                "No Rock Sprites Found",
                "Unity found no matching rock sprites. Check the Console and " +
                "confirm the assets are imported as Sprite (2D and UI).",
                "OK");
            return;
        }

        rocks.Sort((a, b) => string.CompareOrdinal(a.name, b.name));

        Transform oldGroup = arena.transform.Find(GroupName);

        if (oldGroup != null)
        {
            if (!EditorUtility.DisplayDialog(
                "Replace Rocks",
                "Replace the existing rock obstacle group?",
                "Replace", "Cancel"))
                return;

            Undo.DestroyObjectImmediate(oldGroup.gameObject);
        }

        GameObject group = new GameObject(GroupName);
        Undo.RegisterCreatedObjectUndo(group, "Build Rock Obstacles");
        group.transform.SetParent(arena.transform, false);

        Vector2[] positions =
        {
            new Vector2(-3.5f,  1.8f),
            new Vector2( 0.0f,  2.3f),
            new Vector2( 3.5f,  1.8f),
            new Vector2(-2.0f, -0.2f),
            new Vector2( 2.0f, -0.2f),
            new Vector2(-3.5f, -2.1f),
            new Vector2( 3.5f, -2.1f)
        };

        for (int i = 0; i < positions.Length; i++)
        {
            Sprite sprite = rocks[i % rocks.Count];

            GameObject rock = new GameObject("RockObstacle_" + (i + 1));
            Undo.RegisterCreatedObjectUndo(rock, "Create Rock Obstacle");
            rock.transform.SetParent(group.transform, false);
            rock.transform.localPosition =
                new Vector3(positions[i].x, positions[i].y, 0f);
            rock.transform.localScale = Vector3.one * 0.85f;

            SpriteRenderer renderer = rock.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = 2;

            CircleCollider2D collider =
                rock.AddComponent<CircleCollider2D>();

            collider.radius = Mathf.Max(
                sprite.bounds.extents.x,
                sprite.bounds.extents.y) * 0.65f;
        }

        Selection.activeGameObject = group;
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

        Debug.Log("Built seven Level 2 rock obstacles from " +
                  rocks.Count + " matching rock sprites.");
    }

    void RemoveRocks()
    {
        GameObject arena = GameObject.Find("ArenaFloor");
        if (arena == null) return;

        Transform group = arena.transform.Find(GroupName);

        if (group != null)
        {
            Undo.DestroyObjectImmediate(group.gameObject);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
}
