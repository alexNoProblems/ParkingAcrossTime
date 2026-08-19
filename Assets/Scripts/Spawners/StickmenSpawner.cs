using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StickmanSpawnData
{
    public StickmanColor Color;
    public int Count;
}

public class StickmenSpawner : MonoBehaviour, ISpawner<StickmanSpawnData>
{
   [SerializeField] private GameObject stickmanPrefab;
   [SerializeField] private Transform spawnPoint;
   [SerializeField] private List<Transform> pathWaypoints;
   [SerializeField] private List<Transform> queueWaypoints;
   [SerializeField] private float queueSlotSpacing = 1f;
   [SerializeField] private float spawnInterval = 0.15f;

   private WaitForSeconds _waitForSeconds;
   private int _spawnedCount;

   private void Awake()
   {
       _waitForSeconds = new WaitForSeconds(spawnInterval);
   }
   
   public IEnumerator Spawn(StickmanSpawnData data)
   {
       var slotProvider = new QueueSlotProvider(queueWaypoints, queueSlotSpacing);

       for (int i = 0; i < data.Count; i++)
       {
           var stickman = Instantiate(stickmanPrefab, spawnPoint.position, Quaternion.identity);

           if (stickman.TryGetComponent<ColorSetter>(out var colorSetter))
           {
               colorSetter.SetColor(data.Color);
           }
           else
           {
               Debug.LogError($"На префабе {stickmanPrefab.name} отсутствует ColorSetter", stickman);
           }

           if (stickman.TryGetComponent<StickmanMover>(out var mover))
           {
               var path = BuildPath(slotProvider.GetSlotPosition(_spawnedCount));
               mover.StartMoving(path);
           }
           else
           {
               Debug.LogError($"На префабе {stickmanPrefab.name} отсутствует StickmanMover", stickman);
           }
           
           _spawnedCount++;
           
           yield return _waitForSeconds;
       }
   }

   private List<Vector3> BuildPath(Vector3 finalSlot)
   {
       var path = new List<Vector3>();

       foreach (var waypoint in pathWaypoints)
           path.Add(waypoint.position);
       
       path.Add(finalSlot);
       
       return path;
   }
}
