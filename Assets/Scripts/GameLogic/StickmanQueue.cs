using System.Collections.Generic;
using UnityEngine;

public class StickmanQueue : MonoBehaviour
{
   private readonly List<Stickman> _stickmen = new List<Stickman>();
   
   public void Register(Stickman stickman)
   {
      _stickmen.Add(stickman);
   }

   public Stickman PeekFront()
   {
      return _stickmen.Count > 0 ? _stickmen[0] :  null;
   }

   public Stickman DequeueFront()
   {
      if (_stickmen.Count == 0)
         return null;
      
      Stickman front = _stickmen[0];
      _stickmen.RemoveAt(0);

      if (_stickmen.Count > 0)
      
         _stickmen[0].Mover.SetLeader(null);
   
      return front;
   }
}
