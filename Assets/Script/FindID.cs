using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BackEnd;
using UnityEngine.UI;

public class FindID : LoginBase
{
    [SerializeField]
    private Image imageEmail;
    [SerializeField]
    private TMP_InputField inputFieldEmail;

    [SerializeField]
    private Button btnFindID;

    public void OnClickFindID()
    {
        //매개변수로 입력한 필드들 초기화
        ResetUI(imageEmail);

        //비었는지 체크
        if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "메일 주소")) return;

        if(!inputFieldEmail.text.Contains("@"))
        {
            GuideForIncorrectlyEnterData(imageEmail, "메일 형식이 잘못되었습니다.(ex. address@xx.xx) ");
            return;
        }

        //"아이디 찾기" 버튼 비활성화
        btnFindID.interactable = false;
        SetMessage("메일 발송중입니다.");

        //뒤끝 서버 접속
        FindCustomID();
    }

    private void FindCustomID()
    {
        Backend.BMember.FindCustomID(inputFieldEmail.text, callback =>
        {
            //버튼 활성화
            btnFindID.interactable = true;

            //메일 발송 성공
            if(callback.IsSuccess())
            {
                SetMessage($"{inputFieldEmail.text} 주소로 메일을 발송하였 습니다.");
            }
            //메일 발송 실패
            else
            {
                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 404: // 등록되지 않은 이메일
                        message = "해당 이메일을 사용하는 사용자가 없습니다.";
                        break;
                    case 429: // 24시간 이내 5회 이상 시도
                        message = "24시간 이내에 5회 이상 아이디/비밀번호 찾기를 시도했습니다.";
                        break;
                    default:
                        //statusCod : 400 => 프로젝트 명에 특수문자 추가로인한 메일발송에러
                        message = callback.GetMessage();
                        break;

                }

                if (message.Contains("이메일"))
                {
                    GuideForIncorrectlyEnterData(imageEmail, message);
                }
                else
                {
                    SetMessage(message);
                }
            }
        });
    }
}
