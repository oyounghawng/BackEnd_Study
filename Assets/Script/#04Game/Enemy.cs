using System.Net.NetworkInformation;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int scorePoint = 100; //적 처치시 점수
    [SerializeField]
    private GameObject explosionPrefab; //폭발 프리팹
    private GameController gameController;

    public void Setup(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void OnDie()
    {
        //이펙트 생성
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        //처치시 점수증가
        gameController.Score += scorePoint;
        //적 케릭터 삭제
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnDie();
            gameController.GameOver();
        }
    }
}
