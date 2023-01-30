using UnityEngine;
using UnityEngine.EventSystems;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] InventoryData inventoryData;
    [SerializeField] float interactRadius = 10f;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    private void Update()
    {
        GetInput();
    }

    public void PickUp(Item item, ItemData itemData)
    {
        if (item)
        {
            gameManager.effects.PickEffect(item.transform.position);
            inventoryData.AddItem(item.itemData);
            item.DesableItem();
        }
        else if (itemData)
        {
            inventoryData.AddItem(itemData);
        }
    }

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            Interact(Input.mousePosition);
        }
    }

    private void Interact(Vector3 interactPos)
    {
        Ray ray = cam.ScreenPointToRay(interactPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Item item = hit.collider.GetComponent<Item>();
            if (item && Vector3.Distance(transform.position, item.transform.position) > interactRadius) return;
            PickUp(item, null);
        }
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     // Pick On Collide
    //     Item item = collision.gameObject.GetComponent<Item>();
    //     PickUp(item, null);
    // }
}
