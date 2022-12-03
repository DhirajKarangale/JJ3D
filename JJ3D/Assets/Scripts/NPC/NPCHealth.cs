using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    public bool isDestroyBody;
    [SerializeField] float mxHealth;
    [SerializeField] NPC npc;
    [SerializeField] HealthBar healthBar;
    [SerializeField] GameObject item;
    [HideInInspector] public float health;

    private void Start()
    {
        health = mxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.SetBar(health / mxHealth);
        if (health <= 0) Dye();
        else npc.Hurt();
    }

    private void Dye()
    {
        if (IsInvoking("DestroyBody")) return;
        npc.Dye();
        Invoke("DestroyBody", 5);
    }

    private void DestroyBody()
    {
        GameManager.instance.effects.DestroyBodyEffect(transform.position);
        SpwanItem();
    }

    private void SpwanItem()
    {
        int count = item.name.Contains("Coin") ? Random.Range(7, 15) : 1;
        for (int i = 0; i < count; i++)
        {
            Vector3 spwanPos = transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(0.5f, 3f), Random.Range(-3f, 3f));
            Instantiate(item, spwanPos, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
}