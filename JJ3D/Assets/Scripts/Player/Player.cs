using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] internal Animator animator;
    [SerializeField] internal Rigidbody rigidBody;
    [SerializeField] internal AudioSource audioSource;
    [SerializeField] internal CapsuleCollider capsuleCollider;

    [Header("Refrences")]
    [SerializeField] internal Camera cam;
    [SerializeField] internal PlayerMovement playerMovement;
    [SerializeField] internal PlayerAttack playerAttack;
    [SerializeField] internal PlayerHealth playerHealth;
    private EquipmentManager equipmentManager;

    private float mxHealth;
    private float currhealth;
    private float defence;
    private float attack;

    private float orgForwardSpeed;
    private float orgBackSpeed;
    private float orgSideSpeed;


    private void Start()
    {
        equipmentManager = GameManager.instance.equipementManager;

        orgForwardSpeed = playerMovement.movement.forwardSpeed;
        orgBackSpeed = playerMovement.movement.backwardSpeed;
        orgSideSpeed = playerMovement.movement.sideSpeed;
    }


    internal void PlayAudio(AudioClip clip, float volume = 1)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(clip);
    }

    internal float DamageReducer()
    {
        float damage = 0;
        if (equipmentManager.slotHelmet.item)
        {
            if (equipmentManager.slotHelmet.item.itemData.currHealth <= 0)
            {
                equipmentManager.DestroyHelmet();
                return damage;
            }
            damage += equipmentManager.slotHelmet.item.itemData.modifier;
            equipmentManager.slotHelmet.item.itemData.currHealth -= equipmentManager.slotHelmet.item.itemData.modifier;
            equipmentManager.slotHelmet.UpdateSlider();
        }

        if (equipmentManager.slotVest.item)
        {
            if (equipmentManager.slotVest.item.itemData.currHealth <= 0)
            {
                equipmentManager.DestroyVest();
                return damage;
            }
            damage += equipmentManager.slotVest.item.itemData.modifier;
            equipmentManager.slotVest.item.itemData.currHealth -= equipmentManager.slotVest.item.itemData.modifier;
            equipmentManager.slotVest.UpdateSlider();
        }

        return damage;
    }

    internal void ChangeShoesHealth()
    {
        if (equipmentManager.slotShoes.item)
        {
            if (equipmentManager.slotShoes.item.itemData.currHealth <= 0)
            {
                equipmentManager.DestroyShoes();
                return;
            }

            equipmentManager.slotShoes.item.itemData.currHealth--;
            equipmentManager.slotShoes.UpdateSlider();
        }
    }

    internal void Eat(Item item)
    {
        equipmentManager.Eat(item);
    }

    internal void EquipDefence(Item item)
    {
        equipmentManager.EquipDefence(item);
    }

    internal void EquipWeapon(Item item)
    {
        equipmentManager.SetWeapon(item);
    }

    internal void ChangeSpeed(float modifier)
    {
        playerMovement.movement.forwardSpeed = orgForwardSpeed * modifier;
        playerMovement.movement.backwardSpeed = orgBackSpeed * modifier;
        playerMovement.movement.sideSpeed = orgSideSpeed * modifier;
    }

    internal void ResetSpeed()
    {
        playerMovement.movement.forwardSpeed = orgForwardSpeed;
        playerMovement.movement.backwardSpeed = orgBackSpeed;
        playerMovement.movement.sideSpeed = orgSideSpeed;
    }

    internal void GameOver()
    {
        capsuleCollider.height = 0.2f;
        capsuleCollider.material = null;
        equipmentManager.DesableWeapon();
        playerAttack.enabled = false;
        playerHealth.enabled = false;
        playerMovement.enabled = false;
    }
}
