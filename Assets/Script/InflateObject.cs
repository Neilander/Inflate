using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public enum InflateDirection { X, Y, XY }

public class InflateObject : MonoBehaviour
{
    public bool inflating;
    public List<HitBox> hitBoxes;
    public float inflateSpeed;
    public InflateDirection inflateDirection;
    public bool positionFixed;
    public bool breakable;
    public bool pushable;
    public bool inflateForever;

    private Rigidbody2D rigidBody;
    private float gravity = 50f;
    private float maxFallingSpeed = 12f;

    private bool pressure;
    private float downForcePosition;
    private float upForcePosition;
    private float leftForcePosition;
    private float rightForcePosition;

    private Vector2 memoryVelocity;
    private bool paused;

    public bool canPushRight;
    public bool canPushLeft;
    public bool heroMessage;
    public bool glueMessage;

    void Start()
    {
        if (!positionFixed)
            rigidBody = GetComponent<Rigidbody2D>();
        if (!pushable)
        {
            canPushLeft = false;
            canPushRight = false;
        }
    }

    void FixedUpdate()
    {
        if (!paused && !GameManager.paused)
        {
            if (!positionFixed)
                Fall();
            if(inflateSpeed > 0 || breakable)
                CheckPressure();
            CheckPush();
            if(pressure && breakable)
            {
                Damage();
            }
            ManagePlayerMessage();
            ManageGlueMessage();
            if (inflating)
            {
                Inflate();
            }
        }
        else if (!paused && GameManager.paused)
        {
            if (!positionFixed)
            {
                memoryVelocity = rigidBody.velocity;
                rigidBody.velocity = new Vector2(0, 0);
            }
            paused = true;
        }
        else if (paused && !GameManager.paused)
        {
            if (!positionFixed)
            {
                rigidBody.velocity = memoryVelocity;
            }
            paused = false;
        }
    }

    private void Fall()
    {
        if (rigidBody.velocity.y + maxFallingSpeed > gravity * Time.fixedDeltaTime)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y - gravity * Time.fixedDeltaTime);
        }
        else if (rigidBody.velocity.y + maxFallingSpeed > 0)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, -maxFallingSpeed);
        }
    }

    private void ManagePlayerMessage()
    {
        if (heroMessage)
        {
            inflating = true;
            heroMessage = false;
        }
        else
        {
            if (!inflateForever)
                inflating = false;
        }
    }

    private void ManageGlueMessage()
    {
        if (glueMessage)
        {
            inflating = false;
            glueMessage = false;
        }
    }

    private void Inflate()
    {
        if (!pressure)
        {
            if (inflateDirection == InflateDirection.X)
                transform.localScale = new Vector3(transform.localScale.x + inflateSpeed * Time.fixedDeltaTime, transform.localScale.y, transform.localScale.z);
            if (inflateDirection == InflateDirection.Y)
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + inflateSpeed * Time.fixedDeltaTime, transform.localScale.z);
            if (inflateDirection == InflateDirection.XY)
                transform.localScale = new Vector3(transform.localScale.x + inflateSpeed * Time.fixedDeltaTime, transform.localScale.y + inflateSpeed * Time.fixedDeltaTime, transform.localScale.z);
            foreach(HitBox hitbox in hitBoxes)
            {
                hitbox.Inflate();
            }
        }
    }

    private void CheckPush()
    {
        if (pushable)
        {
            canPushLeft = true;
            canPushRight = true;
            foreach (HitBox hitBox in hitBoxes)
            {
                if (hitBox.direction == Direction.Left)
                {
                    hitBox.CheckPush();
                    canPushLeft &= hitBox.canPush;
                }
                else if (hitBox.direction == Direction.Right)
                {
                    hitBox.CheckPush();
                    canPushRight &= hitBox.canPush;
                }

            }
        }
    }

    private void CheckPressure()
    {
        leftForcePosition = 1024;
        rightForcePosition = -1024;
        upForcePosition = -1024;
        downForcePosition = 1024;
        foreach (HitBox hitbox in hitBoxes)
        {
            hitbox.CheckPressure();
            if (hitbox.hit)
            {
                if (hitbox.direction == Direction.Up)
                {
                    upForcePosition = Mathf.Max(hitbox.edgePosition, upForcePosition);
                }
                if (hitbox.direction == Direction.Down)
                {
                    downForcePosition = Mathf.Min(hitbox.edgePosition, downForcePosition);
                }
                if (hitbox.direction == Direction.Left)
                {
                    leftForcePosition = Mathf.Min(hitbox.edgePosition, leftForcePosition);
                }
                if (hitbox.direction == Direction.Right)
                {
                    rightForcePosition = Mathf.Max(hitbox.edgePosition, rightForcePosition);
                }
            }
        }
        if (positionFixed)
        {
            if (inflateDirection == InflateDirection.X)
                pressure = leftForcePosition < 0 || rightForcePosition > 0;
            else if (inflateDirection == InflateDirection.Y)
                pressure = downForcePosition < 0 || upForcePosition > 0;
            else if (inflateDirection == InflateDirection.XY)
                pressure = leftForcePosition < 0 || rightForcePosition > 0 || downForcePosition < 0 || upForcePosition > 0;
        }
        else
        {
            if (inflateDirection == InflateDirection.X)
                pressure = leftForcePosition < rightForcePosition;
            else if (inflateDirection == InflateDirection.Y)
                pressure = upForcePosition > downForcePosition;
            else if (inflateDirection == InflateDirection.XY)
                pressure = leftForcePosition < rightForcePosition || upForcePosition > downForcePosition;
        }
    }

    private void Damage()
    {
        Destroy(gameObject);
    }
}
