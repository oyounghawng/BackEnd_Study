using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BackEnd;
using UnityEngine.UI;

public class FindPW : LoginBase
{
    [SerializeField]
    private Image imageID;
    [SerializeField]
    private TMP_InputField inputFieldID;
    [SerializeField]
    private Image imageEmail;
    [SerializeField]
    private TMP_InputField inputFieldEmail;

    [SerializeField]
    private Button btnFindPW;

    public void OnClickFindPW()
    {
        //�Ű������� �Է��� �ʵ�� �ʱ�ȭ
        ResetUI(imageEmail,imageEmail);

        //������� üũ
        if (IsFieldDataEmpty(imageID, inputFieldID.text, "���̵�")) return;
        if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "���� �ּ�")) return;

        if (!inputFieldEmail.text.Contains("@"))
        {
            GuideForIncorrectlyEnterData(imageEmail, "���� ������ �߸��Ǿ����ϴ�.(ex. address@xx.xx) ");
            return;
        }


        //"��й�ȣ ã��" ��ư ��Ȱ��ȭ
        btnFindPW.interactable = false;
        SetMessage("���� �߼����Դϴ�.");

        //�ڳ� ���� ����
        FindCustomPW();
    }

    private void FindCustomPW()
    {
        Backend.BMember.ResetPassword(inputFieldID.text,inputFieldEmail.text, callback =>
        {
            //��ư Ȱ��ȭ
            btnFindPW.interactable = true;

            //���� �߼� ����
            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldEmail.text} �ּҷ� ������ �߼��Ͽ� ���ϴ�.");
            }
            //���� �߼� ����
            else
            {
                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 404: // ��ϵ��� ���� �̸���
                        message = "�ش� �̸����� ����ϴ� ����ڰ� �����ϴ�.";
                        break;
                    case 429: // 24�ð� �̳� 5ȸ �̻� �õ�
                        message = "24�ð� �̳��� 5ȸ �̻� ���̵�/��й�ȣ ã�⸦ �õ��߽��ϴ�.";
                        break;
                    default:
                        //statusCod : 400 => ������Ʈ �� Ư������ �߰������� ���Ϲ߼ۿ���
                        message = callback.GetMessage();
                        break;

                }

                if (message.Contains("�̸���"))
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
