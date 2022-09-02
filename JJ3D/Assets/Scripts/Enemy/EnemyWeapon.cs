using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] float impactForce;
    [SerializeField] float damage;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameManager.isGameOver) return;

        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
        if (player)
        {
            Vector3 direction = player.transform.position - transform.position;
            player.rigidBody.AddForceAtPosition(direction.normalized, transform.position);
            gameManager.PlayerBloodEffect(collision.GetContact(0).point);
            player.TakeDamage(damage);
        }
    }
}
