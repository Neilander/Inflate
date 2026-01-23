using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Princess : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Collider2D land;
    private Vector2 landVelocity;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!MyInput.paused)
        {
            Move();
        }
    }

    private void TestLand()
    {
        land = Physics2D.OverlapBox(transform.position, new Vector2(0.2f, 0.02f), 0, 1 << 7);
    }

    private void Move()
    {
        
    }
}
