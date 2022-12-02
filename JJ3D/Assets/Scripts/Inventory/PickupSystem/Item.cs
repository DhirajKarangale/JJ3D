using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    [SerializeField] GameObject obj;
    [SerializeField] Rigidbody rigidBody;
    private Vector3 startScale;
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
        obj.SetActive(false);
        rigidBody.isKinematic = true;
    }

    internal void DesableItem()
    {
        StartCoroutine(AnimateItemPickup());
    }

    internal void ThrowItem(Vector3 pos)
    {
        obj.SetActive(true);
        rigidBody.isKinematic = false;
        transform.localScale = startScale;
        transform.position = pos + new Vector3(0, 5, 0);
        rigidBody.velocity = Vector3.zero;
    }

    internal void DestoryItem()
    {
        Destroy(this.gameObject);
    }
}
