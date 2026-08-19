using UnityEngine;

public class ColorSetter : MonoBehaviour
{
    private static readonly int ColorPropertyID = Shader.PropertyToID("_Color");
    
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private int materialIndex;

    public void SetColor(StickmanColor color)
    {
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        targetRenderer.GetPropertyBlock(materialPropertyBlock, materialIndex);
        materialPropertyBlock.SetColor(ColorPropertyID, ColorPalette.GetColor(color));
        targetRenderer.SetPropertyBlock(materialPropertyBlock,  materialIndex);
    }
}
