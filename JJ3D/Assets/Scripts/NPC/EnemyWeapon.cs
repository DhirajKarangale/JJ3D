using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    // [SerializeField] float impactForce;
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
            // Vector3 dir = collision.transform.position - this.transform.position;
            // dir.Normalize();
            // player.rigidBody.AddForce(dir * impactForce);
            gameManager.PlayerBloodEffect(collision.GetContact(0).point);
            player.TakeDamage(damage);
        }
    }
}
