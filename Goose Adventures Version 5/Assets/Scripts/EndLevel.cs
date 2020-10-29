using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    public GameManager gameManager;
    public Player player;
    private Vector3 newPosition = Vector3.one;

    void OnTriggerEnter2D(Collider2D other)
    {
        gameManager.CompleteLevel();

        if (other.CompareTag("EndLevel"))
            player.transform.position = Vector3.zero;

            player.canMove = false;

    }

}
