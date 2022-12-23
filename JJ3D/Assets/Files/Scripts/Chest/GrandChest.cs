using UnityEngine;

public class GrandChest : MonoBehaviour
{
    [SerializeField] GameObject objCanvas;
    [SerializeField] Animator animator;
    [SerializeField] TMPro.TMP_Text txtCost;
    // [SerializeField] Rigidbody coin;
    // [SerializeField] Rigidbody[] food;
    // [SerializeField] Rigidbody[] items;
    [SerializeField] string[] foodNames;
    [SerializeField] string[] itemsNames;
    private Transform cam;

    private int cost;

    private void Start()
    {
        cam = Camera.main.transform;
        cost = Random.Range(100, 600);
        txtCost.text = $"Get Grand Reward with {cost} coins";
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
        Vector3 pos;
        Rigidbody rewardItem;
        ObjectPooler objectPooler = ObjectPooler.instance;

        switch (Random.Range(0, 15))
        {
            case > 10:
                int coins = Random.Range(20, 60);
                for (int i = 0; i < coins; i++)
                {
                    pos = transform.position + new Vector3(Random.Range(-2, 2), 1, Random.Range(-2, 2));
                    rewardItem = objectPooler.SpwanObject("Coin", pos);
                    rewardItem.AddForce(Vector3.up * 8, ForceMode.Impulse);
                }
                break;
            case > 5:
                int foodCount = Random.Range(1, 4);
                for (int i = 0; i < foodCount; i++)
                {
                    pos = transform.position + new Vector3(Random.Range(-2, 2), 1, Random.Range(-2, 2));
                    rewardItem = objectPooler.SpwanObject(foodNames[Random.Range(0, foodNames.Length)], pos);
                    rewardItem.AddForce(Vector3.up * 10, ForceMode.Impulse);
                }
                break;
            default:
                rewardItem = objectPooler.SpwanObject(itemsNames[Random.Range(0, itemsNames.Length)], transform.position + new Vector3(0, 1, 0));
                rewardItem.AddForce(Vector3.up * 50, ForceMode.Impulse);
                break;
        }

        GameManager.instance.effects.ChestEffect(transform.position);

        Destroy(objCanvas.gameObject);
        Destroy(GetComponent<SphereCollider>());
        Destroy(GetComponent<GrandChest>());
    }
}
