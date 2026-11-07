using UnityEngine;

public class ARColorChanger : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnMouseDown()
    {
        // Randomly change color when tapped/clicked
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        if (meshRenderer != null)
        {
            meshRenderer.material.color = randomColor;
        }
    }
}