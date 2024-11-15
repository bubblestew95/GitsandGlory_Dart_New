using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private PlayerScoreUI[] playerScoreUIs = null;

    [SerializeField]
    private ResultUI resultUI = null;

    private static UIManager instance = null;

    public static UIManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdatePlayerName(int _playerIdx, string _name)
    {
        playerScoreUIs[_playerIdx].SetPlayerName(_name);
    }

    public void UpdatePlayerScore(int _playerIdx, int round, int _score, int _total)
    {
        playerScoreUIs[_playerIdx].SetRoundScores(round, _score);
        playerScoreUIs[_playerIdx].SetTotal(_total);
    }

    public void SetResultUI(int _rank, string _name, int _score)
    {
        resultUI.SetResultTexts(_rank, _name, _score);
    }

    public void ShowResultUI()
    {
        resultUI.gameObject.SetActive(true);
    }
}
