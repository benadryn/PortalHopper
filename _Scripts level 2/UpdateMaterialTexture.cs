
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class UpdateMaterialTexture : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Material _material;
    private static readonly int SpriteTex = Shader.PropertyToID("_SpriteTex");

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = GetComponent<Renderer>().material;
    }

    
    void Update()
    {
        UpdateTexture();
    }

    private void UpdateTexture()
    {
        if (_spriteRenderer != null && _material != null)
        {
            _material.SetTexture(SpriteTex, _spriteRenderer.sprite.texture);
        }
    }
}
