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
        //없으면 gamer_id 출력 있으면 닉네임 
        textNickName.text = UserInfo.Data.nickname == null? UserInfo.Data.gamerID : UserInfo.Data.nickname;
    }
}
