using TMPro;
using UnityEngine;

public class Details : MonoBehaviour
{
    private Player player;

    [SerializeField] TMP_Text txtMxHealth;
    [SerializeField] TMP_Text txtHealth;
    [SerializeField] TMP_Text txtSpeed;
    [SerializeField] TMP_Text txtHunger;
    [SerializeField] TMP_Text txtAttack;
    [SerializeField] TMP_Text txtDefence;

    private void Start()
    {
        // GameManager.instance.player.OnDetailsChanged += OnDetailsChanged;
        SetData(GameManager.instance.player.GetDetails());
    }

    private void OnDetailsChanged()
    {
        SetData(GameManager.instance.player.GetDetails());
    }

    private void SetData(DetailsData data)
    {
        txtMxHealth.text = $"Max Health : {data.mxHealth}";
        txtHealth.text = $"Health : {data.health}";
        txtSpeed.text = $"Speed : {data.speed}";
        txtHunger.text = $"Hunger : {data.hunger}";
        txtAttack.text = $"Attack : {data.attack}";
        txtDefence.text = $"Defence : {data.defence}%";
    }
}

public struct DetailsData
{
    public float mxHealth;
    public float health;
    public float speed;
    public float hunger;
    public float attack;
    public float defence;
}