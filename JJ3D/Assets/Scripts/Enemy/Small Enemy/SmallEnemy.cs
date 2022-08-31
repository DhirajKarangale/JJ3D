using UnityEngine;

public class SmallEnemy : EnemyMovement
{
    [SerializeField] float attackDistMulti = 3f;
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

    private void ChangeAttack()
    {
        if (attackClips.Length <= 0 || !overrideController) return;
        attackState = Random.Range(0, attackClips.Length);
        overrideController[attackClips[0].name] = attackClips[attackState];
        attackDist = attackState == 0 ? originalAttackDist : originalAttackDist * attackDistMulti;
        isAttackChanged = true;
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
