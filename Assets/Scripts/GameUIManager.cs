using UnityEngine;
using TMPro;

using Photon.Pun;

public class GameUIManager : MonoBehaviourPun
{
    [SerializeField]
    private TextMeshProUGUI text_CurPlayer = null;

    [SerializeField]
    private TextMeshProUGUI text_CurRound = null;
}
