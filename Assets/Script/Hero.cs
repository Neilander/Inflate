using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public float speed;
    public float landAcceleration;
    public float jumpSpeed;
    public float jumpAcceleration;
    public float gravity1;
    public float gravity2;
    public float gravity3;
    public float maxFallingSpeed;

    public static Collider2D touched;

    private Rigidbody2D rigidBody;
    private bool onLand;
    private float jumpPreInput;
    private Vector2 velocityValue;
    public bool paused;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        paused = false;
    }

    void Update()
    {
        ManageInput();
    }

    void FixedUpdate()
    {
        if (!paused && !MyInput.paused)
        {
            TestOnLand();
            Move();
        }
        if (!paused && MyInput.paused)
        {
            Pause();
        }
        if (paused && !MyInput.paused)
        {
            NotPause();
        }
    }


    private void TestOnLand()
    {
        touched = Physics2D.OverlapBox(transform.position, new Vector2(0.2f, 0.02f), 0, 1 << 7);
        onLand = touched != null;
    }

    private void ManageInput()
    {
        if (MyInput.jump)
            jumpPreInput = 0.15f;
        if (jumpPreInput > 0 && !onLand)
        {
            jumpPreInput -= Time.deltaTime;
        }
        else if(jumpPreInput > 0 && onLand)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x , jumpSpeed);
            jumpPreInput = 0;
        }
    }

    private void Move()
    {
        if (onLand)
        {
            HorizontalMove(speed * MyInput.x, landAcceleration);
        }
        else
        {
            HorizontalMove(speed * MyInput.x, jumpAcceleration);
            if(rigidBody.velocity.y > 3)
                rigidBody.velocity = rigidBody.velocity - new Vector2(0, gravity1*Time.fixedDeltaTime);
            else if (rigidBody.velocity.y > -3)
                rigidBody.velocity = rigidBody.velocity - new Vector2(0, gravity2 * Time.fixedDeltaTime);
            else if (rigidBody.velocity.y > - maxFallingSpeed)
                rigidBody.velocity = rigidBody.velocity - new Vector2(0, gravity3 * Time.fixedDeltaTime);
        }
    }

    public void HorizontalMove(float speed, float acceleration)
    {
        if (rigidBody.velocity.x > speed)
        {
            rigidBody.velocity = rigidBody.velocity - new Vector2(acceleration * Time.fixedDeltaTime, 0);
            if (rigidBody.velocity.x < speed)
            {
                rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);
            }
        }
        else if (rigidBody.velocity.x < speed)
        {
            rigidBody.velocity = rigidBody.velocity + new Vector2(acceleration * Time.fixedDeltaTime, 0);
            if (rigidBody.velocity.x > speed)
            {
                rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);
            }
        }
    }

    public void Pause()
    {
        velocityValue = rigidBody.velocity;
        rigidBody.velocity = new Vector2(0, 0);
        paused = true;
    }

    public void NotPause()
    {
        rigidBody.velocity = velocityValue;
        paused = false;
    }
}
