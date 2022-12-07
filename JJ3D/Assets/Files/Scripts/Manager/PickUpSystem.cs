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

    public void PickUp(Item item)
    {
        if (item)
        {
            gameManager.effects.PickEffect(item.transform.position);
            inventoryData.AddItem(item);
            item.DesableItem();
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
            PickUp(item);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Item item = collision.gameObject.GetComponent<Item>();
        // PickUp(item);
    }
}
