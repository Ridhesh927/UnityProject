
using UnityEngine;

public class ArenaSortingFix : MonoBehaviour
{
    [ContextMenu("Fix Arena Sprite Sorting")]
    public void FixSorting()
    {
        // Keep all decorative sprites behind the characters.
        GameObject arena = GameObject.Find("ArenaFloor");

        if (arena != null)
        {
            foreach (SpriteRenderer sr in
                     arena.GetComponentsInChildren<SpriteRenderer>(true))
            {
                bool isDecoration =
                    sr.gameObject.name.StartsWith("ArenaPlant_") ||
                    sr.transform.parent != null &&
                    sr.transform.parent.name == "ArenaDecorations_Auto";

                if (isDecoration)
                {
                    sr.sortingLayerName = "Default";
                    sr.sortingOrder = 1;
                }
            }
        }

        // Draw the player in front of the decorations.
        SetCharacterSorting(GameObject.Find("Player"));

        Debug.Log("Arena decorative sorting updated.");
    }

    private void SetCharacterSorting(GameObject character)
    {
        if (character == null)
            return;

        foreach (SpriteRenderer sr in
                 character.GetComponentsInChildren<SpriteRenderer>(true))
        {
            sr.sortingLayerName = "Default";
            sr.sortingOrder = 20;
        }
    }
}
