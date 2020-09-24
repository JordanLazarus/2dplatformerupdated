using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _enemySpeed;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Vector2 _groundCheckSize;
    [SerializeField] private bool _canMove;
    [SerializeField] private bool _isFlipped;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_canMove)
            transform.position += Vector3.right * _enemySpeed * Time.deltaTime;
        
        DetectFloor();
    }

    private void DetectFloor()
    {
        if (!GroundCheck() && _canMove)
        {
            _canMove = false;
            StartCoroutine(ChangeTarget());
        }
    }

    //Check if the Enemy is grounded/touching the ground
    private bool GroundCheck()
    {
        return Physics2D.OverlapBox(_groundCheck.position, _groundCheckSize, 0, LayerMask.GetMask("Ground"));
    }

    private IEnumerator ChangeTarget()
    {
        _canMove = false;

        

        _isFlipped = !_isFlipped;

        yield return new WaitForSeconds(0f);
       
        if (_isFlipped)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            _enemySpeed *= -1;
        }

        else if (!_isFlipped)
        {
            transform.localScale = new Vector3(1, 1, 1);
            _enemySpeed *= -1;
        }

        _canMove = true;

    }

    public void Damage()
    {
        Destroy(gameObject);
    }

    //Give collision to the player's attack
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<Player>().Die();

        if (other.CompareTag("MeleeAttack"))
            Damage();
    }

    //Checks if the Enemy is grounded/on the ground
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(_groundCheck.position, _groundCheckSize);
    }
}
