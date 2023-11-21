using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;
public class Nickname : LoginBase
{
    [System.Serializable]
    public class NicknameEvent : UnityEngine.Events.UnityEvent { }

    public NicknameEvent onNicknameEvent = new NicknameEvent();

    [SerializeField]
    private Image imageNickname;  //닉네임 필드 색상변경
    [SerializeField]
    private TMP_InputField inputFieldNickname; //입력필드

    [SerializeField]
    private Button btnUpdateNickname; // 닉네임 설정 버튼

    private void OnEnable()
    {
        //변경에 실패해 에러메세지 출력한상태에서
        //닫았다 열수 있기 떄문에 상태 초기화
        ResetUI(imageNickname);
        SetMessage("닉네임을 입력하세요");

    }

    public void OnClickUpdateNickName()
    {
        //매개변수 입력한 인풋필드와 메시지 초기화
        ResetUI(imageNickname);

        //비엇는지 체크
        if (IsFieldDataEmpty(imageNickname, inputFieldNickname.text, "Nickname")) return;

        //상호작용 비활성화(중복실행 방지)
        btnUpdateNickname.interactable = false;
        SetMessage("닉네임 변경 중입니다..");

        //뒤끝서버 변경시도
        UpdateNickname();
    }

    private void UpdateNickname()
    {
        //닉네임 설정
        Backend.BMember.UpdateNickname(inputFieldNickname.text, callback =>
        {
            //버튼 활성화
            btnUpdateNickname.interactable = true;

            //변경 성공
            if(callback.IsSuccess())
            {
                SetMessage($"{inputFieldNickname.text}(으)로 닉네임이 변경되었습니다.");

                //이벤트 메소드 출력
                onNicknameEvent?.Invoke();
            }
            //실패
            else
            {
                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 400: // 공백이 있꺼나 20자이상 제한사항에 걸린 닉네임
                        message = "닉네임이 비었거나, 20자이상이거나, 앞/뒤에 공백이 있습니다.";
                        break;
                    case 409: //중복된 닉네임
                        message = "이미 존재하는 닉네임 입니다.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }
            }
        });
    }
}
