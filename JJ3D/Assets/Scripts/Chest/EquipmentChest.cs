using UnityEngine;

public class EquipmentChest : MonoBehaviour
{
    [SerializeField] string itemName;
    [SerializeField] GameObject objCanvas;
    [SerializeField] Animator animator;
    [SerializeField] TMPro.TMP_Text txtCost;
    [SerializeField] Rigidbody[] items;
    private Transform cam;

    private int cost;

    private void Start()
    {
        cam = Camera.main.transform;
        cost = Random.Range(80, 400);
        txtCost.text = $"Get {itemName} with {cost} coins";
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
        Rigidbody item = Instantiate(items[Random.Range(0, items.Length)], transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        item.AddForce(Vector3.up * 50, ForceMode.Impulse);
        GameManager.instance.effects.ChestEffect(transform.position);

        Destroy(objCanvas.gameObject);
        Destroy(GetComponent<SphereCollider>());
        Destroy(GetComponent<EquipmentChest>());
    }
}
