using UnityEngine;

public class ShootFireBall : MonoBehaviour
{
    [SerializeField] GameObject fireBall;
    [SerializeField] Transform attackPoint;
    [SerializeField] float force;
    private Transform player;

    private void Start()
    {
        player = GameManager.instance.playerPos;
    }

    public void Shoot()
    {
        GameObject currFireBall = Instantiate(fireBall, attackPoint.position, attackPoint.rotation);
        Vector3 dir = player.position - attackPoint.position + new Vector3(0, 2, 0);
        dir = dir.normalized;
        currFireBall.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Impulse);
    }
}
