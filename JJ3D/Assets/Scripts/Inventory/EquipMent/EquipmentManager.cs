using UnityEngine;
using System.Collections;

public class EquipmentManager : MonoBehaviour
{
    [Header("Equipment Slots")]
    [SerializeField] EquipmentSlot slotHelmet;
    [SerializeField] EquipmentSlot slotVest;
    [SerializeField] internal EquipmentSlot slotWeapon;
    [SerializeField] EquipmentSlot slotShoes;

    [Header("Weapons")]
    [SerializeField] internal GameObject objSwardNormal;
    [SerializeField] internal GameObject objSwardIce;
    [SerializeField] internal GameObject objSwardLightning;
    [SerializeField] internal GameObject objBow;
    [SerializeField] internal GameObject objBowThree;
    [SerializeField] internal GameObject objBowFire;

    [Header("Food")]
    [SerializeField] GameObject objMeat;
    [SerializeField] GameObject objApple;
    [SerializeField] GameObject objMango;

    [Header("Effect")]
    [SerializeField] AudioSource audioSourcePlayer;
    [SerializeField] AudioClip clipEat;
    [SerializeField] ParticleSystem psEat;

    private GameManager gameManager;
    private PickUpSystem pickUpSystem;
    private PlayerAttack playerAttack;
    private PlayerHealth playerHealth;

    public bool isSwardActive
    {
        get
        {
            return objSwardNormal.activeInHierarchy || objSwardIce.activeInHierarchy || objSwardLightning.activeInHierarchy;
        }
    }

    public bool isBowActive
    {
        get
        {
            return objBow.activeInHierarchy || objBowFire.activeInHierarchy || objBowThree.activeInHierarchy;
        }
    }


    private void Start()
    {
        gameManager = GameManager.instance;
        pickUpSystem = gameManager.pickUpSystem;
        playerAttack = gameManager.playerAttack;
        playerHealth = gameManager.playerHealth;

        playerAttack.OnAttack += OnPlayerAttack;

        slotHelmet.Reset();
        slotVest.Reset();
        slotWeapon.Reset();
        slotShoes.Reset();

        slotWeapon.OnRemove += OnWeaponRemove;
    }

    private IEnumerator IEEat(float modifier, string name)
    {
        playerAttack.animator.SetBool("isEating", true);

        audioSourcePlayer.volume = 0.4f;
        audioSourcePlayer.PlayOneShot(clipEat);
        objMeat.SetActive(name.Contains("Meat"));
        objApple.SetActive(name.Contains("Apple"));
        objMango.SetActive(name.Contains("Mango"));
        psEat.Play();
        DesableWeapon();

        yield return new WaitForSeconds(3);

        audioSourcePlayer.volume = 1;
        audioSourcePlayer.Stop();
        objMeat.SetActive(false);
        objApple.SetActive(false);
        objMango.SetActive(false);
        psEat.Stop();
        playerHealth.IncreaseHealth(modifier);

        playerAttack.animator.SetBool("isEating", false);

        if (slotWeapon.item) ActiveWeapon(slotWeapon.item.itemData.name);
    }


    private void OnWeaponRemove()
    {
        slotWeapon.item.ThrowItem(gameManager.playerPos.position + (gameManager.playerPos.forward * 3));
        slotWeapon.Reset();
        ActiveWeapon("None");
    }

    private void OnPlayerAttack()
    {
        slotWeapon.item.itemData.currHealth--;
        slotWeapon.UpdateSlider();
        if (slotWeapon.item.itemData.currHealth <= 0)
        {
            gameManager.DestroyEffect(objSwardIce.transform.position);
            slotWeapon.item.DestoryItem();
            ActiveWeapon("None");
            slotWeapon.Reset();
        }
    }


    private void ActiveWeapon(string name)
    {
        objSwardNormal.SetActive(name.Contains("SwardNormal"));
        objSwardIce.SetActive(name.Contains("SwardIce"));
        objSwardLightning.SetActive(name.Contains("SwardLightning"));

        objBow.SetActive(name.Contains("Bow"));
        objBowThree.SetActive(name.Contains("BowThree"));
        objBowFire.SetActive(name.Contains("BowFire"));
    }

    private void DesableWeapon()
    {
        objSwardNormal.SetActive(false);
        objSwardIce.SetActive(false);
        objSwardLightning.SetActive(false);

        objBow.SetActive(false);
        objBowFire.SetActive(false);
        objBowThree.SetActive(false);
    }




    public void Eat(Item item)
    {
        StartCoroutine(IEEat(item.itemData.modifier, item.name));
        Destroy(item.gameObject);
    }

    public void SetDefence(ItemData itemData)
    {

    }

    public void SetWeapon(Item item)
    {
        if (slotWeapon.item) pickUpSystem.PickUp(slotWeapon.item);
        slotWeapon.SetData(item);
        ActiveWeapon(item.itemData.name);
    }
}
