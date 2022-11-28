using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] InventoryItem inventoryItem;

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
