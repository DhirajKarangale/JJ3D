using UnityEngine;

public class CheckPos : MonoBehaviour
{
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] Transform playerPos;

    private void Start()
    {
        InvokeRepeating("Check", 10, 10);
        playerPos = GameManager.instance.player.transform;
    }

    private void Check()
    {
        if (transform.position.y < -100)
        {
            rigidBody.velocity = Vector3.zero;
            transform.position = playerPos.position + new Vector3(0, 3, 0);
        }
    }
}
