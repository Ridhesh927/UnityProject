
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
public class Level2ArenaSetup : EditorWindow
{
    private const float ArenaWidth = 16f;
    private const float ArenaHeight = 10f;
    private const float WallThickness = 0.4f;

    [MenuItem("Tools/ON THE LIVE/Build Level 2 Arena")]
    static void OpenWindow()
    {
        GetWindow<Level2ArenaSetup>("Level 2 Arena");
    }

    void OnGUI()
    {
        GUILayout.Label("Level 2 - Hazard Zone", EditorStyles.boldLabel);

        EditorGUILayout.HelpBox(
            "Creates an orange arena floor and four solid boundary walls. " +
            "Does not modify MainLevel.",
            MessageType.Info);

        if (GUILayout.Button("CREATE LEVEL 2 ARENA", GUILayout.Height(40)))
            BuildArena();
    }

    void BuildArena()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog(
                "Stop Play Mode",
                "Stop Play mode before building the arena.",
                "OK");
            return;
        }

        GameObject existing = GameObject.Find("ArenaFloor");

        if (existing != null)
        {
            if (!EditorUtility.DisplayDialog(
                "Arena already exists",
                "Replace the existing ArenaFloor and its children?",
                "Replace", "Cancel"))
                return;

            Undo.DestroyObjectImmediate(existing);
        }

        // Generate and import a reusable orange floor sprite.
        string folder = "Assets/Sprites/Generated";

        if (!AssetDatabase.IsValidFolder("Assets/Sprites"))
        {
            AssetDatabase.CreateFolder("Assets", "Sprites");
        }

        if (!AssetDatabase.IsValidFolder(folder))
        {
            AssetDatabase.CreateFolder("Assets/Sprites", "Generated");
        }

        string texturePath = folder + "/Level2Floor.png";

        Texture2D texture = new Texture2D(
            256, 256, TextureFormat.RGBA32, false);

        Random.InitState(21);

        Color baseColor = new Color(0.72f, 0.31f, 0.19f, 1f);

        for (int y = 0; y < 256; y++)
        {
            for (int x = 0; x < 256; x++)
            {
                float variation = Random.Range(-0.035f, 0.035f);
                Color pixel = new Color(
                    Mathf.Clamp01(baseColor.r + variation),
                    Mathf.Clamp01(baseColor.g + variation * 0.65f),
                    Mathf.Clamp01(baseColor.b + variation * 0.5f),
                    1f);

                // Small subtle floor speckles.
                if (Random.value < 0.025f)
                    pixel *= 0.88f;

                texture.SetPixel(x, y, pixel);
            }
        }

        texture.Apply();

        File.WriteAllBytes(texturePath, texture.EncodeToPNG());
        DestroyImmediate(texture);

        AssetDatabase.ImportAsset(
            texturePath, ImportAssetOptions.ForceUpdate);

        TextureImporter importer =
            AssetImporter.GetAtPath(texturePath) as TextureImporter;

        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }

        Sprite floorSprite =
            AssetDatabase.LoadAssetAtPath<Sprite>(texturePath);

        if (floorSprite == null)
        {
            Debug.LogError("Could not create the Level 2 floor sprite.");
            return;
        }

        // Create arena root.
        GameObject arena = new GameObject("ArenaFloor");
        Undo.RegisterCreatedObjectUndo(arena, "Build Level 2 Arena");

        GameObject floor = new GameObject("Level2_Floor");
        floor.transform.SetParent(arena.transform, false);

        SpriteRenderer sr = floor.AddComponent<SpriteRenderer>();
        sr.sprite = floorSprite;
        sr.drawMode = SpriteDrawMode.Simple;
        sr.sortingOrder = -10;
        floor.transform.localScale = new Vector3(
            ArenaWidth, ArenaHeight, 1f);

        // Create solid boundary walls.
        CreateWall(arena.transform, "WallTop",
            new Vector2(0, ArenaHeight / 2f + WallThickness / 2f),
            new Vector2(ArenaWidth + WallThickness * 2f, WallThickness));

        CreateWall(arena.transform, "WallBottom",
            new Vector2(0, -ArenaHeight / 2f - WallThickness / 2f),
            new Vector2(ArenaWidth + WallThickness * 2f, WallThickness));

        CreateWall(arena.transform, "WallLeft",
            new Vector2(-ArenaWidth / 2f - WallThickness / 2f, 0),
            new Vector2(WallThickness, ArenaHeight));

        CreateWall(arena.transform, "WallRight",
            new Vector2(ArenaWidth / 2f + WallThickness / 2f, 0),
            new Vector2(WallThickness, ArenaHeight));

        // Keep the arena at world origin.
        arena.transform.position = Vector3.zero;

        Selection.activeGameObject = arena;

        EditorUtility.SetDirty(arena);
        Debug.Log("Level 2 arena created with floor and four boundary colliders.");
        EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }

    void CreateWall(
        Transform parent,
        string wallName,
        Vector2 position,
        Vector2 size)
    {
        GameObject wall = new GameObject(wallName);
        Undo.RegisterCreatedObjectUndo(wall, "Create Arena Wall");
        wall.transform.SetParent(parent, false);
        wall.transform.localPosition = position;

        BoxCollider2D collider = wall.AddComponent<BoxCollider2D>();
        collider.size = size;
    }
}
