using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GuildCreatePage : MonoBehaviour
{
    [SerializeField]
    private BackendGuildSystem backendGuildSystem;
    [SerializeField]
    private TMP_InputField inputFieldGuildName;

    public void OnClickCreateGuild()
    {
        string guildName = inputFieldGuildName.text;

        if(guildName.Trim().Equals(""))
        {
            return;
        }

        inputFieldGuildName.text = "";

        //길드 생성
        backendGuildSystem.CreatedGuild(guildName);
    }

    public void SucessCreateGuild()
    {
        gameObject.SetActive(false);
    }
}
