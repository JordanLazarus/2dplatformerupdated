using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //Movement
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _coyoteTime = 0;

    //Ground Check
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Vector2 _groundCheckSize;

    //Check whether Player is able to perform a task
    [SerializeField] private bool _canAttack = true;
    [SerializeField] public bool _canJump = true;
    [SerializeField] public bool _hasHitFloor = true;
    [SerializeField] private bool _isFlipped = false;
    [SerializeField] private bool _isAttack = false;

    //Wall Sliding
    [SerializeField] float wallSlideSpeed = 0;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] Vector2 wallCheckSize;
    
    private bool isTouchingWall;
    private bool isWallSliding;

    //Wall Jumping
    [SerializeField] float wallJumpForce;
    [SerializeField] float wallJumpDirection;
    [SerializeField] Vector2 wallJumpAngle;
    public float xWallForce;
    public float yWallForce;


    //Checkpoints
    private Vector3 _newScale = Vector3.one;
    private Vector3 newPosition = Vector3.one;
    private Vector2 checkpoint;

    private Rigidbody2D rb;
    public Animator animator;
    private GameManager gameManager;

    public bool canMove;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;
        canMove = true;

        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.instance;


        if (gameManager.lastCheckpoint != Vector3.zero)
            transform.position = gameManager.lastCheckpoint;


        wallJumpAngle.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        float xSpeed = Input.GetAxisRaw("Horizontal");

        canMove = true;

        animator.SetFloat("Speed", Mathf.Abs(xSpeed));

        Movement();

        FlipPlayer();
        
        Jump();

        Attack();

        ControlCoyoteTime();


        isTouchingWall = Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0, wallLayer);

        WallSlide();

        WallJump();
    }

    //Player movement
    private void Movement()
    {
    
        float xSpeed = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(xSpeed * _speed, rb.velocity.y);

        canMove = true;
    }

    //Flip the direction of the player when moving left and right.
    private void FlipPlayer()
    {
        if (rb.velocity.x < 0 && !_isFlipped)
        {
            _isFlipped = true;
            _newScale.x = -1;
            wallJumpDirection = 1;
        }

        if (rb.velocity.x > 0 && _isFlipped)
        {
            _isFlipped = false;
            _newScale.x = 1;
            wallJumpDirection *= -1;
        }

        transform.localScale = _newScale;
        canMove = true;
    }

    //Create a Coyote Jump (Player can jump just after walking off platform)
    private void ControlCoyoteTime()
    {
        if (IsGrounded())
        {
            if (_hasHitFloor)
            {
                _canJump = true;
                _hasHitFloor = false;
            }
            _coyoteTime = 0;
        }
        
        else if (!IsGrounded())
        {
            _coyoteTime += 5f * Time.deltaTime;
        }

        if (_coyoteTime >= 1f)
        {
            _canJump = false;
        }
    }

    //Allow the player to jump
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _canJump)
        {
            rb.gravityScale = 4;
            rb.velocity = Vector2.up * _jumpForce;
            _canJump = false;
        }
        else if (!Input.GetKey(KeyCode.Space) || rb.velocity.y < 0)
        {
            rb.gravityScale = 4;
            _hasHitFloor = true;
        }

        canMove = true;
    }

    
    void WallSlide()
    {
        if (isTouchingWall && !IsGrounded() && rb.velocity.y < 0 && rb.velocity.x != 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, wallSlideSpeed);
            _canJump = true;
        }
    }

    //Let's Player Wall Jump
    void WallJump()
    {
        if (isWallSliding && (Input.GetKeyDown(KeyCode.Space)))
        {
            rb.AddForce(new Vector2(wallJumpForce * wallJumpDirection * wallJumpAngle.x,
                wallJumpForce * wallJumpAngle.y), ForceMode2D.Impulse);
            
            _canJump = false;
        }
    }

    //Check if the player is grounded/touching the ground
    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(_groundCheck.position, _groundCheckSize, 0, LayerMask.GetMask("Ground"));
    }

    //Let the player attack the enemies
    private void Attack()
    {
        if (_canAttack && Input.GetKeyDown(KeyCode.J))
        {
            //animator.SetTrigger("Attack");
            _canAttack = false;
            //_isAttacking = true;
        }
    }
    
    //Reset the player's attack
    public void ResetAttack()
    {
        _canAttack = true;
        //_isAttacking = false;
    }

    //Create a Checkpoint function so the player respawns at the checkpoint
    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        checkpoint = newCheckpoint;
    }

    //If the player falls off, runs into an enemy or runs out of time the player will die
    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Give collision to the player's attack
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
            gameManager.SetLastCheckpoint(other.transform.position);

        if (other.CompareTag("DeathCollision"))
            Die();

        if (other.CompareTag("EndLevel"))
            transform.position = Vector3.zero;

    }

    //Checks if the player is grounded/on the ground
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(_groundCheck.position, _groundCheckSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPoint.position, wallCheckSize);
    }

}
