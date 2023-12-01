public class FriendData
{
    public string nickname; //닉네임
    public string inDate; // 유저 indate
    public string createdAt; // 친구요청을 보낸시간, 받은시간, 친구가된시간

    public override string ToString()
    {
        string result = string.Empty;
        result += $"닉네임 :{nickname}\n";
        result += $"inDate :{inDate}\n";
        result += $"createdAt :{createdAt}\n";

        return result;
    }
}
