using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelColorShuffleCoordinator : MonoBehaviour
{
    [SerializeField] private StickmanQueue _stickmanQueue;
    [SerializeField] private BusRegistry _busRegistry;
    [SerializeField] private float _blinkDuration = 1.49f;
    [SerializeField] private float _blinkInterval = 0.1f;
    
    private readonly ColorShuffleRoutine _shuffleRoutine = new ColorShuffleRoutine();

    public void TriggerShuffle()
    {
        IReadOnlyList<Stickman> stickmen = _stickmanQueue.GetAll();
        IReadOnlyList<Bus> buses = _busRegistry.GetAll();

        Dictionary<StickmanColor, int> supply = CalculateSupply(buses);
        Dictionary<Stickman, StickmanColor> stickmanColors = AssignStickmanColors(stickmen, supply);

        var flickerPalette = new List<StickmanColor>(supply.Keys);
        var targets = new List<IColorFlickerTarget>(stickmen);

        var finalColors = new Dictionary<IColorFlickerTarget, StickmanColor>();
        foreach (var pair in stickmanColors)
            finalColors[pair.Key] = pair.Value;

        StartCoroutine(_shuffleRoutine.Run(targets, flickerPalette, finalColors, _blinkDuration, _blinkInterval));
    }

    private Dictionary<StickmanColor, int> CalculateSupply(IReadOnlyList<Bus> buses)
    {
        var supply = new Dictionary<StickmanColor, int>();

        foreach (Bus bus in buses)
        {
            if (!supply.ContainsKey(bus.Color))
                supply[bus.Color] = 0;

            supply[bus.Color] += bus.Capacity.Capacity;
        }

        return supply;
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