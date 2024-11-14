using UnityEngine;
using TMPro;

using Photon.Pun;

public class LobbyUIManager : MonoBehaviourPun
{
    [SerializeField]
    private TextMeshProUGUI[] nickNameTexts = null;

    private string curNickname = string.Empty;

    public string CurNickName
    {
        get { return curNickname; }
    }

    // 대기실의 플레이어 닉네임 UI들을 갱신
    [PunRPC]
    public void UpdateNicknameUIs(string[] _nickNames)
    {
        for(int i = 0; i < nickNameTexts.Length; i++)
        {
            // Debug.Log(_nickNames[i]);
            nickNameTexts[i].text = string.Format("Player {0}: {1}", i + 1, _nickNames[i]);
        }
    }

    // 타이틀 화면에서 닉네임 인풋필드 변경 시 바인딩
    public void OnNickNameValueChanged(string _value)
    {
        curNickname = _value;
    }
}
