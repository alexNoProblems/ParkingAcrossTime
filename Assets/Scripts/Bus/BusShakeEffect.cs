using System.Collections;
using UnityEngine;

public class BusShakeEffect : MonoBehaviour
{
    [SerializeField] private float _shakeDuration = 0.3f;
    [SerializeField] private float _shakeMagnitude = 0.1f;
    [SerializeField] private int _shakeCount = 4;

    private Coroutine _shakeCoroutine;
    private Vector3? _restPosition;

    public void Shake()
    {
        if (_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);

            if (_restPosition.HasValue)
                transform.position = _restPosition.Value;
        }
        else
        {
            _restPosition = transform.position;
        }

        _shakeCoroutine = StartCoroutine(ShakeRoutine());
    }

    public void CancelShake()
    {
        if (_shakeCoroutine == null)
            return;

        StopCoroutine(_shakeCoroutine);
        _shakeCoroutine = null;

        ResetToRestState();
    }

    private IEnumerator ShakeRoutine()
    {
        Vector3 originalPosition = _restPosition.Value;
        float stepDuration = _shakeDuration / _shakeCount;
        var waitForSeconds = new WaitForSeconds(stepDuration);

        for (int i = 0; i < _shakeCount; i++)
        {
            Vector3 offset = new Vector3(
                Random.Range(-_shakeMagnitude, _shakeMagnitude),
                0f,
                Random.Range(-_shakeMagnitude, _shakeMagnitude));

            transform.position = originalPosition + offset;

            yield return waitForSeconds;
        }

        _shakeCoroutine = null;
        ResetToRestState();
    }

    private void ResetToRestState()
    {
        if (_restPosition.HasValue)
            transform.position = _restPosition.Value;

        transform.rotation = Quaternion.identity;
    }
}