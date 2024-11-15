using UnityEngine;
using TMPro;

public class CurrentUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text_curPlayer = null;
    [SerializeField]
    private TextMeshProUGUI text_curRound = null;

    public void SetCurrentPlayerText(string _name)
    {
        text_curPlayer.text = string.Format("PLAYER: {0}", _name);
    }

    public void SetCurrentRoundText(int _round)
    {
        text_curRound.text = string.Format("ROUND: {0}", _round);
    }
}
