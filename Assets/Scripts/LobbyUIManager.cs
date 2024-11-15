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

    // ������ �÷��̾� �г��� UI���� ����
    public void UpdateNicknameUIs(string[] _nickNames, int _hilightedIdx)
    {
        for (int i = 0; i < nickNameTexts.Length; i++)
        {
            // Debug.Log(_nickNames[i]);
            nickNameTexts[i].text = string.Format("Player {0}: {1}", i + 1, _nickNames[i]);
        }

        // nickNameTexts[_hilightedIdx].color = Color.yellow;
    }

    // Ÿ��Ʋ ȭ�鿡�� �г��� ��ǲ�ʵ� ���� �� ���ε�
    public void OnNickNameValueChanged(string _value)
    {
        curNickname = _value;
    }
}
