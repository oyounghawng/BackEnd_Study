public class FriendData
{
    public string nickname; //�г���
    public string inDate; // ���� indate
    public string createdAt; // ģ����û�� �����ð�, �����ð�, ģ�����Ƚð�
    public string lastLogin; //������ ���� ��¥

    //level�� ���� ����ϰ� ���� ���������� �ִٸ� �߰�
    public string level; //�ش� ������ ����

    public override string ToString()
    {
        string result = string.Empty;
        result += $"�г��� :{nickname}\n";
        result += $"inDate :{inDate}\n";
        result += $"createdAt :{createdAt}\n";
        result += $"lastLogin :{lastLogin}\n";
        result += $"level :{level}\n";

        return result;
    }
}
