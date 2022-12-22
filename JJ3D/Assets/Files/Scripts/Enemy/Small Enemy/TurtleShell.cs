using UnityEngine;

public class TurtleShell : SmallEnemy
{
    [SerializeField] float force;
    [SerializeField] GameObject spikePrefab;
    [SerializeField] Transform[] attackPoints;
    private ObjectPooler objectPooler;

    protected override void Start()
    {
        base.Start();
        objectPooler = ObjectPooler.instance;
    }
   
    public void ShootSpikes()
    {
        Vector3 dir = player.position - attackPoints[0].position;
        dir = dir.normalized;
        for (int i = 0; i < 3; i++)
        {
            // GameObject spike = Instantiate(spikePrefab, attackPoints[i].position, Quaternion.identity);
            GameObject spike = objectPooler.SpwanObject("Spike", attackPoints[i].position);
            spike.transform.rotation = Quaternion.LookRotation(player.position - attackPoints[i].position);
            spike.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Impulse);
        }
    }
}
