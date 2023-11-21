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
    /// "�α���" ��ư�� ��������
    /// </summary>
    public void OnclickLogin()
    {
        //�Ű� ������ �Է��� ��ǲ�ʵ��� ����� �޽��� �ʱ�ȭ
        ResetUI(imageID, imagedPW);

        //�ʵ尪�� ������ üũ
        if (IsFieldDataEmpty(imageID, inputfieldID.text, "���̵�")) return;
        if (IsFieldDataEmpty(imagedPW, inputfieldPW.text, "��й�ȣ")) return;

        //�α��� ��ư�� ��Ÿ���� ���ϵ��� ��Ȱ��ȭ
        btnLogin.interactable = false;

        //������ �α����� ��û�ϴ� ���� ȭ�鿡 ��µǴ³���
        //ex) �α��� ���� �ؽ�Ʈ, ��Ϲ��� ������
        StartCoroutine(nameof(LoginProcess));
        //�ڳ����� �α��� �õ�
        ReasponToLogin(inputfieldID.text, inputfieldPW.text);
    }
    /// <summary>
    /// �α��� �õ� �� �����κ��� ���޹��� �޽��� ������� ó��
    /// </summary>
    private void ReasponToLogin(string ID,string PW)
    {
        Backend.BMember.CustomLogin(ID, PW, callback =>
        {
            StopCoroutine(nameof(LoginProcess));

            if (callback.IsSuccess())
            {
                SetMessage($"{inputfieldID.text}�� ȯ���մϴ�.");

                //�κ� �� �̵�
                Utils.LoadScene(SceneNames.Lobby);
            }
            //�α��ν���
            else
            {
                //������������ �ٽ� ��ư ��ȣ�ۿ� Ȱ��ȭ
                btnLogin.interactable = true;

                string message = string.Empty;

                Debug.Log(callback.GetMessage());
                switch(int.Parse(callback.GetStatusCode()) )
                {
                    case 401: // ���������ʴ� ���̵�, �߸��� ��й�ȣ
                        message = callback.GetMessage().Contains("customId") ? "�������� �ʴ� ���̵��Դϴ�." : "�߸��� ��й�ȣ�Դϴ�.";
                        break;
                    case 403: // ���ܴ��� ���̵�
                        message = callback.GetMessage().Contains("user") ? "���ܴ��� ���������Դϴ�." : "���ܴ��� ����̽��Դϴ�.";
                        break;
                    case 410:
                        message = "Ż�� �������� �����Դϴ�.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }

                //401�ڵ忡�� ��й�ȣ Ʋ������
                if(message.Contains("��й�ȣ"))
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
            SetMessage($"�α��� ���Դϴ�...{time:F1}");

            yield return null;
        }
    }
}
