using UnityEngine;

public class StickmanAnimator : MonoBehaviour
{
    private readonly int IsRunning = Animator.StringToHash("IsRunning");
    
    [SerializeField] private Animator _animator;
    
    public void SetRunning(bool isRunning)
    {
        _animator.SetBool(IsRunning, isRunning);
    }
}
