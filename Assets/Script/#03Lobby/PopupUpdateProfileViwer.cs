using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopupUpdateProfileViwer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textNickname;
    [SerializeField]
    private TextMeshProUGUI textGamerID;

    public void UpdateNickname()
    {
        //�г��� ���� ����?

        textNickname.text = UserInfo.Data.nickname == null ? UserInfo.Data.gamerID : UserInfo.Data.nickname;

        //gamer id ���
        textGamerID.text = UserInfo.Data.gamerID;

    }
}
