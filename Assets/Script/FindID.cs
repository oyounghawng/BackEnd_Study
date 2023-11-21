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
        //�Ű������� �Է��� �ʵ�� �ʱ�ȭ
        ResetUI(imageEmail);

        //������� üũ
        if (IsFieldDataEmpty(imageEmail, inputFieldEmail.text, "���� �ּ�")) return;

        if(!inputFieldEmail.text.Contains("@"))
        {
            GuideForIncorrectlyEnterData(imageEmail, "���� ������ �߸��Ǿ����ϴ�.(ex. address@xx.xx) ");
            return;
        }

        //"���̵� ã��" ��ư ��Ȱ��ȭ
        btnFindID.interactable = false;
        SetMessage("���� �߼����Դϴ�.");

        //�ڳ� ���� ����
        FindCustomID();
    }

    private void FindCustomID()
    {
        Backend.BMember.FindCustomID(inputFieldEmail.text, callback =>
        {
            //��ư Ȱ��ȭ
            btnFindID.interactable = true;

            //���� �߼� ����
            if(callback.IsSuccess())
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
