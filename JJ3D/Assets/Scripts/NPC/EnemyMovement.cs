using UnityEngine;

public class EnemyMovement : NPC
{
    [SerializeField] protected float attackDist;
    [SerializeField] float followDist;

    protected GameManager gameManager;
    protected PlayerHealth playerHealth;
    protected Transform player;
    protected float playerDist;

    protected override void Start()
    {
        gameManager = GameManager.instance;
        playerHealth = gameManager.playerHealth;
        player = playerHealth.transform;
        targetPos = player.position;
        base.Start();
    }

    protected override void FixedUpdate()
    {
        if (isDye) return;

        playerDist = Mathf.Abs(Vector3.Distance(myTransform.position, player.position));

        if (gameManager.isGameOver)
        {
            if (!isIdle && !isWalk)
            {
                StartCoroutine(IEWalkIdle());
            }
        }
        else
        {
            if (!isAttack && playerDist < attackDist) Attack();
            else if (isAttack && playerDist > (attackDist + 1.7f) && playerDist < followDist) FollowPlayer();
            // else if (isAttack && playerDist > (attackDist + 3f) && playerDist < followDist) FollowPlayer();
            else if (!isAttack && playerDist < followDist) FollowPlayer();
            else if (!isAttack && playerDist > followDist && !isWalk && !isIdle) StartCoroutine(IEWalkIdle());
        }
        base.FixedUpdate();
    }

    protected override void Move(float speed)
    {
        base.Move(speed);
        myTransform.position += myTransform.forward * speed * Time.deltaTime;
    }

    protected override void Look()
    {
        if (isAttack) targetPos = player.position;
        base.Look();
    }

    private void Attack()
    {
        StopAllCoroutines();
        AttackState();
    }

    public void FollowPlayer()
    {
        StopAllCoroutines();
        targetPos = player.position;
        RunState();
    }

    protected virtual void AttackState()
    {
        isIdle = false;
        isWalk = false;
        isRun = false;
        isAttack = true;
        SetAnimation();
    }

    protected override void SetAnimation()
    {
        base.SetAnimation();
        animator.SetBool("isAttack", isAttack);
    }
}