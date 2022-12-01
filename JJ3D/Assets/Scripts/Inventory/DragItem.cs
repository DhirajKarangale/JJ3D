using UnityEngine;

public class DragItem : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] InventorySlot inventoryItem;

    private void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, Input.mousePosition, canvas.worldCamera, out position);
        transform.position = canvas.transform.TransformPoint(position);
    }

    public void SetData(Sprite sprite, int count)
    {
        inventoryItem.SetData(sprite, count);
    }

    public void Active(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
