using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System.Net.NetworkInformation;
public class DailyRankRegister : MonoBehaviour
{
    public void Process(int newScore)
    {
        //UpdateMyRankData(newScore);
        UpdateMyBestRankData(newScore);
        
    }
    private void UpdateMyRankData(int newScore)
    {
        string rowInDate = string.Empty;

        //��ŷ �����͸� ������Ʈ �ϱ� ���ؼ��� ���� �����Ϳ� ����ϴ� indate���� �ʿ�
        Backend.GameData.GetMyData(Constants.USER_RANK_DATA_TABLE, new Where(), callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"������ ��ȭ �� ������ �߻��߽��ϴ�. : {callback}");
                return;
            }

            Debug.Log($"������ ��ȸ�� �����߽��ϴ�. : {callback}");

            if (callback.FlattenRows().Count > 0)
            {
                rowInDate = callback.FlattenRows()[0]["inDate"].ToString();
            }
            else
            {
                Debug.LogError("�����Ͱ� �������� �ʽ��ϴ�.");
                return;
            }

            Param param = new Param()
            {
                {"dailyBestScore", newScore}
            };

            Backend.URank.User.UpdateUserScore(Constants.DALIY_RANK_UUID, Constants.USER_RANK_DATA_TABLE, rowInDate, param
                , callback =>
                {
                    if(callback.IsSuccess())
                    {
                        Debug.Log($"��ŷ ��Ͽ� �����߽��ϴ� : {callback}");
                    }
                    else
                    {
                        Debug.LogError($"��ŷ��Ͽ� �����߽��ϴ� : {callback}");
                    }
                });

        });
    }
    private void UpdateMyBestRankData(int newScore)
    {
        Backend.URank.User.GetMyRank(Constants.DALIY_RANK_UUID, callback =>
        {
            if(callback.IsSuccess() )
            {
                //json ������ �Ľ� ����
                try
                {
                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    //�޾ƿ� �������� ������ 0�̸� �����Ͱ� ���°�
                    if(rankDataJson.Count <= 0)
                    {
                        Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
                    }
                    else
                    {
                        //��ŷ�� ��� �� ���� �÷����� "dailyBestScore�� �������
                        //�ҷ��� ���� �÷����� score�� ���ϵǾ� �ִ�.

                        //�߰��� ����� �׸��� �÷����� �״�� ���
                        int betsScore = int.Parse(rankDataJson[0]["score"].ToString());

                        //���� ������ �ְ� �������� ������
                        if(newScore > betsScore)
                        {
                            UpdateMyRankData(newScore);

                            Debug.Log($"�ְ� ���� ���� {betsScore} -> {newScore}");
                        }
                    }
                }
                //json �Ľ� ����
                catch(System.Exception e)
                {
                    //try-catch ���� ���
                    Debug.LogError(e);
                }
            }
            else
            {
                //�ڽ��� ��ŷ ������ �������� ���� ���� ���� ������ ���ο� ��ŷ���� ���
                if(callback.GetMessage().Contains("userRank"))
                {
                    UpdateMyRankData(newScore);

                    Debug.Log($"���ο� ��ŷ������ ������ ��� {callback}");
                }
            }
        });
    }
}
