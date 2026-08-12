using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class CameraFitWidth : MonoBehaviour
{
    private const float HalfToFullMultiplier = 2f;
    
    public float targetWidth = 6f;
    public float targetHeight = 12f;
    
    private Camera camera;
    private int lastScreenWidth;
    private int lastScreenHeight;
 
    private void Awake()
    {
        camera = GetComponent<Camera>();
        ApplyFit();
    }
 
    private void Update()
    {
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
            ApplyFit();
    }
 
    private void ApplyFit()
    {
        if (camera == null) 
            camera = GetComponent<Camera>();
        
        if (camera == null || !camera.orthographic) 
            return;
 
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
 
        float aspect = camera.aspect;
        float sizeForWidth = targetWidth / (HalfToFullMultiplier * aspect);
        float sizeForHeight = targetHeight / HalfToFullMultiplier;
        
        camera.orthographicSize = Mathf.Max(sizeForWidth, sizeForHeight);
    }
}