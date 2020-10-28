using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPlatform : MonoBehaviour
{
    Rigidbody2D rb;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name.Equals("Player"))
        {
            Invoke ("FallingPlatform", 0.2f);
            Destroy(gameObject, 1f);
        }
    }

    void FallingPlatform()
    {
        rb.isKinematic = false;
    }
}
