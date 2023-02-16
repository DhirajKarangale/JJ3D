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
                    CollectableData.instance.UpdateCoin(5, collision.GetContact(0).point);
                    break;
                case CollectableType.Key:
                    CollectableData.instance.UpdateKey(1, collision.GetContact(0).point);
                    break;
            }
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (GameManager.instance.isGameOver)
            {
                this.enabled = false;
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, collider.transform.position + new Vector3(0, 0.5f, 0), Time.deltaTime * speed);
        }
    }
}