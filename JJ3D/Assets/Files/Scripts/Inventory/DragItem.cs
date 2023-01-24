using UnityEngine;

public class DragItem : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] InventorySlot inventoryItem;
    private Vector2 position;

    private void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, Input.mousePosition, canvas.worldCamera, out position);
        transform.position = canvas.transform.TransformPoint(position);
    }

    public void SetData(Sprite sprite)
    {
        inventoryItem.SetData(sprite);
    }

    public void Active(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
