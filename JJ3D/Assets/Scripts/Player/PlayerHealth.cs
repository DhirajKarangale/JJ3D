using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Refrence")]
    [SerializeField] Player player;
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

    private void Start()
    {
        currHunger = mxHunger;
        currHealth = mxHealth;

        InvokeRepeating("HungerReducer", 60, 30);
        InvokeRepeating("AutoHealthIncrease", 60, 15);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
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
        currHunger -= 4;
        UpdateSliders();
        if (currHunger <= 0)
        {
            player.PlayAudio(clipHunger);
            TakeDamage(4);
        }
    }

    private void UpdateSliders()
    {
        sliderHunger.value = currHunger / mxHunger;
        sliderHealth.value = currHealth / mxHealth;
        hungerFillImage.color = hungerGradient.Evaluate(sliderHunger.value);
        healthFillImage.color = healthGradient.Evaluate(sliderHealth.value);
    }

    private void PlayerDye()
    {
        player.animator.Play("Dye");
        player.rigidBody.mass = 2000;
        GameManager.instance.GameOver();
    }

    public void IncreaseHealth(float modifier)
    {
        // currHealth = Mathf.Clamp(0, mxHealth, currHealth + modifier); // Problem in this code
        currHealth += modifier; // This is temp Substitue Replace it with valid code
        currHunger = Mathf.Clamp(0, mxHunger, currHunger + (modifier * 1.5f));
        UpdateSliders();
    }

    public void TakeDamage(float damage)
    {
        damage -= player.DamageReducer();
        currHealth -= damage;
        UpdateSliders();

        if (currHealth <= 0)
        {
            PlayerDye();
        }
    }
}
