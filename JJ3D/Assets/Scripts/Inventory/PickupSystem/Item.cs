using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    private Vector3 startScale;
    private float duration = 0.3f;

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
        // this.gameObject.SetActive(false);
    }

    internal void DesableItem()
    {
        StartCoroutine(AnimateItemPickup());
    }

    internal void ThrowItem(Vector3 pos)
    {
        transform.localScale = startScale;
        transform.position = pos + new Vector3(5, 5, 5);
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(Vector3.up * 10);
    }
}
