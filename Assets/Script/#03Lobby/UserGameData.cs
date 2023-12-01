[System.Serializable]
public class UserGameData
{
    public int level; // 플레이어 레벨
    public float experience; // 플레이어 경험치
    public int gold; // 무료재화
    public int jewel; // 유료재화
    public int heart; // 게임 플레이에 소모되는 재화
    public int dailyBestScore;

    public void Reset()
    {
        level = 1;
        experience = 0;
        gold = 0;
        jewel = 0;
        heart = 30;
        dailyBestScore = 0;
    }
}
