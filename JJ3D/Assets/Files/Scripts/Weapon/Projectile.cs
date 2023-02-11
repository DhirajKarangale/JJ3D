using UnityEngine;

public class Projectile : MonoBehaviour
{
    private enum DestroyEffect { Fireball, Object };
    [SerializeField] bool isCollisionDestroy;
    [SerializeField] DestroyEffect destroyEffect;

    private int collided;

    private void Start()
    {
        collided = 0;
        Invoke("DesableObj", 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isCollisionDestroy)
        {
            NPCHealth npcHealth = collision.gameObject.GetComponent<NPCHealth>();
            if (npcHealth)
            {
                if (npcHealth.isDestroyBody)
                {
                    DestroyProjectile(collision.gameObject);
                    // Debug.Log("Destroy NPC");
                }
                else
                {
                    // Debug.Log("Save NPC");
                    this.gameObject.SetActive(false);
                }
            }
            else
            {
                DestroyProjectile(collision.gameObject);
            }
        }
        else
        {
            GameManager.instance.effects.HitSound(transform.position);
        }
    }

    private void DestroyProjectile(GameObject collision)
    {
        if (destroyEffect == DestroyEffect.Object)
        {
            if (collision.layer == 6 && collided < 2)
            {
                collided++;
                return;
            }
            GameManager.instance.effects.DestroyEffect(transform.position);
            this.gameObject.SetActive(false);
            collided = 0;
        }
        else
        {
            GameManager.instance.effects.FireballDestroyEffect(transform.position);
            this.gameObject.SetActive(false);
        }
    }

    private void DesableObj()
    {
        this.gameObject.SetActive(false);
    }
}
