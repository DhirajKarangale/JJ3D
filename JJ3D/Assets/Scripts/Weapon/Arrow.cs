using UnityEngine;

public class Arrow : MonoBehaviour
{
    private void Start()
    {
        Invoke("DestroyArrow", 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameManager.instance.HitSound(transform.position);
    }

    private void DestroyArrow()
    {
        GameManager.instance.DestroyEffect(transform.position);
        Destroy(this.gameObject);
    }
}
