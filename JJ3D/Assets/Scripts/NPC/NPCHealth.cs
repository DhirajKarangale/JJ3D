using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    [SerializeField] AnimalMovement animalMovement;
    [SerializeField] EnemyMovement enemyMovement;
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] GameObject item;
    [SerializeField] Transform healthBar;
    [SerializeField] GameObject objHealth;
    [SerializeField] float mxHealth;
    private float health;

    private void Start()
    {
        objHealth.SetActive(false);
        health = mxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        objHealth.SetActive(health > 0);
        healthBar.localScale = new Vector3(health / mxHealth, 1, 1);
        CancelInvoke("DesableHealthBar");
        Invoke("DesableHealthBar", 20);

        if (health <= 0) Dye();
        else if (animalMovement) animalMovement.Hurt();
    }

    private void DesableHealthBar()
    {
        objHealth.SetActive(false);
    }

    private void Dye()
    {
        if (IsInvoking("DestroyBody")) return;
        if (animalMovement) animalMovement.Dye();
        else if (enemyMovement) enemyMovement.Dye();
        rigidBody.AddForce(Vector3.down * 15, ForceMode.Impulse);
        rigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        Invoke("DestroyBody", 3);
    }

    private void DestroyBody()
    {
        GameManager.instance.DestroyBodyEffect(transform.position);
        Instantiate(item, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
        Destroy(this.gameObject);
    }
}
