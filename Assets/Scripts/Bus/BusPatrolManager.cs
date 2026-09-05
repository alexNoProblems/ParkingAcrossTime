using System.Collections.Generic;
using UnityEngine;

public class BusPatrolManager : MonoBehaviour
{
   [SerializeField] private int _maxCurrentPatrols = 3;
   
   private readonly List<BusRoutePatrol> _activePatrols = new List<BusRoutePatrol>();
   
   public bool HasFreeSlot => _activePatrols.Count < _maxCurrentPatrols;

   public void BusRegister(BusRoutePatrol busRoutePatrol)
   {
      if (!_activePatrols.Contains(busRoutePatrol))
         _activePatrols.Add(busRoutePatrol);
   }

   public void BusUnregister(BusRoutePatrol busRoutePatrol)
   {
      _activePatrols.Remove(busRoutePatrol);
   }
}
