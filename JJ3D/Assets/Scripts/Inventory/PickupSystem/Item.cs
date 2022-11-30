using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field: SerializeField]
    public InventoryItemData InventoryItem { get; private set; }

    [field: SerializeField]
    public int Count { get; set; } = 1;

    [SerializeField] float duration = 0.3f;

    internal void DestroyItem()
    {
        GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(AnimateItemPickup());
    }

    private IEnumerator AnimateItemPickup()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currTime = 0;
        while (currTime < duration)
        {
            currTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currTime / duration);
            yield return null;
        }
        transform.localScale = endScale;
        Destroy(gameObject);
    }
}
