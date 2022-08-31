using UnityEngine;

public class TurtleShell : SmallEnemy
{
    [SerializeField] float force;
    [SerializeField] GameObject spikePrefab;
    [SerializeField] Transform[] attackPoints;
   
    public void ShootSpikes()
    {
        Vector3 dir = player.position - attackPoints[0].position;
        dir = dir.normalized;
        for (int i = 0; i < 5; i++)
        {
            GameObject spike = Instantiate(spikePrefab, attackPoints[i].position, Quaternion.identity);
            spike.transform.rotation = Quaternion.LookRotation(player.position - transform.position);
            spike.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Impulse);
        }
    }
}
