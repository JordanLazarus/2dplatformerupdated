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
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _coyoteTime = 0;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Vector2 _groundCheckSize;

    [SerializeField] private bool _canAttack = true;
    [SerializeField] private bool _canJump = true;
    [SerializeField] private bool _hasHitFloor = true;
    [SerializeField] private bool _isFlipped = false;
    [SerializeField] private bool _isAttack = false;


    private Vector3 _newScale = Vector3.one;
    private Vector2 checkpoint;

    private Rigidbody2D rb;
    private Animator animator;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.instance;


        if (gameManager.lastCheckpoint != Vector3.zero)
            transform.position = gameManager.lastCheckpoint;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        FlipPlayer();
        
        Jump();

        Attack();

        ControlCoyoteTime();

    }

    void FixedUpdate()
    {
        
    }

    //Player movement
    private void Movement()
    {

        float xSpeed = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(xSpeed * _speed, rb.velocity.y);
    }

    //Flip the direction of the player when moving left and right.
    private void FlipPlayer()
    {
        if (rb.velocity.x < 0 && !_isFlipped)
        {
            _isFlipped = true;
            _newScale.x = -1;
        }

        if (rb.velocity.x > 0 && _isFlipped)
        {
            _isFlipped = false;
            _newScale.x = 1;
        }

        transform.localScale = _newScale;
            
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
    }

    //Checks if the player is grounded/on the ground
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(_groundCheck.position, _groundCheckSize);
    }

}
