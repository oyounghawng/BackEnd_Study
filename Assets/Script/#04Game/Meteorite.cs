using UnityEngine;

public class Meteorite : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab; //气惯 橇府普
    private GameController gameController;

    public void Setup(GameController gameController)
    {
        this.gameController = gameController;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //捞棋飘 积己
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            //款籍昏力
            Destroy(gameObject);
            gameController.GameOver();
        }
    }
}
