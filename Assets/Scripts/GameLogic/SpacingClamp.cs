using System;
using System.Collections.Generic;
using UnityEngine;

public class SpacingClamp
{
   private const int MaxIterations = 6;

   public float ClampDistance(Func<float, Vector3> positionAt, float currentDistance, float desiredDistance,
      IReadOnlyList<Vector3> blockerPositions, float minSpacing)
   {
      if (blockerPositions.Count == 0)
         return desiredDistance;

      if (IsSafe(positionAt, desiredDistance, blockerPositions,  minSpacing))
         return desiredDistance;

      if (!IsSafe(positionAt, currentDistance, blockerPositions, minSpacing))
         return currentDistance;

      float safeDistance = currentDistance;
      float unsafeDistance = desiredDistance;

      for (int i = 0; i < MaxIterations; i++)
      {
         float midDistance = (safeDistance + unsafeDistance) * 0.5f;

         if (IsSafe(positionAt, midDistance, blockerPositions, minSpacing))
            safeDistance = midDistance;
         else
            unsafeDistance = midDistance;
      }
      
      return safeDistance;
   }

   private bool IsSafe(Func<float, Vector3> positionAt, float distance, IReadOnlyList<Vector3> blockerPositions,
      float minSpacing)
   {
      Vector3 position = positionAt(distance);

      foreach (Vector3 blockerPosition in blockerPositions)
      {
         Vector3 offsetToBlocker = blockerPosition - position;
         offsetToBlocker.y = 0f;

         if (offsetToBlocker.magnitude <  minSpacing)
            return false;
      }
      
      return true;
   }
}
