using UnityEngine;

public class CoinChest : MonoBehaviour
{
    [SerializeField] GameObject objCanvas;
    [SerializeField] Animator animator;
    [SerializeField] TMPro.TMP_Text txtCost;
    [SerializeField] Rigidbody coins;
    private Transform cam;

    private int cost;

    private void Start()
    {
        cam = Camera.main.transform;
        cost = Random.Range(5, 100);
        txtCost.text = $"Get Coins with {cost} coins";
    }

    private void LateUpdate()
    {
        objCanvas.transform.LookAt(transform.position - cam.forward);
    }



    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            objCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            objCanvas.SetActive(false);
        }
    }

    public void ButtonOpen()
    {
        if (GameManager.instance.collectableData.coins < cost)
        {
            Msg.instance.DisplayMsg("Not Enough Coins", Color.red);
            GameManager.instance.collectableData.ShakeCoins();
            return;
        }
        GameManager.instance.collectableData.UpdateCoin(-cost, Vector3.zero);
        GameManager.instance.effects.ButtonSound();
        objCanvas.SetActive(false);
        animator.Play("Play");
    }

    // Acess by Anim Event
    public void ChestOpen()
    {
        if (cost > 15) cost /= 5;
        int reward = Random.Range(1, 10) * cost;
        if (cost > 15) reward = Random.Range(1, 5) * cost;

        for (int i = 0; i < reward; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(-2, 2), 1, Random.Range(-2, 2));
            Rigidbody item = Instantiate(coins, pos, Quaternion.identity);
            item.AddForce(Vector3.up * 8, ForceMode.Impulse);
        }
        GameManager.instance.effects.ChestEffect(transform.position);

        Destroy(objCanvas.gameObject);
        Destroy(GetComponent<SphereCollider>());
        Destroy(GetComponent<CoinChest>());
    }
}
