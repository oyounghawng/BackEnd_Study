using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class DailyRankLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject rankDataPrefab;
    [SerializeField]
    private Scrollbar scrollbar;
    [SerializeField]
    private Transform rankDataParent;
    [SerializeField]
    private DailyRankData myRankData;

    private List<DailyRankData> rankDataList;

    private void Awake()
    {
        rankDataList = new List<DailyRankData>();

        //1~20 �� ��ŷ��Ƽ�� ��� 
        for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
        {
            GameObject clone = Instantiate(rankDataPrefab, rankDataParent);
            rankDataList.Add(clone.GetComponent<DailyRankData>());
        }
    }

    private void OnEnable()
    {
        //1�� ��ŷ�� ���̵��� ������
        scrollbar.value = 1;
        // 1~20 �� ��ŷ ���� �ҷ�����
        GetRankList();
        //�� ��ŷ ���� �ҷ�����
        GetMyRank();
    }
    private void GetRankList()
    {
        //1~20�� ��ŷ���� �ҷ�����
        Backend.URank.User.GetRankList(Constants.DALIY_RANK_UUID, Constants.MAX_RANK_LIST, callback =>
        {
            if (callback.IsSuccess())
            {
                //json ������ �Ľ̼���
                try
                {
                    Debug.Log($"��ŷ ��ȸ�� �����߽��ϴ�. : {callback}");

                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    if (rankDataJson.Count <= 0)
                    {
                        //1~20 �� �� �����ͷ� ����
                        for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
                        {
                            SetRankData(rankDataList[i], i + 1, "-", 0);

                        }
                        Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�");
                    }
                    else
                    {
                        int rankerCount = rankDataJson.Count;

                        for (int i = 0; i < rankerCount; ++i)
                        {
                            rankDataList[i].Rank = int.Parse(rankDataJson[i]["rank"].ToString());
                            rankDataList[i].Score = int.Parse(rankDataJson[i]["score"].ToString());

                            //�г����� ������ �������� ���� ������ ���� �� �� �ֱ� ������
                            //�г����� �������� �ʴ� ������ �г��� ��� gamerID���
                            rankDataList[i].Nickname = rankDataJson[i].ContainsKey("Nickname") == true ?
                                                        rankDataJson[i]["nickname"]?.ToString() : UserInfo.Data.gamerID;
                        }
                        //���� rankerCount �� 20������ �������� ������ �������� �� �����͸� ����
                        for (int i = rankerCount; i < Constants.MAX_RANK_LIST; ++i)
                        {
                            SetRankData(rankDataList[i], i + 1, "-", 0);
                        }
                    }
                }
                //json �Ľ̿���
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
            else
            {
                //1~20������ �����ͷ� ����
                for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
                {
                    SetRankData(rankDataList[i], i + 1, "-", 0);
                }

                Debug.LogError($"��ŷ ��ȸ �� ������ �߻��߽��ϴ�.");
            }
        });
    }
    private void GetMyRank()
    {
        //�� ��ŷ���� �ҷ�����
        Backend.URank.User.GetMyRank(Constants.DALIY_RANK_UUID, callback =>
        {
            //�г����� ������ GamerID ������ �г��� ���
            string nickname = UserInfo.Data.nickname == null ? UserInfo.Data.gamerID : UserInfo.Data.nickname;

            if (callback.IsSuccess())
            {
                //json ������ �Ľ̼���
                try
                {
                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    //�޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
                    if (rankDataJson.Count <= 0)
                    {
                        // ["������ ����","�г���",0] �������� ���
                        SetRankData(myRankData, 1000000000, nickname, 0);
                        Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�");
                    }

                    else
                    {
                        myRankData.Rank = int.Parse(rankDataJson[0]["rank"].ToString());
                        myRankData.Score = int.Parse(rankDataJson[0]["score"].ToString());

                        //�г����� ������ �������� ���� ������ ���� �� �� �ֱ� ������
                        //�г����� �������� �ʴ� ������ �г��� ��� gamerID���
                        myRankData.Nickname = rankDataJson[0].ContainsKey("Nickname") == true ?
                                                    rankDataJson[0]["nickname"]?.ToString() : UserInfo.Data.gamerID;
                    }
                }
                //json �Ľ̿���
                catch (System.Exception e)
                {
                    // ["������ ����","�г���",0] �������� ���
                    SetRankData(myRankData, 1000000000, nickname, 0);
                    //try - catch ����
                    Debug.LogError(e);
                }
            }
            else
            {
                if(callback.GetMessage().Contains("userRank"))
                {
                    // ["������ ����","�г���",0] �������� ���
                    SetRankData(myRankData, 1000000000, nickname, 0);
                }
            }

        });
    }
    private void SetRankData(DailyRankData rankData, int rank, string nickname, int score)
    {
        rankData.Rank = rank;
        rankData.Nickname = nickname;
        rankData.Score = score;
    }
}
