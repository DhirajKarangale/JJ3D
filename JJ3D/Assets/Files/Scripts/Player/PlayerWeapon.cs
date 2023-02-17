using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] bool isProjectile;
    [SerializeField] float impactForce;
    [SerializeField] IceSward iceSward;
    [HideInInspector] internal float damage;
    private GameManager gameManager;
    private ItemData itemWeapon;
    private int collided;

    private void Start()
    {
        collided = 0;
        gameManager = GameManager.instance;
        itemWeapon = gameManager.equipementManager.slotWeapon.itemData;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameManager.isGameOver) return;

        damage = (itemWeapon) ? itemWeapon.modifier : 10;
        Rigidbody rigidBody = collision.gameObject.GetComponent<Rigidbody>();
        if (rigidBody)
        {
            Vector3 dir = collision.transform.position - this.transform.position;
            dir = dir.normalized;
            if (rigidBody.useGravity) rigidBody.AddForce(dir * impactForce, ForceMode.Impulse);
        }


        if (collision.gameObject.layer == 8)
        {
            if (collision.gameObject.CompareTag("DestFX"))
            {
                gameManager.effects.RockEnemyEffect(collision.GetContact(0).point);
            }
            else
            {
                gameManager.effects.EnemyBloodEffect(collision.GetContact(0).point);
            }

            NPCHealth npcHealth = collision.gameObject.GetComponent<NPCHealth>();
            if (npcHealth)
            {
                if (iceSward) iceSward.Freez(rigidBody);
                if (npcHealth.health <= 0 && iceSward) iceSward.DeFreez();
                npcHealth.TakeDamage(damage);
                if (isProjectile) this.gameObject.SetActive(false);
            }
        }
        else if (isProjectile)
        {
            if (collision.gameObject.layer == 6 && collided < 2)
            {
                collided++;
                return;
            }
            gameManager.effects.DestroyEffect(transform.localPosition);
            collided = 0;
            this.gameObject.SetActive(false);
        }
    }
}
