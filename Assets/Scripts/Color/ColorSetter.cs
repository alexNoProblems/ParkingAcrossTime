using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorSetter : MonoBehaviour
{
    private static readonly int ColorPropertyID = Shader.PropertyToID("_Color");
    
    [SerializeField] private List<ColorTarget> _targets;

    public void SetColor(StickmanColor color)
    {
        Color paletteColor = ColorPalette.GetColor(color);

        foreach (ColorTarget target in _targets)
        {
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            target.Renderer.GetPropertyBlock(materialPropertyBlock, target.MaterialIndex);
            materialPropertyBlock.SetColor(ColorPropertyID, paletteColor);
            target.Renderer.SetPropertyBlock(materialPropertyBlock, target.MaterialIndex);
        }
    }
}
