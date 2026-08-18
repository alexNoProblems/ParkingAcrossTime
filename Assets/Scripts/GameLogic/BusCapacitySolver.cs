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
      {
         Shuffle(result);
         
         return result;
      }
      
      return null;
   }

   private bool TryDecompose(int remainingSeats, List<int> result)
   {
      if (remainingSeats == 0)
         return true;

      if (remainingSeats < 0)
         return false;

      foreach (var capacity in _availableCapacities.OrderByDescending(capacity => capacity))
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

   private void Shuffle(List<int> list)
   {
      for (int i = list.Count - 1; i > 0; i--)
      {
         int j =  Random.Range(0, i + 1);
         (list[i], list[j]) = (list[j], list[i]);
      }
   }
}
