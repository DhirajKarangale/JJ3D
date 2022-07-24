using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rigidBody;

    [Header("Attributes")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float attackDist;
    [SerializeField] float followDist;

    private Transform terran;
    private Vector3 targetPos;
    private Transform myTransform;
    private Transform player;
    private Vector3[] terranVertices;
    private GameManager gameManager;
    private float playerDist;

    private bool isIdle;
    private bool isJump;
    private bool isWalk;
    private bool isRun;
    private bool isAttack;

    private void Start()
    {
        myTransform = transform;
        gameManager = GameManager.instance;
        player = GameManager.instance.playerAttack.transform;
        IdleState();
    }

    private void FixedUpdate()
    {
        Look();

        playerDist = Mathf.Abs(Vector3.Distance(myTransform.position, player.position));

        if (gameManager.isGameOver)
        {
            StartCoroutine(IEWalkIdle());
        }
        else
        {
            if (!isAttack && playerDist < attackDist) Attack();
            else if (isAttack && playerDist > (attackDist + 1.5f) && playerDist < followDist) FollowPlayer();
            else if (!isAttack && playerDist < followDist) FollowPlayer();
            else if (!isAttack && playerDist > followDist) StartCoroutine(IEWalkIdle());
        }

        if (isWalk) Move(moveSpeed);
        if (isRun) Move(moveSpeed * 2);

        if (myTransform.position.y <= -60)
        {
            myTransform.position = terran.position + new Vector3(0, 5, 0);
        }
    }


    private IEnumerator IEWalkIdle()
    {
        if (!isWalk)
        {
            IdleState();

            yield return new WaitForSeconds(5);

            ChangePos();
            WalkState();

            yield return new WaitForSeconds(10);

            StartCoroutine(IEWalkIdle());
        }
    }


    private void Move(float speed)
    {
        myTransform.position += myTransform.forward * speed * Time.deltaTime;

        RaycastHit hit;
        if (!Physics.Linecast(myTransform.position + new Vector3(0, 0.5f, 0), myTransform.position - new Vector3(0, 5000, 0), out hit))
        {
            myTransform.position = new Vector3(myTransform.position.x, hit.point.y, myTransform.position.z);
            JumpState();
        }
        else
        {
            isJump = false;
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

    public void FollowPlayer()
    {
        StopAllCoroutines();
        targetPos = player.position;
        RunState();
    }

    private void Attack()
    {
        StopAllCoroutines();
        AttackState();
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
        isJump = false;
        isWalk = false;
        isRun = false;
        isAttack = false;
        SetAnimation();
    }

    private void WalkState()
    {
        isIdle = false;
        isJump = false;
        isWalk = true;
        isRun = false;
        isAttack = false;
        SetAnimation();
    }

    private void RunState()
    {
        isIdle = false;
        isJump = false;
        isWalk = false;
        isRun = true;
        isAttack = false;
        SetAnimation();
    }

    private void AttackState()
    {
        isIdle = false;
        isJump = false;
        isWalk = false;
        isRun = false;
        isAttack = true;
        SetAnimation();
    }

    private void DyeState()
    {
        isIdle = false;
        isJump = false;
        isWalk = false;
        isRun = false;
        isAttack = false;
        SetAnimation();
        animator.Play("Death");
    }

    private void JumpState()
    {
        rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        isIdle = false;
        isJump = true;
        isWalk = false;
        isRun = false;
        isAttack = false;
        SetAnimation();
    }

    private void SetAnimation()
    {
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isWalk", isWalk);
        animator.SetBool("isRun", isRun);
        animator.SetBool("isJump", isJump);
        animator.SetBool("isAttack", isAttack);
    }
}
