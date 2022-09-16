using System.Collections;
using UnityEngine;

public class Brute : EnemyMovement
{
    [Header("Brute")]
    [SerializeField] Transform body;

    // [Header("Throw")]
    // [SerializeField] float force;
    // [SerializeField] GameObject obj;
    // [SerializeField] Transform throwPos;

    [Header("Animation")]
    [SerializeField] AnimationClip[] attackClips;
    private AnimatorOverrideController overrideController;

    private float originalAttackDist;
    private bool isAttackChanged;

    protected override void Start()
    {
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
        originalAttackDist = attackDist;
        base.Start();
    }


    private IEnumerator IEChangeAttack()
    {
        yield return new WaitForSecondsRealtime(2);

        overrideController[attackClips[0].name] = attackClips[ChangeAttackAnim()];

        isAttackChanged = true;
        AttackState();
    }


    protected override void IdleState()
    {
        ResetTransform();
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

    // protected override void DyeState()
    // {
    //     base.DyeState();
        
    //     collider.radius = 0.1f;
    //     collider.height = 0.1f;
    //     collider.center = new Vector3(0, 1, 0);
    // }


    private void ResetTransform()
    {
        body.localPosition = Vector3.zero;
        body.localRotation = Quaternion.identity;
    }

    private int ChangeAttackAnim()
    {
        ResetTransform();
        if (playerDist < originalAttackDist)
        {
            attackDist = originalAttackDist;
            return Random.Range(0, attackClips.Length);
        }
        // else if (playerDist < moveAttack)
        // {
        //     attackState = Random.Range(4, 8);
        //     attackDist = moveAttack;
        // }
        // else if (playerDist < rockShower)
        // {
        //     attackState = 3;
        //     attackDist = rockShower;
        // }
        else
        {
            attackDist = originalAttackDist;
            return 0;
        }
    }


    public void ChangeAttack()
    {
        ResetTransform();
        if (!isAttack || attackClips.Length <= 0 || !overrideController) return;
        StopAllCoroutines();
        StartCoroutine(IEChangeAttack());
    }

    public void ThrowRock()
    {
        // Vector3 dir = player.position - throwPos.position + new Vector3(0, 1.8f, 0);
        // Rigidbody rock = Instantiate(obj, throwPos.position, Quaternion.identity).GetComponent<Rigidbody>();
        // rock.AddForce(dir.normalized * force, ForceMode.Impulse);
    }
}
