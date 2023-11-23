using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private GameObject projecttilePrefab; // 공격프리팹
    [SerializeField]
    private float attackRate = 0.1f; //공격속도

    private float lastAttackTime = 0;

    public void WeaponAction()
    {
        if(Time.time - lastAttackTime > attackRate)
        {
            //오브젝트생성
            Instantiate(projecttilePrefab, transform.position,Quaternion.identity);
            //공격 주기가 되어야 공격 할 수 있도록 공격시간 저장
            lastAttackTime = Time.time;

        }
    }
}
