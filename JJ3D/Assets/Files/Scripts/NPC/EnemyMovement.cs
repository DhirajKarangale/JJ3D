using UnityEngine;

public class EnemyMovement : NPC
{
    [SerializeField] protected float attackDist;
    [SerializeField] float followDist;

    protected GameManager gameManager;
    protected PlayerHealth playerHealth;
    protected Transform player;
    protected float playerDist;
    private bool isHurt;

    protected override void Start()
    {
        gameManager = GameManager.instance;
        playerHealth = gameManager.player.playerHealth;
        player = playerHealth.transform;
        targetPos = player.position;
        isHurt = false;
        base.Start();
    }

    protected override void Update()
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
            if (TimeController.isDay)
            {
                if (!isAttack && playerDist < attackDist) Attack();
                else if (isAttack && playerDist > (attackDist + 1.7f) && playerDist < followDist) FollowPlayer();
                // else if (isAttack && playerDist > (attackDist) && playerDist < followDist) FollowPlayer();
                else if (!isAttack && ((playerDist < followDist) || isHurt)) FollowPlayer();
                else if (!isAttack && playerDist > followDist && !isWalk && !isIdle) StartCoroutine(IEWalkIdle());
            }
            else
            {
                if (isAttack && playerDist > (attackDist + 1.7f) && playerDist < (3 * followDist)) FollowPlayer();
                else if (!isAttack && ((playerDist < (6 * followDist)) || isHurt)) FollowPlayer();
            }
        }
        base.Update();
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

    internal override void Hurt()
    {
        base.Hurt();
        isHurt = true;
    }

    private void Attack()
    {
        if (isDye) return;
        StopAllCoroutines();
        AttackState();
    }

    public void FollowPlayer()
    {
        StopAllCoroutines();
        targetPos = player.position;
        RunState();

        if (isHurt && !IsInvoking("DesableHurt") && playerDist > followDist && playerDist < 3 * followDist)
        {
            Invoke("DesableHurt", 20);
        }
    }

    protected override void IdleState()
    {
        base.IdleState();

        isHurt = false;
    }

    protected virtual void AttackState()
    {
        isHurt = false;

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

    private void DesableHurt()
    {
        isHurt = false;
    }
}