using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TopPanelViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textNickName;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private Slider sliderExperience;
    [SerializeField]
    private TextMeshProUGUI textHeart;
    [SerializeField]
    private TextMeshProUGUI textJewel;
    [SerializeField]
    private TextMeshProUGUI textGold;
    private void Awake()
    {
        BackendGameData.Instance.onGameDataLoadEvenet.AddListener(UpdataGameData);
    }
    public void UpdateNickname()
    {
        //없으면 gamer_id 출력 있으면 닉네임 
        textNickName.text = UserInfo.Data.nickname == null? UserInfo.Data.gamerID : UserInfo.Data.nickname;
    }

    public void UpdataGameData()
    {
        textLevel.text = $"{BackendGameData.Instance.UserGameData.level}";
        // 임시로 최대 경험치 100으로 설정
        sliderExperience.value = BackendGameData.Instance.UserGameData.experience / 100;
        textHeart.text = $"{BackendGameData.Instance.UserGameData.heart}";
        textJewel.text = $"{BackendGameData.Instance.UserGameData.jewel}";
        textGold.text = $"{BackendGameData.Instance.UserGameData.gold}";
    }
}
