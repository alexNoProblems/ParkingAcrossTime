using TMPro;
using UnityEngine;

public class LeaderboardRowView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rankText;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    public void Fill(int rank, string playerName, int score)
    {
        _rankText.text = rank.ToString();
        _nameText.text = playerName;
        _scoreText.text = score.ToString();
    }
}
