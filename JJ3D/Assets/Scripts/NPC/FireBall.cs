using UnityEngine;

public class FireBall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GameManager.instance.FireballDestroyEffect(transform.position);
        Destroy(this.gameObject);
    }
}
