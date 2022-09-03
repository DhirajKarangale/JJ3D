using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] float impactForce;
    [SerializeField] IceSward iceSward;
    [HideInInspector] internal float damage;
    private GameManager gameManager;
    private EquipmentSlot equipmentSlot;

    private void Start()
    {
        gameManager = GameManager.instance;
        equipmentSlot = gameManager.weaponSlot;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameManager.isGameOver) return;

        damage = (equipmentSlot.equipedItem) ? equipmentSlot.equipedItem.damageModifire : 10;
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
            if(!npcHealth.isDestroyBody) gameManager.EnemyBloodEffect(collision.GetContact(0).point);
            else gameManager.DestroyEffect(collision.GetContact(0).point);
            npcHealth.TakeDamage(damage);
        }
    }
}
