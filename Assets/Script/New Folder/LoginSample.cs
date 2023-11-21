using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
public class LoginSample : MonoBehaviour
{
    private void Awake()
    {
        string ID = "ASD1";
        string PW = "1234";
        string email = "asd@gmail.com";
        string nickname = "유저1";

        //회원가입
        Backend.BMember.CustomSignUp(ID, PW);

        //이메일 설정
        Backend.BMember.UpdateCustomEmail(email);

        //로그인
        Backend.BMember.CustomLogin(ID, PW);

        //아이디 찾기
        Backend.BMember.FindCustomID(email);

        //비밀번호찾기
        Backend.BMember.ResetPassword(ID, email);

        //닉네임설정
        //닉네임 없을떄 최초설정
        Backend.BMember.CreateNickname(nickname);
        //이미 있는 닉네임 수정 (없으면 creat실행)
        Backend.BMember.UpdateNickname(nickname);
    }
}
