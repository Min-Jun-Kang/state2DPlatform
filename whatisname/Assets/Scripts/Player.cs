using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isBusy { get; private set; }
    [Header("이동 정보")]
    public float moveSpeed = 12f;
    public float jumpForce;

    [Header("공격 디테일")]
    public Vector2[] attackMovement;

    [Header("대시 정보")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }


    [Header("플레이어 속도 정보")]
    public float VelocityX;
    public float VelocityY;

    [Header("충돌 정보")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    public int facingDir { get; private set; } = 1; //기본 값도 설정 가능
    private bool facingRight = true;


    #region Component
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    #region States
    // 플레이어의 상태를 관리하는 상태 머신
    public PlayerStateMachine stateMachine { get; private set; }

    // 플레이어의 상태 (대기 상태, 이동 상태)
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveStatee { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerGroudedState groudedState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallGripState wallgripState { get; private set; }
    public PlayerWallSlideState wallslideState { get; private set; }
    public PlayerWallJumpState walljumpState { get; private set; }

    public PlayerPrimaryAttackState primaryaAttack { get; private set; }
    #endregion

    private void Awake()
    {
        // 상태 머신 인스턴스 생성
        stateMachine = new PlayerStateMachine();

        // 각 상태 인스턴스 생성 (this: 플레이어 객체, stateMachine: 상태 머신, "Idle"/"Move": 상태 이름)
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveStatee = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallgripState = new PlayerWallGripState(this, stateMachine, "Grip");
        wallslideState = new PlayerWallSlideState(this, stateMachine, "Slide");
        walljumpState = new PlayerWallJumpState(this, stateMachine, "Jump");

        primaryaAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");

    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // 게임 시작 시 초기 상태를 대기 상태(idleState)로 설정
        stateMachine.Initialize(idleState);

    }

    private void Update()
    {
        VelocityX = rb.linearVelocityX;
        VelocityY = rb.linearVelocityY;

        stateMachine.currentState.Update();
        FlipController();
        CheckForDashInput();

    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        Debug.Log("바쁨");
        yield return new WaitForSeconds(_seconds);
        Debug.Log("안 바쁨");
        isBusy = false;
    }



    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public void ZeroVelocity() => rb.linearVelocity = new Vector2(0, 0);

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
    }

    private void CheckForDashInput()
    {
        dashUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer<0)
        {
            dashUsageTimer = dashCooldown;

            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
            
    }

    #region 충돌
    //땅체크
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround); 

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
    #endregion

    public void Flip() //좌우 반전
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0); //y축 방향으로 180도 회전
        //Debug.Log("좌우 반전");
    }

    public void FlipController()
    {
        if (rb.linearVelocityX > 0 && !facingRight) //오른쪽 방향으로 가려고함 && 왼쪽보고 있다.
            Flip();
        else if (rb.linearVelocityX < 0 && facingRight) //왼쪽 방향으로 가려고함 && 오른쪽보고 있다.
            Flip();
    }

}
