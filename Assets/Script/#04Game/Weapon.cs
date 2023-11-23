using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private GameObject projecttilePrefab; // ����������
    [SerializeField]
    private float attackRate = 0.1f; //���ݼӵ�

    private float lastAttackTime = 0;

    public void WeaponAction()
    {
        if(Time.time - lastAttackTime > attackRate)
        {
            //������Ʈ����
            Instantiate(projecttilePrefab, transform.position,Quaternion.identity);
            //���� �ֱⰡ �Ǿ�� ���� �� �� �ֵ��� ���ݽð� ����
            lastAttackTime = Time.time;

        }
    }
}
