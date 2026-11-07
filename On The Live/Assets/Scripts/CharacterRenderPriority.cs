
using UnityEngine;

[DisallowMultipleComponent]
public class CharacterRenderPriority : MonoBehaviour
{
    [SerializeField] private int characterSortingOrder = 20;

    private void Awake()
    {
        SpriteRenderer[] renderers =
            GetComponentsInChildren<SpriteRenderer>(true);

        foreach (SpriteRenderer sr in renderers)
        {
            sr.sortingOrder = characterSortingOrder;
        }
    }
}
