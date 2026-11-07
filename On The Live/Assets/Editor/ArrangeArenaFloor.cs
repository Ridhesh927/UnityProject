
using UnityEngine;
using UnityEditor;

public class ArrangeArenaFloor : EditorWindow
{
    [MenuItem("Tools/ON THE LIVE/Arrange Arena Floor")]
    static void OpenWindow()
    {
        GetWindow<ArrangeArenaFloor>("Arrange Arena Floor");
    }

    void OnGUI()
    {
        GUILayout.Label("ON THE LIVE - Floor Layout",
            EditorStyles.boldLabel);

        if (GUILayout.Button("Arrange 30 Tiles (6 x 5)"))
        {
            GameObject parent = GameObject.Find("ArenaFloor");

            if (parent == null)
            {
                Debug.LogError("ArenaFloor not found!");
                return;
            }

            Transform[] tiles = parent.GetComponentsInChildren<Transform>();

            int index = 0;

            foreach (Transform tile in tiles)
            {
                if (tile == parent.transform)
                    continue;

                int col = index % 6;
                int row = index / 6;

                tile.localPosition = new Vector3(
                    col * 0.64f,
                    -row * 0.64f,
                    0
                );

                tile.localScale = Vector3.one;

                SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.sortingOrder = -10;

                index++;
            }

            Debug.Log("Arena floor arranged: " + index + " tiles.");
        }
    }
}
