using UnityEngine;

public class FireArrow : MonoBehaviour
{
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
        GameManager.instance.effects.BombExplosionEffect(transform.position);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearByObj in colliders)
        {
            NPCHealth npcHealth = nearByObj.GetComponent<NPCHealth>();
            if (npcHealth)
            {
                npcHealth.TakeDamage(damage);
            }

            Rigidbody rigidBody = nearByObj.GetComponent<Rigidbody>();
            if (rigidBody && rigidBody.useGravity)
            {
                rigidBody.AddExplosionForce(force, transform.position, radius);
            }
        }

        this.gameObject.SetActive(false);
    }

    private void DestroyArrow()
    {
        GameManager.instance.effects.DestroyEffect(transform.position);
        this.gameObject.SetActive(false);
    }
}
