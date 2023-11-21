using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BackEnd;
using UnityEngine.UI;
public class RegisterAccount : LoginBase
{
    /// <summary>
    /// image : 이미지 프레임 inpufield : 입력란
    /// </summary>
    [SerializeField]
    private Image imageID;
    [SerializeField]
    private TMP_InputField inputFieldID;
    [SerializeField]
    private Image imagePW;
    [SerializeField]
    private TMP_InputField inputFieldPW;
    [SerializeField]
    private Image imageConfirmPW;
    [SerializeField]
    private TMP_InputField inputFieldConfirmPW;
    [SerializeField]
    private Image imageEmail;
    [SerializeField]
    private TMP_InputField inputFieldEmail;

    [SerializeField]
    private Button btnRegisterAccount;

    /// <summary>
    /// 계정생성 버튼을 눌렀을때 호출
    /// </summary>
    public void OnClickRegisterAccount()
    {
        //매개변수로 인식한 인풋필드의 색상과 메시지 내용 초기화
        ResetUI(imageID,imagePW,imageConfirmPW,imageEmail);

        //비었는지 체크
        if (IsFieldDataEmpty(imageID, inputFieldID.text, "아이디")) return;
        if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "비밀번호")) return;
        if (IsFieldDataEmpty(imageConfirmPW, inputFieldConfirmPW.text, "비밀번호 확인")) return;
        if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "메일 주소")) return;

        //비밀번호와 비밀번호 확인이 다를때
        if(!inputFieldPW.text.Equals(inputFieldConfirmPW.text))
        {
            GuideForIncorrectlyEnterData(imageConfirmPW, "비밀번호가 일치하지 않습니다.");
            return;
        }

        //메일 형식검사
        if (!inputFieldPW.text.Contains("@"))
        {
            GuideForIncorrectlyEnterData(imageEmail, "메일형식이 잘못되었습니다. (ex. address@xx.xx) ");
            return;
        }

        //계정 생성 버튼의 상호작용 비활성화
        btnRegisterAccount.interactable = false;
        SetMessage("계정 생성중입니다.");

        //뒤끛 서버 계쩡생성 시도
        CustomSingUp();
    }

    /// <summary>
    /// 계정 생성 시도후 서버와 통신
    /// </summary>
    private void CustomSingUp()
    {
        Backend.BMember.CustomSignUp(inputFieldID.text, inputFieldPW.text, callback =>
        {
            //계정생성 버튼 활성화
            btnRegisterAccount.interactable = true;

            if(callback.IsSuccess())
            {
                Backend.BMember.UpdateCustomEmail(inputFieldEmail.text, callback =>
                {
                    if (callback.IsSuccess())
                    {
                        SetMessage($"계정 생성 성공.{inputFieldEmail}");

                        //로비 씬 이동
                        Utils.LoadScene(SceneNames.Lobby);
                    }
                });
            }
            //계정 생성 실패
            else
            {
                string message = string.Empty;

                switch(int.Parse(callback.GetStatusCode())) 
                {
                    case 409: // 중복된 아이디
                        message = "이미 존재하는 아이디 입니다.";
                        break;
                    case 403: //차단당한 아이디

                        break;
                    case 401://프로젝트 상대가 '점검'중

                        break;
                    case 400://디바이스 정보가 null

                        break;
                    default:
                        message = callback.GetMessage();
                        break;

                }

                if(message.Contains("아이디"))
                {
                    GuideForIncorrectlyEnterData(imageID, message);
                }
                else
                {
                    SetMessage(message);
                }
            }
        });
        
    }

}
