using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BusCapacitySolver
{
   private readonly int[] _availableCapacities = { 4, 6, 8 };

   public List<int> SolveCapacities(int totalSeats)
   {
      var result = new List<int>();

      if (TryDecompose(totalSeats, result))
         return result;
      
      return null;
   }

   private bool TryDecompose(int remainingSeats, List<int> result)
   {
      if (remainingSeats == 0)
         return true;

      if (remainingSeats < 0)
         return false;
      
      var shuffledCapacities = _availableCapacities.OrderBy(_ => Random.value).ToList();

      foreach (var capacity in shuffledCapacities)
      {
         if (remainingSeats - capacity < 0)
            continue;
         
         result.Add(capacity);

         if (TryDecompose(remainingSeats - capacity, result))
            return true;
         
         result.RemoveAt(result.Count - 1);
      }
      
      return false;
   }
}
