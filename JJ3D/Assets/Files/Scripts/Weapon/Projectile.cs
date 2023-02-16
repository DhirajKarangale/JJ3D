using UnityEngine;

public class Projectile : MonoBehaviour
{
    private enum DestroyEffect { Fireball, Object };
    [SerializeField] bool isCollisionDestroy;
    [SerializeField] DestroyEffect destroyEffect;
    private GameManager gameManager;
    [SerializeField] float speed;

    private int collided;

    private void Start()
    {
        gameManager = GameManager.instance;
        collided = 0;
        Invoke("DesableObj", 5);
    }

    private void Update()
    {
        if (destroyEffect == DestroyEffect.Fireball && collided > 0)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, gameManager.playerPos.position, Time.deltaTime * speed);
        }
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
            if (collision.layer == 6 && collided < 1)
            {
                collided++;
                return;
            }
            GameManager.instance.effects.FireballDestroyEffect(transform.position);
            this.gameObject.SetActive(false);
            collided = 0;
        }
    }

    private void DesableObj()
    {
        this.gameObject.SetActive(false);
    }
}
