using UnityEngine;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] text_resultScores = null;

    public void SetResultTexts(int _rank, string _name, int _score)
    {
        text_resultScores[_rank].text += string.Format(" {0} {1}", _name, _score);
    }
}
