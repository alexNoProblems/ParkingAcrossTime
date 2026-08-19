using UnityEngine;

public static class ColorPalette
{
    public static Color GetColor(StickmanColor color)
    {
        switch (color)
        {
            case StickmanColor.Red: 
                return new Color(0.8f, 0.1f, 0.1f);
            case StickmanColor.Yellow: 
                return new Color(0.95f, 0.8f, 0.1f);
            case StickmanColor.Blue: 
                return new Color(0.1f, 0.3f, 0.8f);
            case StickmanColor.Green: 
                return new Color(0.2f, 0.7f, 0.2f);
            case StickmanColor.Purple: 
                return new Color(0.5f, 0.1f, 0.6f);
            default: 
                return Color.white;
        }
    }
}
