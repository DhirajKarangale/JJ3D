using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    internal Vector3 startScale;
    private float duration = 0.3f;

    private void Start()
    {
        itemData.currHealth = itemData.mxHealth;
    }

    private IEnumerator AnimateItemPickup()
    {
        startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currTime = 0;
        while (currTime < duration)
        {
            currTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currTime / duration);
            yield return null;
        }
        transform.localScale = endScale;

        yield return null;
        transform.localScale = startScale;
        gameObject.SetActive(false);
    }

    internal void DesableItem()
    {
        StartCoroutine(AnimateItemPickup());
    }
}
