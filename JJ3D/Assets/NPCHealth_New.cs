using UnityEngine;

public class NPCHealth_New : MonoBehaviour
{
    [SerializeField] HealthBar healthBar;
    [SerializeField] NPC_New npc;
    [SerializeField] float mxHealth;
    [SerializeField] string itemTag;
    [SerializeField] int itemCount;

    internal float health;


    private void Start()
    {
        health = mxHealth;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);
        }
    }


    private void SetHealthBar()
    {
        healthBar.SetBar(health / mxHealth);
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
    }

    private void DestroyBody()
    {
        GameManager.instance.effects.DestroyBodyEffect(transform.position);
        SpwanItem();
        Destroy(this.gameObject);
    }

    private void Dye()
    {
        CamController.instance.Shake(0.9f);
        Invoke("DestroyBody", 5);
        npc.Dye();
    }

    private void Hurt()
    {
        npc.Hurt();
        // NPC_New -> Hurt -> Run 
    }


    internal void TakeDamage(float damage)
    {
        health -= damage;
        SetHealthBar();
        if (health <= 0) Dye();
        else Hurt();
    }
}