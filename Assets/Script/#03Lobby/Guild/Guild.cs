using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Guild : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textGuildName;
    [SerializeField]
    private TextMeshProUGUI textMasterNickname;
    [SerializeField]
    private TextMeshProUGUI textMemberCount;

    private BackendGuildSystem backendguildSystem;
    private GuildData guildData;

    public void Setup(BackendGuildSystem guildSystem, GuildData guildData)
    {
        backendguildSystem = guildSystem;
        this.guildData = guildData;

        textGuildName.text = guildData.guildName;
        textMasterNickname.text = guildData.master.nickname;
        textMemberCount.text = $"{guildData.memberCount}/100";
    }

    public void OnClickGuildInofo()
    {
        backendguildSystem.GetGuildInfo(guildData.guildInDate);
    }
}
