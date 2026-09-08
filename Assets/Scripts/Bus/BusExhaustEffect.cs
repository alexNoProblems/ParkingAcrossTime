using UnityEngine;

public class BusExhaustEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _exhaustEffect;

    public void PlayPuff()
    {
        _exhaustEffect.Play();
    }
}
