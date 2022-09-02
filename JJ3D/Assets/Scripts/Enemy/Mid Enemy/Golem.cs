using UnityEngine;

public class Golem : EnemyMovement
{
    [Header("Dist")]
    [SerializeField] float rockThrow;
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

    protected override void AttackState()
    {
        isAttackChanged = false;
        base.AttackState();
    }

    public void ChangeAttack()
    {
        if (!isAttack && attackClips.Length <= 0 || !overrideController) return;

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
}
