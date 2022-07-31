using UnityEngine;

public class FireArrow : MonoBehaviour
{
    // [SerializeField] PlayerWeapon playerWeapon;
    // public Item item;
    [HideInInspector] public float damage;
    [SerializeField] float force = 700;
    [SerializeField] float radius = 5;

    private void Start()
    {
        Invoke("DestroyArrow", 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.instance.isGameOver) return;
        GameManager.instance.BombExplosionEffect(transform.position);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearByObj in colliders)
        {
            NPCHealth npcHealth = nearByObj.GetComponent<NPCHealth>();
            if (npcHealth)
            {
                npcHealth.TakeDamage(damage);

                // if (item.currHealth > 0)
                // {
                //     item.currHealth -= item.armorModifire;
                // }
                // else
                // {
                //     item.DestroyItem();
                //     return;
                // }

                // npcHealth.rigidBody.AddExplosionForce(force, transform.position, radius);
            }

            Rigidbody rigidBody = nearByObj.GetComponent<Rigidbody>();
            if (rigidBody)
            {
                rigidBody.AddExplosionForce(force, transform.position, radius);
            }
        }

        Destroy(this.gameObject);
    }

    private void DestroyArrow()
    {
        GameManager.instance.DestroyEffect(transform.position);
        Destroy(this.gameObject);
    }
}
