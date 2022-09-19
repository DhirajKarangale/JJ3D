using System.Collections;
using UnityEngine;

public class Brute : EnemyMovement
{
    [Header("Brute")]
    [SerializeField] Transform body;

    [Header("Animation")]
    [SerializeField] AnimationClip[] attackClips;
    private AnimatorOverrideController overrideController;

    protected override void Start()
    {
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
        base.Start();
    }

    protected override void Update()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) return;
        base.Update();
        ResetTransform();
    }

    private IEnumerator IEChangeAttack()
    {
        yield return new WaitForSecondsRealtime(2);
        overrideController[attackClips[0].name] = attackClips[ChangeAttackAnim()];
        AttackState();
    }

    private void ResetTransform()
    {
        body.localPosition = Vector3.zero;
        body.localRotation = Quaternion.identity;
    }

    private int ChangeAttackAnim()
    {
        return Random.Range(0, attackClips.Length);
    }

    public void ChangeAttack()
    {
        if (!isAttack || attackClips.Length <= 0 || !overrideController) return;
        StopAllCoroutines();
        StartCoroutine(IEChangeAttack());
    }
}
