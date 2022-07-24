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

        Rigidbody rigidBody = collision.gameObject.GetComponent<Rigidbody>();
        if (rigidBody)
        {
            Vector3 dir = collision.transform.position - this.transform.position;
            dir = dir.normalized;
            rigidBody.AddForce(dir * impactForce, ForceMode.Impulse);
        }

        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
        if (player)
        {
            gameManager.PlayerBloodEffect(collision.GetContact(0).point);
            player.TakeDamage(damage);
        }
    }
}
