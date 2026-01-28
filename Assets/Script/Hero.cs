using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.VFX;

public class Hero : MonoBehaviour
{
    public Transform eyes;
    public HitBox hitBoxeUp;
    public HitBox hitBoxeDown;
    public HitBox hitBoxeLeft;
    public HitBox hitBoxeRight;

    private float jumpPreTypeTime;

    private Rigidbody2D rigidBody;
    private Collider2D land;
    private InflateObject landConponent;
    private bool onLand;
    private float wolfJumpTime;
    private Vector2 velocity;
    private float speed = 8f;
    private float landAceleration = 100f;
    private float airAceleration = 30f;
    private float jumpSpeed = 12f;
    private float gravity1 = 35f;
    private float gravity2 = 25f;
    private float gravity3 = 45f;

    private bool pressure;
    private float pressureTime;
    private RaycastHit2D barrier;
    private InflateObject barrierComponent;
    private float distance;

    private Vector2 memoryVelocity;
    private bool paused;

    void Start()
    {
        onLand = false;
        memoryVelocity = new Vector2(0, 0);
        paused = false;
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector2(0, 0);
    }

    void Update()
    {
        if (!paused)
        {
            if (MyInput.jump)
                jumpPreTypeTime = 0.2f;
        }
    }

    void FixedUpdate()
    {
        if (!paused && !GameManager.paused)
        {
            ShowEyes();
            CheckOnLand();
            CheckPressure();
            if (pressureTime > 0.2f)
                Damage();
            if (onLand)
            {
                HorizontalMove(MyInput.x * speed, landAceleration);
                VerticalMove();
            }
            else if (rigidBody.velocity.x * MyInput.x >= 0)
            {
                HorizontalMove(MyInput.x * speed, airAceleration);
                VerticalMove();
            }
            else
            {
                HorizontalMove(MyInput.x * speed, 2 * airAceleration);
                VerticalMove();
            }
        }
        else if (!paused && GameManager.paused)
        {
            memoryVelocity = rigidBody.velocity;
            rigidBody.velocity = new Vector2(0, 0);
            paused = true;
        }
        else if (paused && !GameManager.paused)
        {
            rigidBody.velocity = memoryVelocity;
            paused = false;
        }
    }

    private void ShowEyes()
    {
        eyes.localPosition = new Vector3(velocity.x * 0.01f, velocity.y * 0.01f, 0);
    }

    private void CheckOnLand()
    {
        land = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.8f, 0.02f), 0, MyLayerMask.Up);
        onLand = land != null;
        if (onLand)
        {
            wolfJumpTime = 0.15f;
            landConponent = land.transform.parent.parent.GetComponent<InflateObject>();
            if (landConponent != null)
                landConponent.heroMessage = true;
        }
    }

    private void CheckPressure()
    {
        hitBoxeUp.CheckPressure();
        hitBoxeDown.CheckPressure();
        hitBoxeLeft.CheckPressure();
        hitBoxeRight.CheckPressure();
        pressure = (hitBoxeUp.hit && hitBoxeDown.touch) || (hitBoxeUp.touch && hitBoxeDown.hit) || (hitBoxeLeft.hit && hitBoxeRight.touch) || (hitBoxeLeft.touch && hitBoxeRight.hit);
        if (pressure)
            pressureTime += Time.fixedDeltaTime;
        else
            pressureTime = 0;
    }

    private void HorizontalMove(float speed, float acceleration)
    {
        distance = 2f;
        velocity = rigidBody.velocity;
        if (velocity.x > 0 || (velocity.x == 0 && MyInput.x > 0))
        {
            barrier = Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f), new Vector2(1, 0), 2f, MyLayerMask.Left);
            if (barrier)
            {
                barrierComponent = barrier.transform.GetComponent<InflateObject>();
                if (barrierComponent != null)
                {
                    if (!barrierComponent.canPushRight)
                        distance = Mathf.Min(barrier.distance, distance);
                }
            }
            barrier = Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y), new Vector2(1, 0), 2f, MyLayerMask.Left);
            if (barrier)
            {
                barrierComponent = barrier.transform.GetComponent<InflateObject>();
                if (barrierComponent != null)
                {
                    if (!barrierComponent.canPushRight)
                        distance = Mathf.Min(barrier.distance, distance);
                }
            }
            barrier = Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y - 0.5f), new Vector2(1, 0), 2f, MyLayerMask.Left);
            if (barrier)
            {
                barrierComponent = barrier.transform.GetComponent<InflateObject>();
                if (barrierComponent != null)
                {
                    if (!barrierComponent.canPushRight)
                        distance = Mathf.Min(barrier.distance, distance);
                }
            }
            if (distance < 0.03f)
            {
                velocity.x = 0;
            }
            else if (distance < velocity.x * Time.fixedDeltaTime)
            {
                transform.position = transform.position + new Vector3(distance - 0.03f, 0, 0);
                velocity.x = 0;
            }
            else
            {
                velocity.x = Mathf.MoveTowards(velocity.x, speed, acceleration * Time.fixedDeltaTime);
            }
        }
        else if (velocity.x < 0 || (velocity.x == 0 && MyInput.x < 0))
        {
            barrier = Physics2D.Raycast(new Vector2(transform.position.x - 0.5f, transform.position.y + 0.5f), new Vector2(-1, 0), 2f, MyLayerMask.Right);
            if (barrier)
            {
                barrierComponent = barrier.transform.GetComponent<InflateObject>();
                if (barrierComponent != null)
                {
                    if (!barrierComponent.canPushLeft)
                        distance = Mathf.Min(barrier.distance, distance);
                }
            }
            barrier = Physics2D.Raycast(new Vector2(transform.position.x - 0.5f, transform.position.y), new Vector2(-1, 0), 2f, MyLayerMask.Right);
            if (barrier)
            {
                barrierComponent = barrier.transform.GetComponent<InflateObject>();
                if (barrierComponent != null)
                {
                    if (!barrierComponent.canPushLeft)
                        distance = Mathf.Min(barrier.distance, distance);
                }
            }
            barrier = Physics2D.Raycast(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f), new Vector2(-1, 0), 2f, MyLayerMask.Right);
            if (barrier)
            {
                barrierComponent = barrier.transform.GetComponent<InflateObject>();
                if (barrierComponent != null)
                {
                    if (!barrierComponent.canPushLeft)
                        distance = Mathf.Min(barrier.distance, distance);
                }
            }
            if (distance < 0.03f)
            {
                velocity.x = 0;
            }
            else if (distance < - velocity.x * Time.fixedDeltaTime)
            {
                transform.position = transform.position - new Vector3(distance - 0.03f, 0, 0);
                velocity.x = 0;
            }
            else
            {
                velocity.x = Mathf.MoveTowards(velocity.x, speed, acceleration * Time.fixedDeltaTime);
            }
        }
        rigidBody.velocity = velocity;
    }

    private void VerticalMove()
    {
        velocity = rigidBody.velocity;
        if (wolfJumpTime > 0 && jumpPreTypeTime > 0)
        {
            distance = 2f;
            barrier = Physics2D.Raycast(new Vector2(transform.position.x - 0.5f, transform.position.y + 0.5f), new Vector2(0, 1), 2f, MyLayerMask.Down);
            if (barrier)
            {
                distance = Mathf.Min(barrier.distance, distance);
            }
            barrier = Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f), new Vector2(0, 1), 2f, MyLayerMask.Down);
            if (barrier)
            {
                distance = Mathf.Min(barrier.distance, distance);
            }
            if (distance < 0.04f)
            {
                velocity.y = 0;
            }
            else if (distance > jumpSpeed * Time.fixedDeltaTime)
            {
                velocity.y = jumpSpeed;
            }
            else
            {
                transform.position = transform.position + new Vector3(0, distance - 0.03f, 0);
                velocity.y = 0;
            }
            jumpPreTypeTime = 0;
            wolfJumpTime = 0;
        }
        else
        {
            if (wolfJumpTime > 0)
                wolfJumpTime -= Time.fixedDeltaTime;
            if (jumpPreTypeTime > 0)
                jumpPreTypeTime -= Time.fixedDeltaTime;
            distance = 2f;
            if (velocity.y > 0)
            {
                barrier = Physics2D.Raycast(new Vector2(transform.position.x - 0.5f, transform.position.y + 0.5f), new Vector2(0, 1), 2f, MyLayerMask.Down);
                if (barrier)
                {
                    distance = Mathf.Min(barrier.distance, distance);
                }
                barrier = Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f), new Vector2(0, 1), 2f, MyLayerMask.Down);
                if (barrier)
                {
                    distance = Mathf.Min(barrier.distance, distance);
                }
                if (distance < 0.01f)
                {
                    velocity.y = 0;
                }
                else if (distance > rigidBody.velocity.y * Time.fixedDeltaTime)
                {
                    if (velocity.y > 4f)
                        velocity.y -= gravity1 * Time.fixedDeltaTime;
                    else if (velocity.y > -4f)
                        velocity.y -= gravity2 * Time.fixedDeltaTime;
                    else if (velocity.y > -12f)
                        velocity.y -= gravity3 * Time.fixedDeltaTime;
                }
                else
                {
                    transform.position = transform.position + new Vector3(0, distance - 0.01f, 0);
                    velocity.y = 0;
                }
            }
            else
            {
                barrier = Physics2D.Raycast(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f), new Vector2(0, -1), 2f, MyLayerMask.Up);
                if (barrier)
                {
                    distance = Mathf.Min(barrier.distance, distance);
                }
                barrier = Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y - 0.5f), new Vector2(0, -1), 2f, MyLayerMask.Up);
                if (barrier)
                {
                    distance = Mathf.Min(barrier.distance, distance);
                }
                if (distance > -rigidBody.velocity.y * Time.fixedDeltaTime)
                {
                    if (velocity.y > 4f)
                        velocity.y -= gravity1 * Time.fixedDeltaTime;
                    else if (velocity.y > -4f)
                        velocity.y -= gravity2 * Time.fixedDeltaTime;
                    else if (velocity.y > -12f)
                        velocity.y -= gravity3 * Time.fixedDeltaTime;
                }
                else
                {
                    transform.position = transform.position - new Vector3(0, distance, 0);
                    velocity.y = 0;
                }
            }
        }
        rigidBody.velocity = velocity;
    }

    private void Damage()
    {
        Destroy(gameObject);
    }
}
