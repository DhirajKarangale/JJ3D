using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] float impactForce;
    [SerializeField] IceSward iceSward;
    [HideInInspector] internal float damage;
    private GameManager gameManager;
    private Item itemWeapon;

    private void Start()
    {
        gameManager = GameManager.instance;
        itemWeapon = gameManager.equipementManager.slotWeapon.item;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameManager.isGameOver) return;

        damage = (itemWeapon) ? itemWeapon.itemData.modifier : 10;
        Rigidbody rigidBody = collision.gameObject.GetComponent<Rigidbody>();
        if (rigidBody)
        {
            Vector3 dir = collision.transform.position - this.transform.position;
            dir = dir.normalized;
            if (rigidBody.useGravity) rigidBody.AddForce(dir * impactForce, ForceMode.Impulse);
            if (iceSward) iceSward.Freez(rigidBody);
        }

        NPCHealth npcHealth = collision.gameObject.GetComponent<NPCHealth>();
        if (npcHealth)
        {
            if (npcHealth.health <= 0 && iceSward) iceSward.DeFreez();
            if(!npcHealth.isDestroyBody) gameManager.effects.EnemyBloodEffect(collision.GetContact(0).point);
            else gameManager.effects.DestroyEffect(collision.GetContact(0).point);
            npcHealth.TakeDamage(damage);
        }
    }
}
