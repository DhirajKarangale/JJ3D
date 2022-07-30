using UnityEngine;
using System.Collections;

public class AnimalMovement : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rigidBody;

    [Header("Attributes")]
    [SerializeField] float moveSpeed;

    private Transform terran;
    private Vector3 targetPos;
    private Transform myTransform;
    private Vector3[] terranVertices;

    private bool isIdle;
    private bool isWalk;
    private bool isRun;

    private void Start()
    {
        myTransform = transform;
        IdleState();
    }

    private void Update()
    {
        Look();

        if (isWalk) Move(moveSpeed);
        if (isRun) Move(moveSpeed * 2);

        if (myTransform.position.y <= -60)
        {
            transform.position = terran.position + new Vector3(0, 5, 0);
        }
    }


    private IEnumerator IEWalkIdle()
    {
        IdleState();

        yield return new WaitForSeconds(5);

        ChangePos();
        WalkState();

        yield return new WaitForSeconds(10);

        StartCoroutine(IEWalkIdle());
    }

    private IEnumerator IERun()
    {
        ChangePos();
        RunState();

        yield return new WaitForSeconds(15);

        IdleState();
        StartCoroutine(IEWalkIdle());
    }


    private void Move(float speed)
    {
        if (Vector3.Distance(myTransform.position, targetPos) > 7f)
        {
            myTransform.position += myTransform.forward * speed * Time.deltaTime;
            // rigidBody.velocity = myTransform.forward * speed * Time.deltaTime;
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(IEWalkIdle());
        }
    }

    private void Look()
    {
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(targetPos - myTransform.position), 2 * Time.deltaTime);
    }

    public void StartPos(Vector3[] vertices, Transform itemParent)
    {
        terranVertices = vertices;
        terran = itemParent;

        StartCoroutine(IEWalkIdle());
    }

    public void Hurt()
    {
        StopAllCoroutines();
        StartCoroutine(IERun());
    }

    public void Dye()
    {
        StopAllCoroutines();
        DyeState();
    }

    private void ChangePos()
    {
        int verticeIndex = Random.Range(0, terranVertices.Length);
        targetPos = new Vector3(terranVertices[verticeIndex].x + terran.position.x, terranVertices[verticeIndex].y - 1, terranVertices[verticeIndex].z + terran.position.z);
    }

    private void IdleState()
    {
        isIdle = true;
        isWalk = false;
        isRun = false;
        SetAnimation();
    }

    private void WalkState()
    {
        isIdle = false;
        isWalk = true;
        isRun = false;
        SetAnimation();
    }

    private void RunState()
    {
        isIdle = false;
        isWalk = false;
        isRun = true;
        SetAnimation();
    }

    private void DyeState()
    {
        isIdle = false;
        isWalk = false;
        isRun = false;
        SetAnimation();
        animator.Play("Death");
    }

    private void SetAnimation()
    {
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isWalk", isWalk);
        animator.SetBool("isRun", isRun);
    }
}