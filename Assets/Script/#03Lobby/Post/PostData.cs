using System.Collections.Generic;
public class PostData
{
    public string title;
    public string content;
    public string inDate;
    public string expirationDate;

    public bool isCanReceive = false;

    //아이템 이름 아이템 개수
    public Dictionary<string, int> postReward = new Dictionary<string, int>();

    //우편정보를 log로 출력
    public override string ToString()
    {
        string result = string.Empty;
        result += $"title : {title}\n";
        result += $"content : {content}\n";
        result += $"inDate : {inDate}\n";

        if (isCanReceive)
        {
            result += "우편아이템\n";

            foreach (string itemKey in postReward.Keys)
            {
                result += $"| {itemKey} : {postReward[itemKey]}개\n";
            }
        }
        else
        {
            result += "지원하지 않는 아이템입니다.";
        }

        return result;

    }

}
