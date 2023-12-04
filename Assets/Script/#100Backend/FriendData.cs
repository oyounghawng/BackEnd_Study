public class FriendData
{
    public string nickname; //닉네임
    public string inDate; // 유저 indate
    public string createdAt; // 친구요청을 보낸시간, 받은시간, 친구가된시간
    public string lastLogin; //마지막 접솔 날짜

    //level과 같이 출력하고 싶은 유저정보가 있다면 추가
    public string level; //해당 유저의 레벨

    public override string ToString()
    {
        string result = string.Empty;
        result += $"닉네임 :{nickname}\n";
        result += $"inDate :{inDate}\n";
        result += $"createdAt :{createdAt}\n";
        result += $"lastLogin :{lastLogin}\n";
        result += $"level :{level}\n";

        return result;
    }
}
