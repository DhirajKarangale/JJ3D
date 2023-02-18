using System;
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
    internal EquipmentManager equipmentManager;

    internal event Action OnDetailsChanged;

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

    private float GetDefence()
    {
        float defence = 0;
        if (equipmentManager.slotHelmet.itemData) defence += equipmentManager.slotHelmet.itemData.modifier;
        if (equipmentManager.slotVest.itemData) defence += equipmentManager.slotVest.itemData.modifier;
        return defence;
    }

    internal void PlayAudio(AudioClip clip, float volume = 1)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(clip);
    }

    internal float DamageReducer()
    {
        float damage = 0;
        if (equipmentManager.slotHelmet.itemData)
        {
            if (equipmentManager.slotHelmet.itemData.currHealth <= 0)
            {
                equipmentManager.DestroyHelmet();
                return damage;
            }
            damage += equipmentManager.slotHelmet.itemData.modifier;
            equipmentManager.slotHelmet.itemData.currHealth -= equipmentManager.slotHelmet.itemData.modifier;
            equipmentManager.slotHelmet.UpdateSlider();
        }

        if (equipmentManager.slotVest.itemData)
        {
            if (equipmentManager.slotVest.itemData.currHealth <= 0)
            {
                equipmentManager.DestroyVest();
                return damage;
            }
            damage += equipmentManager.slotVest.itemData.modifier;
            equipmentManager.slotVest.itemData.currHealth -= equipmentManager.slotVest.itemData.modifier;
            equipmentManager.slotVest.UpdateSlider();
        }

        return damage;
    }

    internal DetailsData GetDetails()
    {
        DetailsData detailsData = new DetailsData();

        detailsData.mxHealth = playerHealth.mxHealth;
        detailsData.health = playerHealth.currHealth;
        detailsData.speed = playerMovement.movement.forwardSpeed;
        detailsData.hunger = playerHealth.currHunger;
        detailsData.attack = equipmentManager.slotWeapon.itemData ? equipmentManager.slotWeapon.itemData.modifier : 0;
        detailsData.defence = GetDefence();

        return detailsData;
    }

    internal void ChangeShoesHealth()
    {
        if (equipmentManager.slotShoes.itemData)
        {
            if (equipmentManager.slotShoes.itemData.currHealth <= 0)
            {
                equipmentManager.DestroyShoes();
                return;
            }

            equipmentManager.slotShoes.itemData.currHealth--;
            equipmentManager.slotShoes.UpdateSlider();
        }
    }

    internal void Eat(ItemData itemData)
    {
        equipmentManager.Eat(itemData);
    }

    internal void EquipDefence(ItemData itemData)
    {
        equipmentManager.EquipDefence(itemData);
        ChangeDetails();
    }

    internal void EquipWeapon(ItemData itemData)
    {
        equipmentManager.SetWeapon(itemData);
        ChangeDetails();
    }

    internal void ChangeDetails()
    {
        OnDetailsChanged?.Invoke();
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
