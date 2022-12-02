using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Refrence")]
    public Rigidbody rigidBody;
    [SerializeField] PlayerAttack playerAttack;
    [SerializeField] AudioSource audioSource;

    [Header("Effect")]
    [SerializeField] AudioClip clipHunger;

    [Header("Health")]
    [SerializeField] Slider sliderHealth;
    [SerializeField] Image healthFillImage;
    [SerializeField] Gradient healthGradient;
    [SerializeField] float mxHealth;
    private float currHealth;

    [Header("Hunger")]
    [SerializeField] Slider sliderHunger;
    [SerializeField] Image hungerFillImage;
    [SerializeField] Gradient hungerGradient;
    [SerializeField] float mxHunger;
    private float currHunger;

    private GameManager gameManager;

    private void Start()
    {
        currHunger = mxHunger;
        currHealth = mxHealth;
        gameManager = GameManager.instance;

        InvokeRepeating("HungerReducer", 120, 45);
        InvokeRepeating("AutoHealthIncrease", 60, 15);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(float damage)
    {
        // damage = vest ? Armor(vest, damage) : damage;
        // damage = helmet ? Armor(helmet, damage) : damage;

        currHealth -= damage;
        UpdateSliders();

        if (currHealth <= 0)
        {
            PlayerDye();
        }
    }

    private void AutoHealthIncrease()
    {
        if ((currHealth < mxHealth) && (currHunger > 1))
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

    // private void OnEquipmentChanged(ItemOld newItem, ItemOld oldItem)
    // {
    //     if (oldItem && oldItem.itemType == ItemType.Helmet) helmet = null;
    //     if (oldItem && oldItem.itemType == ItemType.Vest) vest = null;

    //     if (newItem && newItem.itemType == ItemType.Helmet) helmet = newItem;
    //     if (newItem && newItem.itemType == ItemType.Vest) vest = newItem;
    // }

    // private float Armor(ItemOld item, float damage)
    // {
    //     if (item.currHealth > 0)
    //     {
    //         damage -= item.modifire;
    //         item.currHealth -= item.modifire;
    //     }
    //     else
    //     {
    //         item.DestroyItem();
    //     }

    //     return damage;
    // }

    private void UpdateSliders()
    {
        sliderHunger.value = currHunger / mxHunger;
        sliderHealth.value = currHealth / mxHealth;
        hungerFillImage.color = hungerGradient.Evaluate(sliderHunger.value);
        healthFillImage.color = healthGradient.Evaluate(sliderHealth.value);
    }

    private void PlayerDye()
    {
        playerAttack.animator.Play("Dye");
        rigidBody.mass = 2000;
        gameManager.GameOver();
    }

    public void IncreaseHealth(float modifier)
    {
        // currHealth = Mathf.Clamp(0, mxHealth, currHealth + modifier); // Problem in this code
        currHealth += modifier; // This is temp Substitue Replace it with valid code
        currHunger = Mathf.Clamp(0, mxHunger, currHunger + (modifier * 1.5f));
        UpdateSliders();
    }
}
