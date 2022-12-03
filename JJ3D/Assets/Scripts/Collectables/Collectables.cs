using UnityEngine;

public class Collectables : MonoBehaviour
{
    private enum CollectableType { Coin, Key }
    [SerializeField] CollectableType collectableType;
    [SerializeField] float speed;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (collectableType)
            {
                case CollectableType.Coin:
                    CollectableData.instance.UpdateCoin(5, transform.position);
                    break;
                case CollectableType.Key:
                    CollectableData.instance.UpdateKey(1, transform.position);
                    break;
            }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            transform.position = Vector3.MoveTowards(transform.position, collider.transform.position + new Vector3(0, 0.5f, 0), Time.deltaTime * speed);
        }
    }
}