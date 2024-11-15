using UnityEngine;
using TMPro;

public class PlayerScoreUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text_PlayerName = null;

    [SerializeField]
    private TextMeshProUGUI[] text_roundScores = null;

    [SerializeField]
    private TextMeshProUGUI text_Total = null;

    public void SetPlayerName(string _name)
    {
        text_PlayerName.text = _name;
    }

    public void SetRoundScores(int _round, int _score)
    {
        text_roundScores[_round - 1].text = string.Format("Round {0}: {1}", _round, _score);
    }

    public void SetTotal(int _total)
    {
        text_Total.text = "Total: " + _total;
    }
}
