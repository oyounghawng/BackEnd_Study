using BackEnd;
using System;
using TMPro;
using UnityEngine;

public class GuildMember : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textPosition;
    [SerializeField]
    private TextMeshProUGUI textNickname;
    [SerializeField]
    private TextMeshProUGUI textGoodsCount;
    [SerializeField]
    private TextMeshProUGUI textLasgLogin;

    private BackendGuildSystem backendGuildSystem;
    private GuildPage guildPage;
    private GuildMemberData guildMemberData;

    public void Setup(BackendGuildSystem guildSystem, GuildPage guildPage, GuildMemberData memberData)
    {
        backendGuildSystem = guildSystem;
        this.guildPage = guildPage;
        guildMemberData = memberData;

        SetPosition(memberData.position);
        SetDate(memberData.lastLogin);

        textNickname.text = memberData.nickname;
        textGoodsCount.text = memberData.goodsCount.ToString();
    }
    private void SetPosition(string position)
    {
        if (position.Equals("master")) position = "길드마스터";
        else if (position.Equals("viceMaster")) position = "임원";
        else if (position.Equals("member")) position = "길드원";

        textPosition.text = position;
    }
    private void SetDate(string lastLogin)
    {
        //GetServerTime() - 서버시간 불러오기
        Backend.Utils.GetServerTime(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"서버 시간 불러오기에 실패했습니다. : {callback}");
                return;
            }

            //json 데이터 파싱 성공
            try
            {
                //현재 서버시간
                string serverTime = callback.GetFlattenJSON()["utcTime"].ToString();
                //만료까지 남은 시간 = 만료시간 - 현재 서버 시간
                TimeSpan timeSpan = DateTime.Parse(serverTime) - DateTime.Parse(serverTime);

                if (timeSpan.TotalHours < 24) textLasgLogin.text = $"{timeSpan.TotalHours:F0}시간 전";
                else textLasgLogin.text = $"{timeSpan.TotalDays:F0}일 전";
            }
            //데이터 파싱실패
            catch (Exception e)
            {
                //t-c 에러
                Debug.LogError(e);
            }
        });
    }
    public void OnClickMemberEdit()
    {
        //길드 마스터가 아니면 길드원 편집을 할 수 없다.
        if (!UserInfo.Data.nickname.Equals(backendGuildSystem.myGuildData.master.nickname)) return;

        //길드 마스터 본인의 정보는 편집할 수 없다.
        if (UserInfo.Data.nickname.Equals(textNickname.text)) return;

        guildPage.OnClickMemberEdit(guildMemberData);
    }
}
