using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float itemInteractRadius = 10f;
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
            ItemOld item = hit.collider.GetComponent<ItemOld>();

            if (item)
            {
                if (Vector3.Distance(transform.position, item.transform.position) > itemInteractRadius) return;
                gameManager.PickEffect(item.transform.position);
                item.Pickup();
            }
        }
    }
}
