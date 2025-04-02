using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float moveSpeed;

    private float xInput;
    [SerializeField]
    private bool isMoving;
    private int facingDir = 1;
    private bool facingRight = true;

    [Header("Collision Info")]
    
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>(); //자식한테 있는 컴포넌트 가져올 때 사용
        //rb.linearVelocity = new Vector2(5, rb.linearVelocity.y); //움직임을 표현, y값은 고정하고 x값만 움직임
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Movement()
    {
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
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
        anim.SetBool("isMoving", isMoving);

    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;

        transform.Rotate(0, 180, 0); //보는 방향 뒤집기
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }

    void Update()
    {
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
        CollisionCheck();
        FlipController();
        AnimatorControllers();
    }

    private void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        //Debug.Log(isGrounded);
    }
}
