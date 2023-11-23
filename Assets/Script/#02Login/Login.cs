using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class Login : LoginBase
{
    [SerializeField]
    private Image imageID;
    [SerializeField]
    private TMP_InputField inputfieldID;
    [SerializeField]
    private Image imagedPW;
    [SerializeField]
    private TMP_InputField inputfieldPW;

    [SerializeField]
    private Button btnLogin;

    /// <summary>
    /// "로그인" 버튼을 눌렀을때
    /// </summary>
    public void OnclickLogin()
    {
        //매개 변수로 입력한 인풋필드의 색상과 메시지 초기화
        ResetUI(imageID, imagedPW);

        //필드값이 비어는지 체크
        if (IsFieldDataEmpty(imageID, inputfieldID.text, "아이디")) return;
        if (IsFieldDataEmpty(imagedPW, inputfieldPW.text, "비밀번호")) return;

        //로그인 버튼을 연타하지 못하도록 비활성화
        btnLogin.interactable = false;

        //서버에 로그인을 요청하는 동안 화면에 출력되는내용
        //ex) 로그인 관련 텍스트, 톱니바퀴 아이콘
        StartCoroutine(nameof(LoginProcess));
        //뒤끝서버 로그인 시도
        ReasponToLogin(inputfieldID.text, inputfieldPW.text);
    }
    /// <summary>
    /// 로그인 시도 후 서버로부터 전달받은 메시지 기반으로 처리
    /// </summary>
    private void ReasponToLogin(string ID,string PW)
    {
        Backend.BMember.CustomLogin(ID, PW, callback =>
        {
            StopCoroutine(nameof(LoginProcess));

            if (callback.IsSuccess())
            {
                SetMessage($"{inputfieldID.text}님 환영합니다.");

                //로비 씬 이동
                Utils.LoadScene(SceneNames.Lobby);
            }
            //로그인실패
            else
            {
                //실패했을떄는 다시 버튼 상호작용 활성화
                btnLogin.interactable = true;

                string message = string.Empty;

                Debug.Log(callback.GetMessage());
                switch(int.Parse(callback.GetStatusCode()) )
                {
                    case 401: // 존재하지않는 아이디, 잘못된 비밀번호
                        message = callback.GetMessage().Contains("customId") ? "존재하지 않는 아이디입니다." : "잘못된 비밀번호입니다.";
                        break;
                    case 403: // 차단당한 아이디
                        message = callback.GetMessage().Contains("user") ? "차단당한 유저정보입니다." : "차단당한 디바이스입니다.";
                        break;
                    case 410:
                        message = "탈퇴가 진행중인 유저입니다.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }

                //401코드에서 비밀번호 틀렸을때
                if(message.Contains("비밀번호"))
                {
                    GuideForIncorrectlyEnterData(imagedPW, message);
                }
                else
                {
                    GuideForIncorrectlyEnterData(imageID, message);
                }
            }
        });
    }
    private IEnumerator LoginProcess()
    {
        float time = 0;

        while (true)
        {
            time += Time.deltaTime;
            SetMessage($"로그인 중입니다...{time:F1}");

            yield return null;
        }
    }
}
