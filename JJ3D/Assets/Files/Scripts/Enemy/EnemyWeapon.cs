using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] bool isProjectile;
    [SerializeField] bool isFireball;
    [SerializeField] float throwForce = 200;
    [SerializeField] float damage;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    private void OnEnable()
    {
        CancelInvoke();
        if (isProjectile) Invoke(nameof(Disable), 4);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameManager.isGameOver) return;

        Player player = collision.gameObject.GetComponent<Player>();
        if (player)
        {
            Vector3 direction = player.transform.position - transform.position;
            player.rigidBody.AddForce(direction.normalized * throwForce);
            gameManager.effects.PlayerBloodEffect(collision.GetContact(0).point);
            player.playerHealth.TakeDamage(damage);
        }
        if (isProjectile)
        {
            if (isFireball) gameManager.effects.FireballDestroyEffect(collision.GetContact(0).point);
            else gameManager.effects.DestroyEffect(collision.GetContact(0).point);
            this.gameObject.SetActive(false);
        }
    }

    private void Disable()
    {
        this.gameObject.SetActive(false);
    }
}
