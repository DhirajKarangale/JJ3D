using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    public bool isDestroyBody;
    [SerializeField] float mxHealth;
    [SerializeField] internal NPC npc;
    [SerializeField] HealthBar healthBar;
    [SerializeField] string itemTag;
    [SerializeField] int itemCount;
    [HideInInspector] public float health;

    private void Start()
    {
        health = mxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (npc.isDye) return;

        health -= damage;
        healthBar.SetBar(health / mxHealth);
        if (health <= 0) Dye();
        else npc.Hurt();
    }

    private void Dye()
    {
        CamController.instance.Shake(0.9f);
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
        int count = itemCount == -1 ? Random.Range(7, 15) : itemCount;
        ObjectPooler objectPooler = ObjectPooler.instance;
        for (int i = 0; i < count; i++)
        {
            Vector3 spwanPos = transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(0.5f, 3f), Random.Range(-3f, 3f));
            objectPooler.SpwanObject(itemTag, spwanPos);
        }
        Destroy(this.gameObject);
    }
}