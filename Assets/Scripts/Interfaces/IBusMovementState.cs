using System.Collections.Generic;
using UnityEngine;

public interface IBusMovementState
{
   Bus Bus { get; }
   bool IsActive { get; }
   int EffectivePriority { get; }
   int PriorityOrder { get; }
   Vector3 Position { get; }
   
   bool IsBlockedAhead(IReadOnlyList<IBusMovementState> allStates, float minSpacing);
   
   void Tick(float deltaTime);
}
