using UnityEngine;
using System.Collections;

public class Golem : EnemyMovement
{
    [Header("Throw")]
    [SerializeField] string rockTag;
    [SerializeField] float force;
    [SerializeField] float rockThrow;
    [SerializeField] Transform rockThrowPos;

    [Header("Shower")]
    [SerializeField] float rockShower;

    [Header("Animation")]
    [SerializeField] AnimationClip[] attackClips;
    private AnimatorOverrideController overrideController;

    private ObjectPooler objectPooler;
    private Rigidbody rock;
    private float originalAttackDist;
    private int attackState;

    protected override void Start()
    {
        objectPooler = ObjectPooler.instance;
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
        originalAttackDist = attackDist;
        base.Start();
    }

    protected override void Update()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
        base.Update();
        Look();
        ChangeAttackAnim();
    }

    private IEnumerator IEChangeAttack()
    {
        yield return new WaitForSecondsRealtime(2);

        ChangeAttackAnim();
        overrideController[attackClips[0].name] = attackClips[attackState];
        AttackState();
    }


    private void ChangeAttackAnim()
    {
        if (playerDist < originalAttackDist)
        {
            attackState = Random.Range(0, 2);
            attackDist = originalAttackDist;
        }
        else if (playerDist < rockThrow)
        {
            attackState = Random.value <= 0.1f ? 3 : 2;
            attackDist = rockThrow;
        }
        else if (playerDist < rockShower)
        {
            attackState = 3;
            attackDist = rockShower;
        }
        else
        {
            attackState = Random.Range(0, 2);
            attackDist = originalAttackDist;
        }
    }


    public void ChangeAttack()
    {
        if (!isAttack || attackClips.Length <= 0 || !overrideController) return;
        StopAllCoroutines();
        StartCoroutine(IEChangeAttack());
    }

    public void ThrowRock()
    {
        Vector3 dir = player.position - rockThrowPos.position + new Vector3(0, 1.8f, 0);
        rock = objectPooler.SpwanObject(rockTag, rockThrowPos.position);
        rock.AddForce(dir.normalized * force, ForceMode.Impulse);
    }

    public void RockShower()
    {
        float minX = player.position.x - originalAttackDist;
        float maxX = player.position.x + originalAttackDist;

        float minY = transform.position.y + 5;
        float maxY = transform.position.y + 40;

        float minZ = player.position.z - originalAttackDist;
        float maxZ = player.position.z + originalAttackDist;

        for (int i = 0; i < 4; i++)
        {
            float posX = Random.Range(minX, maxX);
            float posY = Random.Range(minY, maxY);
            float posZ = Random.Range(minZ, maxZ);

            rock = objectPooler.SpwanObject(rockTag, new Vector3(posX, posY, posZ));
        }

    }
}
