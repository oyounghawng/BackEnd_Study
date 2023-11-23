using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool IsGameOver { set; get; } = false;
    public void GameOver()
    {
        //�ߺ� ó�� ���� �ʵ��� bool ������ ����
        if (IsGameOver == true) return;

        IsGameOver = false;

        //����ġ ���� �� ������ ���� �˻�
        //���� ���� �ý��ۿ� ���� ������ ���⿡ �ִ밪 100
        //�÷��� �Ҷ����� 25�� ����
        BackendGameData.Instance.UserGameData.experience += 25;
        if(BackendGameData.Instance.UserGameData.experience >= 100)
        {
            BackendGameData.Instance.UserGameData.experience = 0;
            BackendGameData.Instance.UserGameData.level++;
        }
        //�������� ������Ʈ

        BackendGameData.Instance.GameDataUpdate(AfterGameOver);
        
        
    }

    public void AfterGameOver()
    {
        // �κ� ������ �̵�
        Utils.LoadScene(SceneNames.Lobby);
    }
}
