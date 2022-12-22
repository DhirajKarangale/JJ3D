using UnityEngine;

public class Projectile : MonoBehaviour
{
    private enum DestroyEffect { Fireball, Object };
    [SerializeField] bool isCollisionDestroy;
    [SerializeField] DestroyEffect destroyEffect;

    private void Start()
    {
        Invoke("DestroyProjectile", 3);
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
                    DestroyProjectile();
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
                DestroyProjectile();
            }
        }
        else
        {
            GameManager.instance.effects.HitSound(transform.position);
        }
    }

    private void DestroyProjectile()
    {
        if (destroyEffect == DestroyEffect.Object)
        {
            GameManager.instance.effects.DestroyEffect(transform.position);
        }
        else
        {
            GameManager.instance.effects.FireballDestroyEffect(transform.position);
        }
        this.gameObject.SetActive(false);
    }
}
