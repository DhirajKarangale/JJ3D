using UnityEngine;
using VolumetricLines;

public class TurtleShell : EnemyMovement
{
    [SerializeField] float force;
    [SerializeField] GameObject spikePrefab;
    [SerializeField] VolumetricLineBehavior laser;
    [SerializeField] Transform laserPos;
    [SerializeField] Transform[] attackPoints;
    [SerializeField] AnimationClip[] attackClips;
    private AnimatorOverrideController overrideController;
    private bool isShootLaser;

    protected override void Start()
    {
        base.Start();
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Laser();
    }

    private void Laser()
    {
        if (!isAttack && laser.gameObject.activeInHierarchy)
        {
            laser.gameObject.SetActive(false);
            isShootLaser = false;
        }

        if (isShootLaser)
        {
            isShootLaser = isAttack;
            laser.gameObject.SetActive(true);
            laser.transform.LookAt(player);
            laser.transform.position = laserPos.position;
            laser.StartPos = Vector3.zero;
            float playerDist = 11 * Mathf.Abs(Vector3.Distance(transform.position, player.position));
            laser.EndPos = new Vector3(0, 0, playerDist);
            gameManager.PlayerBloodEffect(player.position);
            // playerHealth.TakeDamage(0.04f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!rigidBody.useGravity)
        {
            rigidBody.isKinematic = true;
            CancelInvoke();
            Invoke("DesableKinematics", 0.1f);
        }
    }

    private void DesableKinematics()
    {
        rigidBody.isKinematic = false;
    }

    protected override void AttackState()
    {
        // int attack = Random.Range(0, attackClips.Length);
        int attack = 2;
        float originalAttackDist = attackDist;
        attackDist = attack != 0 ? originalAttackDist * 1.5f : originalAttackDist;
        overrideController[attackClips[0].name] = attackClips[attack];
        base.AttackState();
    }

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

    public void ShootLaser()
    {
        // line.enabled = true;
        // line.SetPosition(0, player.position + new Vector3(0, 0.5f, 0));
        // line.SetPosition(1, laserPos.position);
        // line.transform.position = player.position;
        isShootLaser = true;
    }
}
