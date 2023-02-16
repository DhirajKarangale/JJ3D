using UnityEngine;

public class ShootFireBall : MonoBehaviour
{
    [SerializeField] GameObject fireBall;
    [SerializeField] Transform attackPoint;
    [SerializeField] float force;
    private Transform player;
    private ObjectPooler objectPooler;

    private Rigidbody currFireBall;
    private Vector3 dir;

    private void Start()
    {
        player = GameManager.instance.playerPos;
        objectPooler = ObjectPooler.instance;
    }

    public void Shoot()
    {
        // currFireBall = Instantiate(fireBall, attackPoint.position, attackPoint.rotation);
        currFireBall = objectPooler.SpwanObject("Fireball",attackPoint.position);
        currFireBall.rotation = attackPoint.rotation;
        dir = player.position - attackPoint.position;
        dir = dir.normalized;
        currFireBall.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Impulse);
    }
}
