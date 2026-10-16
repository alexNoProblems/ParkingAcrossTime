using UnityEngine;
using YG;

public class ResetSavesTool : MonoBehaviour
{
    [ContextMenu("Reset Saves")]
    public void ResetSaves()
    {
        YG2.saves.coins = 0;
        YG2.saves.level = 1;
        YG2.saves.firstLaneHintShown = false;
        YG2.SaveProgress();
    }
}
