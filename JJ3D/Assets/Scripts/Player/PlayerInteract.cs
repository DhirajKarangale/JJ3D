using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] Camera cam;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    private void Update()
    {
        GetInput();
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

            if (item)
            {
                if (Vector3.Distance(transform.position, item.transform.position) > item.interactRadius) return;
                gameManager.PickEffet(item.transform.position);
                item.Pickup();
            }
        }
    }
}
