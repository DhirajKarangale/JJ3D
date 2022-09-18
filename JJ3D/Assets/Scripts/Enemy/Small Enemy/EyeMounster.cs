using UnityEngine;
using VolumetricLines;

public class EyeMounster : SmallEnemy
{
    [Header("Laser")]
    [SerializeField] VolumetricLineBehavior laser;
    [SerializeField] Transform laserPos;
    private bool isShootLaser;

    [Header("Spike")]
    [SerializeField] float force;
    [SerializeField] GameObject spikePrefab;
    [SerializeField] Transform[] attackPoints;

    protected override void Update()
    {
        base.Update();
        Laser();
    }

    private void OnCollisionEnter(Collision collision)
    {
        rigidBody.isKinematic = true;
        CancelInvoke();
        Invoke("DesableKinematics", 0.1f);
    }

    private void DesableKinematics()
    {
        rigidBody.isKinematic = false;
    }

    private void Laser()
    {
        if (!laser) return;

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
            float playerDist = 11 * Mathf.Abs(Vector3.Distance(transform.position, player.position + new Vector3(0, 1, 0)));
            // Debug.Log("Player Dist : " + playerDist);
            laser.EndPos = new Vector3(0, 0, playerDist);
            gameManager.PlayerBloodEffect(player.position);
            playerHealth.TakeDamage(0.04f);
        }
    }

    public void ShootLaser()
    {
        isShootLaser = true;
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
}
