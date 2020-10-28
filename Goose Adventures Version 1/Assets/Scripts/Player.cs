using UnityEngine;
using System.Collections;
using UnityEngine;
using System.Security.Cryptography;

public class Player : MonoBehaviour
{

    public float speed;
    RigidBody2D rb;
    bool facingRight = true;
    bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatisGround;
    public float jumpForce;
    
    bool isTouchingFront;
    public Transform frontCheck;
    bool wallSliding;
    public float wallSlidingSpeed;

    bool wallJumping;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;
    public Animator anim;


    //double jump
    bool doubleJump = false;

    // Use this for initialization
    void Start()
    {

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        touchingWall = Physics2D.OverlapCircle(wallCheck.position, wallTouchRadius, whatIsWall);
        anim.SetBool("Ground", grounded);

        if (grounded)
        {
            doubleJump = false;
        }

        if (touchingWall)
        {
            grounded = false;
            doubleJump = false;
        }

        anim.SetFloat("vSpeed", rigidbody2D.velocity.y);



        float move = Input.GetAxis("Horizontal");

        anim.SetFloat("Speed", Mathf.Abs(move));

        rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);

        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !facingRight)
        {
            // ... flip the player.
            Flip();
        }// Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && facingRight)
        {
            // ... flip the player.
            Flip();
        }
    }
    void Update()
    {

        // If the jump button is pressed and the player is grounded then the player should jump.
        if ((grounded || !doubleJump) && Input.GetButtonDown("Jump"))
        {
            anim.SetBool("Ground", false);
            rigidbody2D.AddForce(new Vector2(0, jumpForce));

            if (!doubleJump && !grounded)
            {
                doubleJump = true;
            }
        }

        if (touchingWall && Input.GetButtonDown("Jump"))
        {
            WallJump();
        }

    }

    void WallJump()
    {
        rigidbody2D.AddForce(new Vector2(jumpPushForce, jumpForce));
    }


    void Flip()
    {

        // Switch the way the player is labelled as facing
        facingRight = !facingRight;

        //Multiply the player's x local cale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
