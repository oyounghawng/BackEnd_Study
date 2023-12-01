using System.Net.NetworkInformation;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int scorePoint = 100; //�� óġ�� ����
    [SerializeField]
    private GameObject explosionPrefab; //���� ������
    private GameController gameController;

    public void Setup(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void OnDie()
    {
        //����Ʈ ����
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        //óġ�� ��������
        gameController.Score += scorePoint;
        //�� �ɸ��� ����
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
