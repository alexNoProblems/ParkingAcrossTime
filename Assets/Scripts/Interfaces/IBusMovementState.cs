using System.Collections.Generic;
using UnityEngine;

public interface IBusMovementState
{
   Bus Bus { get; }
   bool IsActive { get; }
   int EffectivePriority { get; }
   int PriorityOrder { get; }
   Vector3 Position { get; }
   bool BlocksAllTraffic { get; }

   void Tick(float deltaTime, IReadOnlyList<IBusMovementState> allStates, float minSpacing);
}
