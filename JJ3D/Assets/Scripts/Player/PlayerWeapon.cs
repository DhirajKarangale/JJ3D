using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] float impactForce;
    [SerializeField] EquipmentSlot equipmentSlot;
    [SerializeField] IceSward iceSward;
    [SerializeField] bool isFireArrow;
    [HideInInspector] public float damage;
    // [HideInInspector] public Item item; 
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameManager.isGameOver) return;

        if (equipmentSlot) damage = equipmentSlot.equipedItem.damageModifire;

        if (!isFireArrow)
        {
            Rigidbody rigidBody = collision.gameObject.GetComponent<Rigidbody>();
            if (rigidBody)
            {
                Vector3 dir = collision.transform.position - this.transform.position;
                dir = dir.normalized;
                rigidBody.AddForce(dir * impactForce, ForceMode.Impulse);
                if (iceSward) iceSward.Freez(rigidBody);
            }

            NPCHealth npcHealth = collision.gameObject.GetComponent<NPCHealth>();
            if (npcHealth)
            {
                if (npcHealth.health <= 0 && iceSward) iceSward.DeFreez();
                gameManager.EnemyBloodEffect(collision.GetContact(0).point);
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
            }
        }
    }
}
