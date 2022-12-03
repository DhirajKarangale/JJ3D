using UnityEngine;

public class GrandChest : MonoBehaviour
{
    [SerializeField] GameObject objCanvas;
    [SerializeField] Animator animator;
    [SerializeField] TMPro.TMP_Text txtCost;
    [SerializeField] Rigidbody coin;
    [SerializeField] Rigidbody[] food;
    [SerializeField] Rigidbody[] items;

    private int cost;

    private void Start()
    {
        cost = Random.Range(100, 600);
        txtCost.text = $"Get Grand Reward with {cost} coins";
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
        switch (Random.Range(0, 15))
        {
            case > 10:
                int coins = Random.Range(20, 60);
                for (int i = 0; i < coins; i++)
                {
                    Vector3 pos = transform.position + new Vector3(Random.Range(-2, 2), 1, Random.Range(-2, 2));
                    Rigidbody rewardCoin = Instantiate(coin, pos, Quaternion.identity);
                    rewardCoin.AddForce(Vector3.up * 8, ForceMode.Impulse);
                }
                break;
            case > 5:
                int foodCount = Random.Range(1, 4);
                int foodIndex = Random.Range(0, food.Length);
                for (int i = 0; i < foodCount; i++)
                {
                    Vector3 pos = transform.position + new Vector3(Random.Range(-2, 2), 1, Random.Range(-2, 2));
                    Rigidbody rewardFood = Instantiate(food[foodIndex], pos, Quaternion.identity);
                    rewardFood.AddForce(Vector3.up * 10, ForceMode.Impulse);
                }
                break;
            default:
                Rigidbody rewardItem = Instantiate(items[Random.Range(0, items.Length)], transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                rewardItem.AddForce(Vector3.up * 50, ForceMode.Impulse);
                break;
        }

        GameManager.instance.effects.ChestEffect(transform.position);

        Destroy(objCanvas.gameObject);
        Destroy(GetComponent<SphereCollider>());
        Destroy(GetComponent<GrandChest>());
    }
}
