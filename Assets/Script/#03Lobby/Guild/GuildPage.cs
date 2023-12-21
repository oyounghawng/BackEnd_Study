using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuildPage : MonoBehaviour
{
    [SerializeField]
    private BackendGuildSystem backendGuildSystem;
    [SerializeField]
    private TextMeshProUGUI textGuildName;
    [SerializeField]
    private Notice notice;
    [SerializeField]
    private GameObject executivesOption;
    [SerializeField]
    private TextMeshProUGUI textMemberCount;
    [SerializeField]
    private GameObject overlayBackground;
    [SerializeField]
    private GuildMemberEdit popupMemberEdit;

    [SerializeField]
    private GameObject memberPrefab;
    [SerializeField]
    private Transform parentContent;

    private string guildName = string.Empty;
    private MemoryPool memoryPool;

    private void Awake()
    {
        memoryPool = new MemoryPool(memberPrefab, parentContent);
    }

    public void Setup(string guildName, bool isMaster = false, bool isOtherGuild = false)
    {
        notice.Setup(isMaster, isOtherGuild);
        executivesOption.SetActive(isMaster);

        gameObject.SetActive(true);

        textGuildName.text = guildName;
        this.guildName = guildName;

        if (isOtherGuild == true)
        {
            textMemberCount.text = $"길드 인원 {backendGuildSystem.myGuildData.memberCount}/100";
            backendGuildSystem.GetGuildMemberList(backendGuildSystem.otherGuildData.guildInDate);
        }
        else
        {
            textMemberCount.text = $"길드 인원 {backendGuildSystem.myGuildData.memberCount}/100";
            backendGuildSystem.GetGuildMemberList(backendGuildSystem.myGuildData.guildInDate);
        }
    }
    public void Activate(GuildMemberData member)
    {
        GameObject item = memoryPool.ActivatePoolItem();
        item.GetComponent<GuildMember>().Setup(backendGuildSystem, this,member);
    }
    public void Deactivate(GameObject member)
    {
        memoryPool.DeactivatePoolItem(member);
    }
    public void DeactivateAll()
    {
        memoryPool.DeactivateAllPoolItems();
    }
    public void OnClickApplyGuild()
    {
        backendGuildSystem.ApplyGuild(guildName);
    }
    public void SucessWithdrawGuild()
    {
        gameObject.SetActive(false);
    }
    public void OnClickMemberEdit(GuildMemberData guildMemberData)
    {
        overlayBackground.SetActive(true);
        popupMemberEdit.gameObject.SetActive(true);

        popupMemberEdit.Setup(guildMemberData);
    }
}
