using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.1f;
    private MeshRenderer meshRenderer;
    private Vector2 savedOffset;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        savedOffset = meshRenderer.material.mainTextureOffset;
    }

    void Update()
    {
        float y = Mathf.Repeat(Time.time * scrollSpeed, 1);
        Vector2 offset = new Vector2(savedOffset.x, y);
        meshRenderer.material.mainTextureOffset = offset;
    }
}
