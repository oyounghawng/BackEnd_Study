using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool IsGameOver { set; get; } = false;
    public void GameOver()
    {
        //중복 처리 되지 않도록 bool 변수로 제어
        if (IsGameOver == true) return;

        IsGameOver = false;

        //경험치 증가 및 레벨업 여부 검사
        //현재 레벨 시스템에 대한 설정이 없기에 최대값 100
        //플래이 할때마다 25씩 증가
        BackendGameData.Instance.UserGameData.experience += 25;
        if(BackendGameData.Instance.UserGameData.experience >= 100)
        {
            BackendGameData.Instance.UserGameData.experience = 0;
            BackendGameData.Instance.UserGameData.level++;
        }
        //게임정보 업데이트

        BackendGameData.Instance.GameDataUpdate(AfterGameOver);
        
        
    }

    public void AfterGameOver()
    {
        // 로비 씬으로 이동
        Utils.LoadScene(SceneNames.Lobby);
    }
}
