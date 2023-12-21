public class GuildMemberData
{
    public string nickname;
    public string inDate;
    public string level;
    public string position; //��� ��å
    public int goodsCount; //���� ����
    public string lastLogin; //������ ���� ��¥

    public override string ToString()
    {
        string result = string.Empty;
        result += $"nickname : {nickname}\n";
        result += $"inDate : {inDate}\n";
        result += $"level : {level}\n";
        result += $"position : {position}\n";
        result += $"goods : {goodsCount}\n";
        result += $"lastLogin : {lastLogin}\n";

        return base.ToString();
    }
}
