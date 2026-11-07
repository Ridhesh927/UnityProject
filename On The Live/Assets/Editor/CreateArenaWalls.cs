
using UnityEngine;
using UnityEditor;

public class CreateArenaWalls
{
    [MenuItem("Tools/ON THE LIVE/Create Arena Walls")]
    public static void CreateWalls()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogError("Stop Play mode before creating walls.");
            return;
        }

        GameObject arena = GameObject.Find("ArenaFloor");

        if (arena == null)
        {
            Debug.LogError("ArenaFloor was not found.");
            return;
        }

        // Measure the actual floor tiles, excluding existing walls.
        SpriteRenderer[] renderers =
            arena.GetComponentsInChildren<SpriteRenderer>(true);

        bool foundTile = false;
        Vector3 min = Vector3.zero;
        Vector3 max = Vector3.zero;

        foreach (SpriteRenderer sr in renderers)
        {
            if (sr.name.StartsWith("Wall_"))
                continue;

            Bounds b = sr.bounds;

            Vector3[] corners =
            {
                new Vector3(b.min.x, b.min.y, b.min.z),
                new Vector3(b.min.x, b.max.y, b.min.z),
                new Vector3(b.max.x, b.min.y, b.min.z),
                new Vector3(b.max.x, b.max.y, b.min.z)
            };

            foreach (Vector3 corner in corners)
            {
                Vector3 p =
                    arena.transform.InverseTransformPoint(corner);

                if (!foundTile)
                {
                    min = max = p;
                    foundTile = true;
                }
                else
                {
                    min = Vector3.Min(min, p);
                    max = Vector3.Max(max, p);
                }
            }
        }

        if (!foundTile)
        {
            Debug.LogError(
                "No floor SpriteRenderers found under ArenaFloor.");
            return;
        }

        float thickness = 0.3f;

        float left = min.x;
        float right = max.x;
        float bottom = min.y;
        float top = max.y;

        CreateOrUpdateWall(
            arena.transform, "Wall_Top",
            new Vector2((left + right) / 2f, top + thickness / 2f),
            new Vector2(right - left + thickness * 2f, thickness));

        CreateOrUpdateWall(
            arena.transform, "Wall_Bottom",
            new Vector2((left + right) / 2f, bottom - thickness / 2f),
            new Vector2(right - left + thickness * 2f, thickness));

        CreateOrUpdateWall(
            arena.transform, "Wall_Left",
            new Vector2(left - thickness / 2f, (bottom + top) / 2f),
            new Vector2(thickness, top - bottom));

        CreateOrUpdateWall(
            arena.transform, "Wall_Right",
            new Vector2(right + thickness / 2f, (bottom + top) / 2f),
            new Vector2(thickness, top - bottom));

        Debug.Log(
            $"Arena walls updated. Floor bounds: " +
            $"X {left:F2} to {right:F2}, " +
            $"Y {bottom:F2} to {top:F2}");
    }

    private static void CreateOrUpdateWall(
        Transform parent,
        string wallName,
        Vector2 position,
        Vector2 size)
    {
        Transform existing = parent.Find(wallName);
        GameObject wall;

        if (existing == null)
        {
            wall = new GameObject(wallName);
            Undo.RegisterCreatedObjectUndo(wall, "Create Arena Wall");
            wall.transform.SetParent(parent, false);
        }
        else
        {
            wall = existing.gameObject;
            Undo.RecordObject(wall.transform, "Update Arena Wall");
        }

        wall.transform.localPosition =
            new Vector3(position.x, position.y, 0f);

        wall.transform.localRotation = Quaternion.identity;
        wall.transform.localScale = Vector3.one;

        BoxCollider2D box =
            wall.GetComponent<BoxCollider2D>();

        if (box == null)
            box = Undo.AddComponent<BoxCollider2D>(wall);

        Undo.RecordObject(box, "Update Arena Wall Collider");
        box.offset = Vector2.zero;
        box.size = size;
    }
}
