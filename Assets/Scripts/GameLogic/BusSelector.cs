using UnityEngine;

public class BusSelector
{
    private readonly BusBoarding _busBoarding;
    private readonly BusPatrolManager _busPatrolManager;
    private readonly AudioSource _feedbackAudioSource;
    private readonly AudioClip _wrongSelectionClip;

    public BusSelector(BusBoarding busBoarding, BusPatrolManager busPatrolManager,
        AudioSource feedbackAudioSource, AudioClip wrongSelectionClip)
    {
        _busBoarding = busBoarding;
        _busPatrolManager = busPatrolManager;
        _feedbackAudioSource = feedbackAudioSource;
        _wrongSelectionClip = wrongSelectionClip;
    }

    public void Select(Bus bus)
    {
        if (bus.Lane != null && !bus.Lane.IsFront(bus))
        {
            PlayWrongSelectionFeedback(bus);

            return;
        }

        if (bus.Lane == null)
            return;

        if (!_busPatrolManager.HasFreeSlot)
        {
            PlayWrongSelectionFeedback(bus);

            return;
        }

        Bus releasedBus = bus.Lane.ReleaseFront();

        if (releasedBus == null)
            return;

        releasedBus.Mover.StopMoving();
        releasedBus.PlayExhaustEffect();
        releasedBus.PlayEngineSound();

        _busBoarding.HandleReleasedBus(releasedBus);
    }

    private void PlayWrongSelectionFeedback(Bus bus)
    {
        _feedbackAudioSource.PlayOneShot(_wrongSelectionClip);
        bus.PlayWrongSelectionShake();
    }
}