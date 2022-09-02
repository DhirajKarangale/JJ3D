using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody rigidBody;

    [Header("Attributes")]
    [SerializeField] protected float moveSpeed;

    protected Transform terran;
    protected Vector3 targetPos;
    protected Transform myTransform;
    protected Vector3[] terranVertices;

    protected bool isIdle;
    protected bool isWalk;
    protected bool isRun;
    protected bool isAttack;
    protected bool isDye;

    protected virtual void Start()
    {
        myTransform = transform;
        IdleState();
    }

    protected virtual void FixedUpdate()
    {
        if (isDye) return;
        
        Look();

        if (isWalk) Move(moveSpeed);
        if (isRun) Move(moveSpeed * 2);

        if (myTransform.position.y <= -60)
        {
            transform.position = terran.position + new Vector3(0, 5, 0);
        }
    }


    protected virtual IEnumerator IEWalkIdle()
    {
        IdleState();

        yield return new WaitForSeconds(5);

        ChangePos();
        WalkState();

        yield return new WaitForSeconds(10);

        StartCoroutine(IEWalkIdle());
    }

    protected virtual IEnumerator IERun()
    {
        ChangePos();
        RunState();

        yield return new WaitForSeconds(15);

        IdleState();
        StartCoroutine(IEWalkIdle());
    }


    protected virtual void Move(float speed)
    {

    }

    protected virtual void Look()
    {
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(targetPos - myTransform.position), 2 * Time.deltaTime);
    }

    public void StartPos(Vector3[] vertices, Transform itemParent)
    {
        terranVertices = vertices;
        terran = itemParent;
        StopAllCoroutines();
        StartCoroutine(IEWalkIdle());
    }

    internal virtual void Hurt()
    {
        
    }

    public void Dye()
    {
        StopAllCoroutines();
        DyeState();
        rigidBody.mass = 100;
    }

    protected virtual void ChangePos()
    {
        int verticeIndex = Random.Range(0, terranVertices.Length);
        targetPos = new Vector3(terranVertices[verticeIndex].x + terran.position.x, terranVertices[verticeIndex].y - 1, terranVertices[verticeIndex].z + terran.position.z);
    }

    protected virtual void IdleState()
    {
        isIdle = true;
        isWalk = false;
        isRun = false;
        isAttack = false;
        SetAnimation();
    }

    protected virtual void WalkState()
    {
        isIdle = false;
        isWalk = true;
        isRun = false;
        isAttack = false;
        SetAnimation();
    }

    protected virtual void RunState()
    {
        isIdle = false;
        isWalk = false;
        isRun = true;
        isAttack = false;
        SetAnimation();
    }

    protected virtual void DyeState()
    {
        isDye = true;
        isIdle = false;
        isWalk = false;
        isRun = false;
        isAttack = false;
        SetAnimation();
        animator.Play("Death");
    }

    protected virtual void SetAnimation()
    {
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isWalk", isWalk);
        animator.SetBool("isRun", isRun);
    }
}
