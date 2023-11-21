using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TopPanelViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textNickName;

    public void UpdateNickname()
    {
        //������ gamer_id ��� ������ �г��� 
        textNickName.text = UserInfo.Data.nickname == null? UserInfo.Data.gamerID : UserInfo.Data.nickname;
    }
}
