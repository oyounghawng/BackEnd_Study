using System.Collections.Generic;
public class GuildData
{
    public string guildName;  //��� �̸�
    public string guildInDate; //��� indate
    public string notice;
    public int memberCount; // ��� �ο���
    public GuildMemberData master; //��� ������
    public List<GuildMemberData> viceMasterList; // ��� �θ�����
}
