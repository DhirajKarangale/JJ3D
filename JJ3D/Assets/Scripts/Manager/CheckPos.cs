using UnityEngine;

public class CheckPos : MonoBehaviour
{
    [SerializeField] Rigidbody rigidBody;

    private void Start()
    {
        InvokeRepeating("Check", 10, 10);
    }

    private void Check()
    {
        if (transform.position.y < -100)
        {
            transform.position = new Vector3(transform.position.x, 20, transform.position.z);
            rigidBody.velocity = Vector3.zero;
        }
    }
}
