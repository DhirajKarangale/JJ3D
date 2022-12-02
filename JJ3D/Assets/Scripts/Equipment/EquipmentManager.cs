using UnityEngine;
using System.Collections;

public class EquipmentManager : MonoBehaviour
{
    [Header("Equipment Slots")]
    [SerializeField] internal EquipmentSlot slotVest;
    [SerializeField] internal EquipmentSlot slotHelmet;
    [SerializeField] internal EquipmentSlot slotWeapon;
    [SerializeField] internal EquipmentSlot slotShoes;

    [Header("Equipment")]
    [SerializeField] GameObject objHelmet;
    [SerializeField] GameObject objVest;
    [SerializeField] GameObject objShoesLeft;
    [SerializeField] GameObject objShoesRight;

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

    private Player player;
    private GameManager gameManager;
    private PickUpSystem pickUpSystem;

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
        player = gameManager.player;
        pickUpSystem = gameManager.pickUpSystem;

        player.playerAttack.OnAttack += OnPlayerAttack;

        slotHelmet.Reset();
        slotVest.Reset();
        slotWeapon.Reset();
        slotShoes.Reset();

        slotWeapon.OnRemove += OnRemoveWeapon;
        slotHelmet.OnRemove += OnRemoveHelmet;
        slotVest.OnRemove += OnRemoveVest;
        slotShoes.OnRemove += OnRemoveShoes;
    }


    private IEnumerator IEEat(float modifier, string name)
    {
        while (Time.timeScale < 1)
        {
            yield return null;
        }

        player.animator.SetBool("isEating", true);

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
        player.playerHealth.IncreaseHealth(modifier);

        player.animator.SetBool("isEating", false);

        if (slotWeapon.item) ActiveWeapon(slotWeapon.item.itemData.name);
    }


    private void OnRemoveWeapon()
    {
        slotWeapon.item.ThrowItem(gameManager.playerPos.position + (gameManager.playerPos.forward * 3));
        slotWeapon.Reset();
        ActiveWeapon("None");
    }

    private void OnRemoveHelmet()
    {
        slotHelmet.item.ThrowItem(gameManager.playerPos.position + (gameManager.playerPos.forward * 3));
        slotHelmet.Reset();
        objHelmet.SetActive(false);
    }

    private void OnRemoveVest()
    {
        slotVest.item.ThrowItem(gameManager.playerPos.position + (gameManager.playerPos.forward * 3));
        slotVest.Reset();
        objVest.SetActive(false);
    }

    private void OnRemoveShoes()
    {
        slotShoes.item.ThrowItem(gameManager.playerPos.position + (gameManager.playerPos.forward * 3));
        slotShoes.Reset();
        player.ResetSpeed();
        objShoesLeft.SetActive(false);
        objShoesRight.SetActive(false);
    }

    private void OnPlayerAttack()
    {
        slotWeapon.item.itemData.currHealth--;
        slotWeapon.UpdateSlider();
        if (slotWeapon.item.itemData.currHealth <= 0)
        {
            gameManager.effects.DestroyEffect(objSwardIce.transform.position);
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



    internal void DesableWeapon()
    {
        objSwardNormal.SetActive(false);
        objSwardIce.SetActive(false);
        objSwardLightning.SetActive(false);

        objBow.SetActive(false);
        objBowFire.SetActive(false);
        objBowThree.SetActive(false);
    }

    internal void DestroyHelmet()
    {
        slotHelmet.item.DestoryItem();
        slotHelmet.Reset();
        gameManager.effects.DestroyEffect(objHelmet.transform.position);
        objHelmet.SetActive(false);
    }

    internal void DestroyVest()
    {
        slotVest.item.DestoryItem();
        slotVest.Reset();
        gameManager.effects.DestroyEffect(objVest.transform.position);
        objVest.SetActive(false);
    }

    internal void DestroyShoes()
    {
        slotShoes.item.DestoryItem();
        slotShoes.Reset();
        player.ResetSpeed();
        gameManager.effects.DestroyEffect(objShoesRight.transform.position);
        objShoesLeft.SetActive(false);
        objShoesRight.SetActive(false);
    }

    internal void Eat(Item item)
    {
        StartCoroutine(IEEat(item.itemData.modifier, item.name));
        Destroy(item.gameObject);
    }

    internal void EquipDefence(Item item)
    {
        if (item.itemData.name.Contains("Helmet"))
        {
            slotHelmet.SetData(item);
            objHelmet.SetActive(true);
        }
        else if (item.itemData.name.Contains("Vest"))
        {
            slotVest.SetData(item);
            objVest.SetActive(true);
        }
        else if (item.itemData.name.Contains("Shoes"))
        {
            slotShoes.SetData(item);
            objShoesLeft.SetActive(true);
            objShoesRight.SetActive(true);
            player.ChangeSpeed(slotShoes.item.itemData.modifier);
        }
    }

    internal void SetWeapon(Item item)
    {
        if (slotWeapon.item) pickUpSystem.PickUp(slotWeapon.item);
        slotWeapon.SetData(item);
        ActiveWeapon(item.itemData.name);
    }
}
