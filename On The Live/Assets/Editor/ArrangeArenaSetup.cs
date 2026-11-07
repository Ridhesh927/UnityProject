
using UnityEngine;
using UnityEditor;

public static class ArrangeArenaSetup
{
    [MenuItem("Tools/ON THE LIVE/Arrange Arena Setup")]
    public static void Arrange()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogError("Stop Play mode before arranging the arena.");
            return;
        }

        GameObject arena = GameObject.Find("ArenaFloor");

        if (arena == null)
        {
            Debug.LogError("ArenaFloor was not found.");
            return;
        }

        // Measure the actual floor tiles.
        SpriteRenderer[] renderers =
            arena.GetComponentsInChildren<SpriteRenderer>(true);

        Bounds bounds = new Bounds();
        bool foundFloor = false;

        foreach (SpriteRenderer sr in renderers)
        {
            if (!sr.name.StartsWith("FloorTile"))
                continue;

            if (!foundFloor)
            {
                bounds = sr.bounds;
                foundFloor = true;
            }
            else
            {
                bounds.Encapsulate(sr.bounds);
            }
        }

        if (!foundFloor)
        {
            Debug.LogError(
                "No FloorTile SpriteRenderers found under ArenaFloor.");
            return;
        }

        float inset = Mathf.Min(
            0.75f,
            bounds.size.x * 0.25f,
            bounds.size.y * 0.25f
        );

        Vector2 center = bounds.center;

        // Find the player by tag, with a name fallback.
        GameObject player = null;

        try
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        catch (UnityException)
        {
            // The Player tag may not exist.
        }

        if (player == null)
            player = GameObject.Find("Player");

        if (player != null)
        {
            Undo.RecordObject(
                player.transform, "Arrange Player");

            Vector3 p = player.transform.position;
            player.transform.position =
                new Vector3(center.x, center.y, p.z);
        }
        else
        {
            Debug.LogWarning("Player object was not found.");
        }

        // Place four spawn points inside the floor.
        string[] names =
        {
            "SpawnPoint_1",
            "SpawnPoint_2",
            "SpawnPoint_3",
            "SpawnPoint_4"
        };

        Vector2[] positions =
        {
            new Vector2(
                bounds.min.x + inset,
                bounds.max.y - inset),

            new Vector2(
                bounds.max.x - inset,
                bounds.max.y - inset),

            new Vector2(
                bounds.min.x + inset,
                bounds.min.y + inset),

            new Vector2(
                bounds.max.x - inset,
                bounds.min.y + inset)
        };

        for (int i = 0; i < names.Length; i++)
        {
            GameObject point = GameObject.Find(names[i]);

            if (point == null)
            {
                Debug.LogWarning(
                    names[i] + " was not found.");
                continue;
            }

            Undo.RecordObject(
                point.transform, "Arrange Spawn Point");

            Vector3 p = point.transform.position;

            point.transform.position = new Vector3(
                positions[i].x,
                positions[i].y,
                p.z
            );
        }

        Debug.Log(
            "Arena arranged. Floor bounds: " +
            bounds + ". Player and spawn points positioned.");
    }
}
