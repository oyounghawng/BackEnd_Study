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
        //닉네임 존재 여부?

        textNickname.text = UserInfo.Data.nickname == null ? UserInfo.Data.gamerID : UserInfo.Data.nickname;

        //gamer id 출력
        textGamerID.text = UserInfo.Data.gamerID;

    }
}
