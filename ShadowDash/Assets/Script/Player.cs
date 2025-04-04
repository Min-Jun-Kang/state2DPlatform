using UnityEngine;

public class Player : Entity
{

    [Header("Move Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isMoving;
    

    [Header("Dash Info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    private float dashTime;
    [SerializeField] private float dashCooldown;
    private float dashCooldownTimer;


    [Header("Attack Info")]
    [SerializeField]private float comboTime = 0.3f;
    private float comboTimeCounter;
    private bool isAttacking;
    private int comboCounter;

    private float xInput;


    protected override void Start()
    {
        base.Start();
        //rb = GetComponent<Rigidbody2D>();
        //anim = GetComponentInChildren<Animator>(); //자식한테 있는 컴포넌트 가져올 때 사용
        ////rb.linearVelocity = new Vector2(5, rb.linearVelocity.y); //움직임을 표현, y값은 고정하고 x값만 움직임
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashAbility();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartAttackEvent();
        }
    }

    private void StartAttackEvent()
    {
        if (!isGrounded)
            return;

        if (comboTimeCounter < 0)
            comboCounter = 0;

        isAttacking = true;
        comboTimeCounter = comboTime;
    }

    public void AttackOver()
    {
        isAttacking = false;
        comboCounter++;

        if (comboCounter > 2)
            comboCounter = 0;
    }


    private void Movement()
    {

        if (isAttacking)
        {
            rb.linearVelocity = new Vector2(0, 0);
        }
        else if (dashTime > 0)
        {
            rb.linearVelocity = new Vector2(facingDir * dashSpeed, rb.linearVelocity.y);
        }
        else 
        {
            rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
        }
            
    }

    private void Jump()
    {
        if(isGrounded)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }


    //Alt + 화살표키
    private void AnimatorControllers()
    {

        isMoving = rb.linearVelocity.x != 0;

        anim.SetFloat("yVelocity", rb.linearVelocityY);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", dashTime > 0);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCounter", comboCounter);

    }

    
    private void DashAbility()
    {

        if (dashCooldownTimer < 0 && !isAttacking)
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }

    private void FlipController()
    {
        if (rb.linearVelocityX > 0 && !facingRight)
        {
            Flip();
        }
        else if (rb.linearVelocityX < 0 && facingRight)
        {
            Flip();
        }
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();
    }


    protected override void Update()
    {
        base.Update();

        //if (Input.GetKeyDown(KeyCode.Space))
        //    Debug.Log("Jump!!");
        //if (Input.GetKey(KeyCode.Alpha1))
        //    Debug.Log("키 눌리고있습니다.");
        //if (Input.GetKeyUp(KeyCode.Alpha2))
        //    Debug.Log("키를 눌렀다 땝니다.");

        //if (Input.GetButtonDown("Jump")) //Edit -> Project Settings에서 이름을 정의해서 사용하거나 기존에 있던 것을 이용할 수 있음
        //    Debug.Log("버튼 눌렀습니다.");

        //Debug.Log(Input.GetAxis("Horizontal")); // 방향키 양쪽 키 누르면 -1 ~ 1 사이의 값을 구한다. (A,D로도 가능함)

        //Debug.Log(Input.GetAxisRaw("Horizontal")); // 방향키 양쪽 키 누르면 -1 ~ 1 사이의 값을 구한다.(정수 값만)

        //xInput = Input.GetAxisRaw("Horizontal");

        //rb.linearVelocity = new Vector2(xInput*speed, rb.linearVelocityY); //Y값은 고정, X값은 방향키 왼쪽 오른쪽에 따라 바뀜

        //if (Input.GetKeyDown(KeyCode.Space))
        //    rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);

        //Ani.SetBool("IsMoving", isMoving);
        CheckInput();
        Movement();

        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        comboTimeCounter -= Time.deltaTime;


        FlipController();
        AnimatorControllers();
    }

    

}
