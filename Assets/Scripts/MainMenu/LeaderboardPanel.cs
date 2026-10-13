using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;
using YG.Utils.LB;

public class LeaderboardPanel : MonoBehaviour
{
    [SerializeField] private string _technoName = "levelRecord";
    [SerializeField] private Transform _rowsRoot;
    [SerializeField] private LeaderboardRowView _rowPrefab;
    [SerializeField] private GameObject _loadingLabel;
    
    private readonly List<LeaderboardRowView> _rows = new List<LeaderboardRowView>();

    private void OnEnable()
    {
        YG2.onGetLeaderboard += OnGetLeaderboard;
        
        _loadingLabel.SetActive(false);
        YG2.GetLeaderboard(_technoName);
    }

    private void OnDisable()
    {
        YG2.onGetLeaderboard -= OnGetLeaderboard;
    }

    private void OnGetLeaderboard(LBData data)
    {
        if (data.technoName != _technoName)
            return;
        
        _loadingLabel.SetActive(false);
        ClearRows();

        foreach (LBPlayerData player in data.players)
        {
            LeaderboardRowView row = Instantiate(_rowPrefab, _rowsRoot);
            row.Fill(player.rank, player.name, player.score);
            _rows.Add(row);
        }
    }

    private void ClearRows()
    {
        foreach (LeaderboardRowView row in _rows)
            Destroy(row.gameObject);
        
        _rows.Clear();
    }
}
