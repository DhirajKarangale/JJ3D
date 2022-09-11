using System.Collections;
using UnityEngine;

public class Brute : EnemyMovement
{
    [Header("Dist")]
    [SerializeField] float comboDist;
    [SerializeField] float jumpDist;

    [Header("Throw")]
    [SerializeField] float force;
    [SerializeField] float throwDist;
    [SerializeField] GameObject axeObj;
    [SerializeField] Transform throwPos;

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
            ChangeAttackAnim();
        }
    }

    public IEnumerator IEChangeAttack()
    {
        yield return new WaitForSecondsRealtime(2);

        ChangeAttackAnim();
        overrideController[attackClips[0].name] = attackClips[attackState];

        isAttackChanged = true;
        AttackState();
    }

    private void ChangeAttackAnim()
    {
        if (playerDist <= originalAttackDist)
        {
            attackState = Random.Range(0, 6);
            attackDist = originalAttackDist;
        }
        else if (playerDist <= comboDist)
        {
            attackState = Random.Range(6, 9);
            attackDist = comboDist;
        }
        else if (playerDist <= jumpDist)
        {
            attackState = 9;
            attackDist = jumpDist;
        }
        else if (playerDist <= throwDist)
        {
            attackState = 10;
            attackDist = throwDist;
        }
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
        // Vector3 dir = player.position - rockThrowPos.position + new Vector3(0, 1.8f, 0);
        // Rigidbody rock = Instantiate(rockObj, rockThrowPos.position, Quaternion.identity).GetComponent<Rigidbody>();
        // rock.AddForce(dir.normalized * force, ForceMode.Impulse);
    }
}
