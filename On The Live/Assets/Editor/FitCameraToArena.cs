
using UnityEngine;
using UnityEditor;

public class ExpandArenaToScreen : EditorWindow
{
    [MenuItem("Tools/ON THE LIVE/Expand Arena For Widescreen")]
    public static void Expand()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogError("Stop Play mode before expanding the arena.");
            return;
        }

        GameObject arena = GameObject.Find("ArenaFloor");
        Camera cam = Camera.main;

        if (arena == null || cam == null)
        {
            Debug.LogError(
                "Could not find ArenaFloor or Main Camera."
            );
            return;
        }

        SpriteRenderer[] renderers =
            arena.GetComponentsInChildren<SpriteRenderer>(true);

        Bounds bounds = new Bounds();
        bool found = false;

        foreach (SpriteRenderer sr in renderers)
        {
            if (!sr.gameObject.name.StartsWith("FloorTile"))
                continue;

            if (!found)
            {
                bounds = sr.bounds;
                found = true;
            }
            else
            {
                bounds.Encapsulate(sr.bounds);
            }
        }

        if (!found)
        {
            Debug.LogError("No FloorTile sprites found.");
            return;
        }

        float currentAspect = bounds.size.x / bounds.size.y;
        float targetAspect = Mathf.Min(cam.aspect * 0.88f, 1.60f);
        float factor = targetAspect / currentAspect;

        if (factor <= 1.01f)
        {
            Debug.Log("Arena is already wide enough.");
            return;
        }

        if (!EditorUtility.DisplayDialog(
            "Expand Arena",
            "This will widen the floor, walls, decorations, and spawn points.\n\n" +
            "Save a backup scene before continuing. Run this tool only once.",
            "Expand Arena",
            "Cancel"))
            return;

        Undo.RegisterFullObjectHierarchyUndo(
            arena, "Expand Arena For Widescreen");

        Transform root = arena.transform;
        float centerX = bounds.center.x;

        // Stretch the floor tiles horizontally while spacing their
        // centers proportionally, keeping adjacent tiles connected.
        foreach (SpriteRenderer sr in renderers)
        {
            if (!sr.gameObject.name.StartsWith("FloorTile"))
                continue;

            Transform t = sr.transform;
            Vector3 p = t.position;
            p.x = centerX + (p.x - centerX) * factor;
            t.position = p;

            Vector3 s = t.localScale;
            s.x *= factor;
            t.localScale = s;
        }

        // Move other direct children and widen horizontal walls.
        foreach (Transform child in root)
        {
            if (child.name.StartsWith("FloorTile"))
                continue;

            Vector3 p = child.position;
            p.x = centerX + (p.x - centerX) * factor;
            child.position = p;

            if (child.name == "Wall_Top" ||
                child.name == "Wall_Bottom")
            {
                Vector3 s = child.localScale;
                s.x *= factor;
                child.localScale = s;
            }
        }

        // Move the four scene-level spawn points to the expanded edges.
        string[] spawnNames =
        {
            "SpawnPoint_1", "SpawnPoint_2",
            "SpawnPoint_3", "SpawnPoint_4"
        };

        foreach (string name in spawnNames)
        {
            GameObject spawn = GameObject.Find(name);
            if (spawn == null)
                continue;

            Undo.RecordObject(spawn.transform, "Move Spawn Point");

            Vector3 p = spawn.transform.position;
            p.x = centerX + (p.x - centerX) * factor;
            spawn.transform.position = p;
        }

        // Fit the camera to the new arena.
        renderers = arena.GetComponentsInChildren<SpriteRenderer>(true);
        found = false;

        foreach (SpriteRenderer sr in renderers)
        {
            if (!sr.gameObject.name.StartsWith("FloorTile"))
                continue;

            if (!found)
            {
                bounds = sr.bounds;
                found = true;
            }
            else
            {
                bounds.Encapsulate(sr.bounds);
            }
        }

        Undo.RecordObject(cam, "Fit Camera");
        Undo.RecordObject(cam.transform, "Center Camera");

        Vector3 cameraPos = cam.transform.position;
        cameraPos.x = bounds.center.x;
        cameraPos.y = bounds.center.y;
        cam.transform.position = cameraPos;

        cam.orthographic = true;
        cam.orthographicSize =
            Mathf.Max(bounds.extents.y, bounds.extents.x / cam.aspect)
            * 1.08f;

        EditorUtility.SetDirty(cam);
        EditorUtility.SetDirty(cam.transform);

        Debug.Log(
            "Arena expanded. Factor: " + factor.ToString("F2") +
            ". Check the walls and spawn points before pressing Play."
        );
    }
}
