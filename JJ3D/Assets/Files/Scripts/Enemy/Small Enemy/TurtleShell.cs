using UnityEngine;

public class TurtleShell : SmallEnemy
{
    [SerializeField] float force;
    [SerializeField] Transform[] attackPoints;
    private ObjectPooler objectPooler;
    private Rigidbody spike;

    protected override void Start()
    {
        objectPooler = ObjectPooler.instance;
        base.Start();
    }
   
    public void ShootSpikes()
    {
        Vector3 dir = player.position - attackPoints[0].position;
        dir = dir.normalized;
        for (int i = 0; i < 3; i++)
        {
            spike = objectPooler.SpwanObject("Spike", attackPoints[i].position);
            spike.rotation = Quaternion.LookRotation(player.position - attackPoints[i].position);
            spike.AddForce(dir * force, ForceMode.Impulse);
        }
    }
}
