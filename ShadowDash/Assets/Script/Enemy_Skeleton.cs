using UnityEngine;

public class Enemy_Skeleton : Entity
{
    private bool isAttacking;
    [Header("Move Info")]
    [SerializeField] private float moveSpeed;

    [Header("Player detection")]
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private LayerMask whatIsPlayer;

    private RaycastHit2D isPlayerDeteected;


    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (isPlayerDeteected)
        {
            if (isPlayerDeteected.distance > 1)
            {
                //추적
                rb.linearVelocity = new Vector2(moveSpeed * 1.5f * facingDir, rb.linearVelocityY);
                isAttacking = false;
                Debug.Log("플레이러를 봤다.");
            }
            else
            {
                Debug.Log("공격! " + isPlayerDeteected.collider.gameObject.name);
                isAttacking = true;
            }
        }
        else 
        {
            Movement();
        }

        if (!isGrounded || isWallDetected)
            Flip();
    }

    private void Movement()
    {
        if(!isAttacking)
            rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocity.y);
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();
        isPlayerDeteected = Physics2D.Raycast(transform.position, Vector2.right, playerCheckDistance * facingDir, whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + playerCheckDistance * facingDir, transform.position.y));

    }

}
