using UnityEngine;

public class Projectile : MonoBehaviour
{
    private enum DestroyEffect { Fireball, Object };
    [SerializeField] bool isCollisionDestroy;
    [SerializeField] DestroyEffect destroyEffect;

    private void Start()
    {
        Invoke("DestroyProjectile", 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isCollisionDestroy)
        {
            DestroyProjectile();
        }
        else
        {
            GameManager.instance.HitSound(transform.position);
        }
    }

    private void DestroyProjectile()
    {
        if (destroyEffect == DestroyEffect.Object)
        {
            GameManager.instance.DestroyEffect(transform.position);
        }
        else
        {
            GameManager.instance.FireballDestroyEffect(transform.position);
        }
        Destroy(this.gameObject);
    }
}
