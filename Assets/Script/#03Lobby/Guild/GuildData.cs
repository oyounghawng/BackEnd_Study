using System.Collections.Generic;
public class GuildData
{
    public string guildName;  //길드 이름
    public string guildInDate; //길드 indate
    public string notice;
    public int memberCount; // 길드 인원수
    public GuildMemberData master; //길드 마스터
    public List<GuildMemberData> viceMasterList; // 길드 부마스터
}
