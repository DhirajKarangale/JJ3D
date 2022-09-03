using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    public bool isDestroyBody;
    [SerializeField] float mxHealth;
    [SerializeField] NPC npc;
    [SerializeField] HealthBar healthBar;
    [SerializeField] GameObject[] items;
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
        // if (isDestroyBody)
        // {
        //     GameManager.instance.DestroyEffect(transform.position);
        //     SpwanItem();
        //     return;
        // }

        if (IsInvoking("DestroyBody")) return;
        npc.Dye();
        Invoke("DestroyBody", 5);
    }

    private void DestroyBody()
    {
        GameManager.instance.DestroyBodyEffect(transform.position);
        SpwanItem();
    }

    private void SpwanItem()
    {
        foreach (GameObject item in items)
        {
            Vector3 spwanPos = transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(0.5f, 3f), Random.Range(-2f, 2f));
            Instantiate(item, spwanPos, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
}
