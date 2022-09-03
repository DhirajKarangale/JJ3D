using UnityEngine;
using System.Collections;

public class Golem : EnemyMovement
{
    [Header("Throw")]
    [SerializeField] float force;
    [SerializeField] float rockThrow;
    [SerializeField] GameObject rockObj;
    [SerializeField] Transform rockThrowPos;

    [Header("Shower")]
    [SerializeField] float rockShower;

    [Header("Animation")]
    [SerializeField] AnimationClip[] attackClips;
    private AnimatorOverrideController overrideController;

    private float originalAttackDist;
    private int attackState;
    private bool isAttackChanged;

    protected override void Start()
    {
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
        originalAttackDist = attackDist;
        base.Start();
    }

    private void Update()
    {
        if (isAttack)
        {
            if (playerDist < originalAttackDist)
            {
                attackState = Random.Range(0, 2);
                attackDist = originalAttackDist;
            }
            else if (playerDist < rockThrow)
            {
                attackState = Random.value <= 0.2f ? 3 : 2;
                attackDist = rockThrow;
            }
            else if (playerDist < rockShower)
            {
                attackState = 3;
                attackDist = rockShower;
            }
        }
    }

    private IEnumerator IEChangeAttack()
    {
        yield return new WaitForSecondsRealtime(2);

        if (playerDist < originalAttackDist)
        {
            attackState = Random.Range(0, 2);
            attackDist = originalAttackDist;
        }
        else if (playerDist < rockThrow)
        {
            attackState = Random.value <= 0.2f ? 3 : 2;
            attackDist = rockThrow;
        }
        else if (playerDist < rockShower)
        {
            attackState = 3;
            attackDist = rockShower;
        }
        overrideController[attackClips[0].name] = attackClips[attackState];

        isAttackChanged = true;
        AttackState();
    }

    protected override void IdleState()
    {
        base.IdleState();
        if (!isAttackChanged) ChangeAttack();
    }

    protected override void WalkState()
    {
        base.WalkState();
        if (!isAttackChanged) ChangeAttack();
    }

    protected override void RunState()
    {
        base.RunState();
        if (!isAttackChanged) ChangeAttack();
    }

    protected override void AttackState()
    {
        isAttackChanged = false;
        base.AttackState();
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
        Rigidbody rock = Instantiate(rockObj, rockThrowPos.position, Quaternion.identity).GetComponent<Rigidbody>();
        rock.AddForce(dir.normalized * force, ForceMode.Impulse);
    }

    public void RockShower()
    {
        float minX = player.position.x - originalAttackDist;
        float maxX = player.position.x + originalAttackDist;

        float minY = transform.position.y + 40;
        float maxY = transform.position.y + 100;

        float minZ = player.position.z - originalAttackDist;
        float maxZ = player.position.z + (originalAttackDist * 2);

        for (int i = 0; i < 7; i++)
        {
            float posX = Random.Range(minX, maxX);
            float posY = Random.Range(minY, maxY);
            float posZ = Random.Range(minZ, maxZ);

            Vector3 pos = new Vector3(posX, posY, posZ);

            GameObject rock = Instantiate(rockObj, pos, Quaternion.identity);
            rock.transform.localScale *= 3;
        }

    }
}
