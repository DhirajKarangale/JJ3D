using UnityEngine;

public class PlayerWeapon : MonoBehaviour
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

        NPCHealth animal = collision.gameObject.GetComponent<NPCHealth>();
        if (animal)
        {
            gameManager.EnemyBloodEffect(collision.GetContact(0).point);
            animal.TakeDamage(damage);
        }
    }
}
