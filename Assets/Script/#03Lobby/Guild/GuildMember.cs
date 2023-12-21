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
        if (position.Equals("master")) position = "��帶����";
        else if (position.Equals("viceMaster")) position = "�ӿ�";
        else if (position.Equals("member")) position = "����";

        textPosition.text = position;
    }
    private void SetDate(string lastLogin)
    {
        //GetServerTime() - �����ð� �ҷ�����
        Backend.Utils.GetServerTime(callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"���� �ð� �ҷ����⿡ �����߽��ϴ�. : {callback}");
                return;
            }

            //json ������ �Ľ� ����
            try
            {
                //���� �����ð�
                string serverTime = callback.GetFlattenJSON()["utcTime"].ToString();
                //������� ���� �ð� = ����ð� - ���� ���� �ð�
                TimeSpan timeSpan = DateTime.Parse(serverTime) - DateTime.Parse(serverTime);

                if (timeSpan.TotalHours < 24) textLasgLogin.text = $"{timeSpan.TotalHours:F0}�ð� ��";
                else textLasgLogin.text = $"{timeSpan.TotalDays:F0}�� ��";
            }
            //������ �Ľ̽���
            catch (Exception e)
            {
                //t-c ����
                Debug.LogError(e);
            }
        });
    }
    public void OnClickMemberEdit()
    {
        //��� �����Ͱ� �ƴϸ� ���� ������ �� �� ����.
        if (!UserInfo.Data.nickname.Equals(backendGuildSystem.myGuildData.master.nickname)) return;

        //��� ������ ������ ������ ������ �� ����.
        if (UserInfo.Data.nickname.Equals(textNickname.text)) return;

        guildPage.OnClickMemberEdit(guildMemberData);
    }
}
