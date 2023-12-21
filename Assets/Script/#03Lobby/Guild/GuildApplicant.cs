using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuildApplicant : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textNickname;
    [SerializeField]
    private TextMeshProUGUI textLevel;

    private BackendGuildSystem backendGuildSystem;
    private GuildApplicantsPage guildApplicantsPage;
    private GuildMemberData guildMemberData;
    public void Setup(BackendGuildSystem guildSystem, GuildApplicantsPage applicantsPage, GuildMemberData memberData)
    {
        textNickname.text = memberData.nickname;
        textLevel.text = $"Lv. {memberData.level}";

        backendGuildSystem = guildSystem;
        guildApplicantsPage = applicantsPage;
        guildMemberData = memberData;
    }
    public void OnClickApproveApplicant()
    {
        //해당 유저의 UI오브젝트 삭제
        guildApplicantsPage.Deactivate(gameObject);
        //해당 유저의 길드 가입 요청 수록
        backendGuildSystem.ApproveApplicant(guildMemberData.inDate);
    }
    public void OnClickRejectApplicant()
    {
        //해당 유저의 UI오브젝트 삭제
        guildApplicantsPage.Deactivate(gameObject);
        //해당 유저의 길드 가입 요청 수록
        backendGuildSystem.RejectApplicant(guildMemberData.inDate);
    }
}
