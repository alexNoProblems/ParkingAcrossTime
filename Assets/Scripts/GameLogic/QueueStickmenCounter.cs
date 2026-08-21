using System;
using UnityEngine;

public class QueueStickmenCounter : MonoBehaviour
{
   public event Action<int> CountChanged;

   public int Count { get; private set; }

   public void SetInitialCount(int count)
   {
      Count = count;
      CountChanged?.Invoke(Count);
   }

   public void Decrease(int amount = 1)
   {
      Count = Mathf.Max(0, Count - amount);
      CountChanged?.Invoke(Count);
   }
}