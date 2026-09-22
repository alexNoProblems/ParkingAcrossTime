using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelColorShuffleCoordinator : MonoBehaviour
{
    [SerializeField] private StickmanQueue _stickmanQueue;
    [SerializeField] private BusMovementGate _movementGate;
    [SerializeField] private float _blinkDuration = 1.49f;
    [SerializeField] private float _blinkInterval = 0.1f;
    
    private readonly ColorShuffleRoutine _shuffleRoutine = new ColorShuffleRoutine();

    public void TriggerShuffle()
    {
        IReadOnlyList<Stickman> stickmen = _stickmanQueue.GetAll();

        Dictionary<StickmanColor, int> currentCounts = CalculateCurrentColorCounts(stickmen);
        Dictionary<Stickman, StickmanColor> stickmanColors = AssignStickmanColors(stickmen, currentCounts);

        var flickerPalette = new List<StickmanColor>(currentCounts.Keys);
        var targets = new List<IColorFlickerTarget>(stickmen);

        var finalColors = new Dictionary<IColorFlickerTarget, StickmanColor>();
        foreach (var pair in stickmanColors)
            finalColors[pair.Key] = pair.Value;

        StartCoroutine(ShuffleWithBusesPaused(targets, flickerPalette, finalColors));
    }

    private Dictionary<StickmanColor, int> CalculateCurrentColorCounts(IReadOnlyList<Stickman> stickmen)
    {
        var counts = new Dictionary<StickmanColor, int>();

        foreach (Stickman stickman in stickmen)
        {
            if (!counts.ContainsKey(stickman.Color))
                counts[stickman.Color] = 0;

            counts[stickman.Color]++;
        }

        return counts;
    }
    
    private IEnumerator ShuffleWithBusesPaused(List<IColorFlickerTarget> targets, List<StickmanColor> flickerPalette,
        Dictionary<IColorFlickerTarget, StickmanColor> finalColors)
    {
        _movementGate.Pause();

        yield return _shuffleRoutine.Run(targets, flickerPalette, finalColors, _blinkDuration, _blinkInterval);

        _movementGate.Resume();
    }

    private Dictionary<Stickman, StickmanColor> AssignStickmanColors(IReadOnlyList<Stickman> stickmen,
        Dictionary<StickmanColor, int> supply)
    {
        var remainingSupply = new Dictionary<StickmanColor, int>(supply);
        var allColors = new List<StickmanColor>(supply.Keys);
        var shuffledStickmen = new List<Stickman>(stickmen);

        Shuffle(shuffledStickmen);

        var finalColors = new Dictionary<Stickman, StickmanColor>();

        foreach (Stickman stickman in shuffledStickmen)
        {
            StickmanColor color = PickColorWithRemainingSupply(remainingSupply, allColors);
            finalColors[stickman] = color;

            if (remainingSupply.ContainsKey(color))
                remainingSupply[color]--;
        }

        return finalColors;
    }

    private StickmanColor PickColorWithRemainingSupply(Dictionary<StickmanColor, int> remainingSupply,
        List<StickmanColor> allColors)
    {
        var available = new List<StickmanColor>();

        foreach (var pair in remainingSupply)
        {
            if (pair.Value > 0)
                available.Add(pair.Key);
        }

        if (available.Count > 0)
            return available[Random.Range(0, available.Count)];

        return allColors[Random.Range(0, allColors.Count)];
    }

    private void Shuffle(List<Stickman> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}