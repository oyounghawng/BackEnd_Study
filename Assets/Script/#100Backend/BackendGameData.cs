using BackEnd;
using UnityEngine;
using UnityEngine.Events;
public class BackendGameData
{
    [System.Serializable]
    public class GameDataLoadEvent : UnityEvent { }
    public GameDataLoadEvent onGameDataLoadEvenet = new GameDataLoadEvent();

    private static BackendGameData instance = null;
    public static BackendGameData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BackendGameData();
            }

            return instance;
        }
    }
    private UserGameData userGameData = new UserGameData();
    public UserGameData UserGameData => userGameData;

    private string gameDataRowIndate = string.Empty;

    /// <summary>
    /// �ڳ� �ܼ� ���̺� ���ο� �������� �߰�
    /// </summary>
    public void GameDataInsert()
    {
        //�ʱⰪ ����
        userGameData.Reset();

        //���̺� �߰��� �����ͷ� ����
        Param param = new Param()
        {
            {"level", userGameData.level},
            {"experience", userGameData.experience},
            {"gold", userGameData.gold},
            {"jewel", userGameData.jewel},
            {"heart", userGameData.heart},
        };
        GameDataInsert(Constants.USER_DATA_TABLE, param);
    }
    private void GameRankDataInsert()
    {
        UserGameData.dailyBestScore = 0;

        Param rankParm = new Param()
        {
            {"dailyBestScore", userGameData.dailyBestScore},
        };
        GameDataInsert(Constants.USER_RANK_DATA_TABLE, rankParm);
    }
    private void GameDataInsert(string tableNmae, Param param)
    {
        // ù��° �Ű������� �ڳ� �ܼ��� "�������� ���� �ǿ� ������ ���̺� �̸�"
        Backend.GameData.Insert(tableNmae, param, callback =>
        {
            //���� ���� �߰��� ���� ���� ��
            if (callback.IsSuccess())
            {
                // ���� ������ ������
                gameDataRowIndate = callback.GetInDate();

                Debug.Log($"���� ���� ������ ���Կ� �����߽��ϴ� : {callback}");

                onGameDataLoadEvenet?.Invoke();
            }
            //����
            else
            {
                Debug.Log($"���� ���� ������ ���Կ� �����߽��ϴ� : {callback}");
            }
        });
    }

    /// <summary>
    /// �ڳ� �ܼ� ���̺��� �������� ȣ��
    /// </summary>
    public void GameDataLoad()
    {
        Backend.GameData.GetMyData(Constants.USER_DATA_TABLE, new Where(), callback =>
        {
            //�������� ��
            if (callback.IsSuccess())
            {
                Debug.Log($"���� ���� ������ �ҷ����⿡ �����߽��ϴ�. : {callback}");

                //json ������ �Ľ� ����
                try
                {
                    LitJson.JsonData gameDataJson = callback.FlattenRows();

                    //������ 0�̸� ���°�
                    if (gameDataJson.Count <= 0)
                    {
                        Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");

                        //���� ������ ������ ���� ����
                        GameDataInsert();
                    }
                    else
                    {
                        //�ҷ��� ���������� ������
                        gameDataRowIndate = gameDataJson[0]["inDate"].ToString();
                        //�ҷ��� ���� ������ usergamedata������ ����
                        UserGameData.level = int.Parse(gameDataJson[0]["level"].ToString());
                        UserGameData.experience = int.Parse(gameDataJson[0]["experience"].ToString());
                        UserGameData.gold = int.Parse(gameDataJson[0]["gold"].ToString());
                        UserGameData.jewel = int.Parse(gameDataJson[0]["jewel"].ToString());
                        UserGameData.heart = int.Parse(gameDataJson[0]["heart"].ToString());

                        onGameDataLoadEvenet?.Invoke();
                    }
                }
                //�Ľ� ���н� ������ �ʱ�ȭ
                catch (System.Exception e)
                {
                    UserGameData.Reset();
                    // try -cath �������
                    Debug.LogError(e);
                }
            }
            //�������� ��
            else
            {
                Debug.LogError($"�������� ������ �ҷ����⿡ �����߽��ϴ� : {callback}");
            }
        });

        Backend.GameData.GetMyData(Constants.USER_RANK_DATA_TABLE, new Where(), callback =>
        {
            //�������� ��
            if (callback.IsSuccess())
            {
                Debug.Log($"���� ��ŷ ������ �ҷ����⿡ �����߽��ϴ�. : {callback}");

                //json ������ �Ľ� ����
                try
                {
                    LitJson.JsonData gameDataJson = callback.FlattenRows();
                    //������ 0�̸� ���°�
                    if (gameDataJson.Count <= 0)
                    {
                        Debug.LogWarning("��ŷ �����Ͱ� �������� �ʽ��ϴ�.");

                        GameRankDataInsert();
                    }
                }
                //�Ľ� ���н� ������ �ʱ�ȭ
                catch (System.Exception e)
                {
                    // try -cath �������
                    Debug.LogError(e);
                }
            }
            //�������� ��
            else
            {
                Debug.LogError($"������ ��ŷ ������ �ҷ����⿡ �����߽��ϴ� : {callback}");
            }
        });
    }

    //update2�� update ���̴� 2������� Ư�� row �˻������ؼ� ����
    /// <summary>
    /// �ڳ� ���̺� �ִ� ���� ������ ����
    /// </summary>
    public void GameDataUpdate(UnityAction action = null)
    {
        if (userGameData == null)
        {
            Debug.LogError("�������� �ٿ� �ްų� ���� ������ �����Ͱ� �������� �ʽ��ϴ�." +
                  "Insert Ȥ�� load�� ���� �����͸� ������ �ּ���");
            return;
        }

        Param param = new Param()
        {
            {"level", userGameData.level},
            {"experience", userGameData.experience},
            {"gold", userGameData.gold},
            {"jewel", userGameData.jewel},
            {"heart", userGameData.heart},
        };

        //���� ������ ������(gamedatarowindate)�� ������ �����޽��� ���
        if (string.IsNullOrEmpty(gameDataRowIndate))
        {
            Debug.LogError("������ indate ������ ���� �������� ������ ������ �����߽��ϴ�.");
        }
        //���� ������ �������� ������ ���̺� ����Ǿ� �ִ� ���� indat �÷��� ����
        //�����ϴ� ������ onwerindate�� ��ġ�ϴ� row�� �˻��Ͽ� �����ϴ� updatev2ȣ��
        else
        {
            Debug.Log($"{gameDataRowIndate}�� ���� ���� ������ ������ ��û�մϴ�.");

            Backend.GameData.UpdateV2(Constants.USER_DATA_TABLE, gameDataRowIndate, Backend.UserInDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log($"���� ���� ������ ������ �����߽��ϴ�. : {callback}");

                    action?.Invoke();

                    onGameDataLoadEvenet?.Invoke();
                }
                else
                {
                    Debug.LogError($"���� ���� ������ ������ �����߽��ϴ�. : {callback}");
                }

            });
        }
    }
}