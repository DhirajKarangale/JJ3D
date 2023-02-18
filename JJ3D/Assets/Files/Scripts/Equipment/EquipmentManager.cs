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
    [SerializeField] Inventory inventory;

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

        if (gameManager.isGameOver) yield break;
        yield return new WaitForSeconds(3);
        if (gameManager.isGameOver) yield break;

        player.playerHealth.IncreaseHealth(modifier);

        StopEating();
    }


    private void OnRemoveWeapon()
    {
        inventory.ThrowItem(slotWeapon.itemData);
        slotWeapon.Reset();
        ActiveWeapon("None");
        player.ChangeDetails();
    }

    private void OnRemoveHelmet()
    {
        inventory.ThrowItem(slotHelmet.itemData);
        slotHelmet.Reset();
        objHelmet.SetActive(false);
        player.ChangeDetails();
    }

    private void OnRemoveVest()
    {
        inventory.ThrowItem(slotVest.itemData);
        slotVest.Reset();
        objVest.SetActive(false);
        player.ChangeDetails();
    }

    private void OnRemoveShoes()
    {
        inventory.ThrowItem(slotShoes.itemData);
        slotShoes.Reset();
        player.ResetSpeed();
        objShoesLeft.SetActive(false);
        objShoesRight.SetActive(false);
        player.ChangeDetails();
    }

    private void OnPlayerAttack()
    {
        slotWeapon.itemData.currHealth--;
        slotWeapon.UpdateSlider();
        if (slotWeapon.itemData.currHealth <= 0)
        {
            gameManager.effects.DestroyEffect(objSwardIce.transform.position);
            // slotWeapon.item.DestoryItem(); // Desable Obj
            ActiveWeapon("None");
            slotWeapon.Reset();
        }
    }


    private void ActiveWeapon(string name)
    {
        objSwardNormal.SetActive(name.Contains("SwardNormal"));
        objSwardIce.SetActive(name.Contains("SwardIce"));
        objSwardLightning.SetActive(name.Contains("SwardLightning"));

        objBow.SetActive(name.Contains("BowNormal"));
        objBowThree.SetActive(name.Contains("BowThree"));
        objBowFire.SetActive(name.Contains("BowFire"));
    }



    internal void StopEating()
    {
        audioSourcePlayer.volume = 1;
        audioSourcePlayer.Stop();
        objMeat.SetActive(false);
        objApple.SetActive(false);
        objMango.SetActive(false);
        psEat.Stop();

        player.animator.SetBool("isEating", false);

        if (slotWeapon.itemData) ActiveWeapon(slotWeapon.itemData.name);
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
        // slotHelmet.item.DestoryItem();  // Desable Obj
        slotHelmet.Reset();
        gameManager.effects.DestroyEffect(objHelmet.transform.position);
        objHelmet.SetActive(false);
        player.ChangeDetails();
    }

    internal void DestroyVest()
    {
        // slotVest.item.DestoryItem(); // Desable Obj
        slotVest.Reset();
        gameManager.effects.DestroyEffect(objVest.transform.position);
        objVest.SetActive(false);
        player.ChangeDetails();
    }

    internal void DestroyShoes()
    {
        // slotShoes.item.DestoryItem(); // Desable Obj
        slotShoes.Reset();
        player.ResetSpeed();
        gameManager.effects.DestroyEffect(objShoesRight.transform.position);
        objShoesLeft.SetActive(false);
        objShoesRight.SetActive(false);
        player.ChangeDetails();
    }

    internal void Eat(ItemData itemData)
    {
        StartCoroutine(IEEat(itemData.modifier, itemData.name));
        // Destroy(item.gameObject); // Desable Obj
    }

    internal void EquipDefence(ItemData itemData)
    {
        if (itemData.name.Contains("Helmet"))
        {
            slotHelmet.SetData(itemData);
            objHelmet.SetActive(true);
        }
        else if (itemData.name.Contains("Vest"))
        {
            slotVest.SetData(itemData);
            objVest.SetActive(true);
        }
        else if (itemData.name.Contains("Shoes"))
        {
            slotShoes.SetData(itemData);
            objShoesLeft.SetActive(true);
            objShoesRight.SetActive(true);
            player.ChangeSpeed(slotShoes.itemData.modifier);
        }
    }

    internal void SetWeapon(ItemData itemData)
    {
        if (slotWeapon.itemData) pickUpSystem.PickUp(null, slotWeapon.itemData);
        slotWeapon.SetData(itemData);
        ActiveWeapon(itemData.name);
    }
}
