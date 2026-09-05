using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class CameraFitWidth : MonoBehaviour
{
    private const float HalfToFullMultiplier = 2f;
    
    public float _targetWidth = 6f;
    public float _targetHeight = 12f;
    
    private Camera _camera;
    private int _lastScreenWidth;
    private int _lastScreenHeight;
 
    private void Awake()
    {
        _camera = GetComponent<Camera>();
        ApplyFit();
    }
 
    private void Update()
    {
        if (Screen.width != _lastScreenWidth || Screen.height != _lastScreenHeight)
            ApplyFit();
    }
 
    private void ApplyFit()
    {
        if (_camera == null) 
            _camera = GetComponent<Camera>();
        
        if (_camera == null || !_camera.orthographic) 
            return;
 
        _lastScreenWidth = Screen.width;
        _lastScreenHeight = Screen.height;
 
        float aspect = _camera.aspect;
        float sizeForWidth = _targetWidth / (HalfToFullMultiplier * aspect);
        float sizeForHeight = _targetHeight / HalfToFullMultiplier;
        
        _camera.orthographicSize = Mathf.Max(sizeForWidth, sizeForHeight);
    }
}