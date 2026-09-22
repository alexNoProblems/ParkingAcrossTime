using System.Collections.Generic;
using UnityEngine;

public class BusParkingShuffler : MonoBehaviour
{
    [SerializeField] private List<BusLane> _lanes;

    public void Shuffle()
    {
        var allBuses = new List<Bus>();
        var countsPerLane = new List<int>();

        foreach (BusLane lane in _lanes)
        {
            allBuses.AddRange(lane.BusesInLane);
            countsPerLane.Add(lane.BusesInLane.Count);
        }

        ShuffleList(allBuses);

        int index = 0;

        for (int i = 0; i < _lanes.Count; i++)
        {
            int count = countsPerLane[i];
            var laneBuses = allBuses.GetRange(index, count);

            _lanes[i].SetParkedBuses(laneBuses);
            index += count;
        }
    }

    private void ShuffleList(List<Bus> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
