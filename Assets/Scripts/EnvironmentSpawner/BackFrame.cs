using UnityEngine;

public class BackFrame : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            enemy.gameObject.SetActive(false);
        }

        if (collision.TryGetComponent(out Rocket rocket) || collision.TryGetComponent(out EnemyLaserShot enemyLaserShot))
        {
            Destroy(collision.gameObject);
        }
    }
}