using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] float impactForce;
    [SerializeField] IceSward iceSward;
    [HideInInspector] internal float damage;
    private GameManager gameManager;
    private ItemData itemWeapon;

    private void Start()
    {
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

        NPCHealth npcHealth = collision.gameObject.GetComponent<NPCHealth>();
        if (npcHealth)
        {
            if (iceSward) iceSward.Freez(rigidBody);
            if (npcHealth.health <= 0 && iceSward) iceSward.DeFreez();
            if (!npcHealth.npc.isDye)
            {
                if (!npcHealth.isDestroyBody) gameManager.effects.EnemyBloodEffect(collision.GetContact(0).point);
                else gameManager.effects.RockEnemyEffect(collision.GetContact(0).point);
            }
            npcHealth.TakeDamage(damage);
        }
    }
}
