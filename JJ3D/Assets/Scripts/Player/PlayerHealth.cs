using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Refrence")]
    public Rigidbody rigidBody;
    [SerializeField] PlayerAttack playerAttack;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Slider sliderHealth;
    [SerializeField] Slider sliderHunger;

    [Header("Food")]
    [SerializeField] GameObject meat;
    [SerializeField] GameObject apple;
    [SerializeField] GameObject mango;

    [Header("Effect")]
    [SerializeField] AudioClip clipEat;
    [SerializeField] AudioClip clipHunger;
    [SerializeField] ParticleSystem psEat;

    [Header("Health")]
    [SerializeField] float mxHealth;
    private float currHealth;

    [Header("Hunger")]
    [SerializeField] float mxHunger;
    private float currHunger;

    private GameManager gameManager;
    private EquipementManager equipementManager;
    private Item helmet;
    private Item vest;

    private void Start()
    {
        currHunger = mxHunger;
        currHealth = mxHealth;
        gameManager = GameManager.instance;
        equipementManager = gameManager.equipementManager;
        equipementManager.onEquipementChanged += OnEquipmentChanged;

        InvokeRepeating("HungerReducer", 120, 45);
        InvokeRepeating("AutoHealthIncrease", 60, 15);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(15);
        }
    }

    public void TakeDamage(float damage)
    {
        damage = vest ? Armor(vest, damage) : damage;
        damage = helmet ? Armor(helmet, damage) : damage;

        currHealth -= damage;
        UpdateSliders();

        if (currHealth <= 0)
        {
            PlayerDye();
        }
    }

    private void AutoHealthIncrease()
    {
        if (currHealth < (mxHealth / 2) && (currHunger > 10))
        {
            currHealth += 3;
            currHunger -= 6;
        }
    }

    private void HungerReducer()
    {
        currHunger -= 1;
        UpdateSliders();
        if (currHunger <= 0)
        {
            audioSource.PlayOneShot(clipHunger);
            TakeDamage(3);
        }
    }

    private void OnEquipmentChanged(Item newItem, Item oldItem)
    {
        if (oldItem && oldItem.itemType == ItemType.Helmet) helmet = null;
        if (oldItem && oldItem.itemType == ItemType.Vest) vest = null;

        if (newItem && newItem.itemType == ItemType.Helmet) helmet = newItem;
        if (newItem && newItem.itemType == ItemType.Vest) vest = newItem;
    }

    private float Armor(Item item, float damage)
    {
        if (item.currHealth > 0)
        {
            damage -= item.armorModifire;
            item.currHealth -= (item.armorModifire / item.modifierMultiplier);
        }
        else
        {
            item.DestroyItem();
        }

        return damage;
    }

    private void UpdateSliders()
    {
        sliderHunger.value = currHunger / mxHunger;
        sliderHealth.value = currHealth / mxHealth;
    }

    private void PlayerDye()
    {
        playerAttack.animator.Play("Dye");
        rigidBody.mass = 2000;
        gameManager.GameOver();
    }

    public void IncreaseHealth(float increaseHealth, float increaseHunger)
    {
        currHealth = Mathf.Clamp(0, mxHealth, currHealth + increaseHealth);
        currHunger = Mathf.Clamp(0, mxHunger, currHunger + increaseHunger);
        UpdateSliders();
    }

    public void Eat(float increaseHealth, float increaseHunger, int item)
    {
        StartCoroutine(IEEat(increaseHealth, increaseHunger, item));
    }

    private IEnumerator IEEat(float increaseHealth, float increaseHunger, int item)
    {
        playerAttack.animator.SetBool("isEating", true);

        audioSource.volume = 0.4f;
        audioSource.PlayOneShot(clipEat);
        if (item == 1) meat.SetActive(true);
        else if (item == 2) apple.SetActive(true);
        else if (item == 3) mango.SetActive(true);
        psEat.Play();

        playerAttack.DesableWeapon();

        yield return new WaitForSeconds(3);

        audioSource.volume = 1;
        audioSource.Stop();
        meat.SetActive(false);
        apple.SetActive(false);
        mango.SetActive(false);
        psEat.Stop();
        IncreaseHealth(increaseHealth, increaseHunger);

        playerAttack.animator.SetBool("isEating", false);

        playerAttack.EnableWeapon();
    }
}
