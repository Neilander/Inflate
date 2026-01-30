using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public enum InflateDirection {X, Y, XY, O}

public class InflateObject : MonoBehaviour
{
    public int colorNumber;
    public bool inflating;
    public List<HitBox> hitBoxes;
    public float inflateSpeed;
    public InflateDirection inflateDirection;
    public bool positionFixed;
    public bool glue;
    public bool breakable;
    public bool pushable;
    public bool inflateForever;

    private Rigidbody2D rigidBody;
    private float gravity = 50f;
    private float maxFallingSpeed = 12f;

    private bool cannotInflate;
    private bool pressure;
    private float pressureTime;
    private float downForcePosition;
    private float upForcePosition;
    private float leftForcePosition;
    private float rightForcePosition;
    private float downTouchPosition;
    private float upTouchPosition;
    private float leftTouchPosition;
    private float rightTouchPosition;

    private Vector2 memoryVelocity;
    private bool paused;

    public Transform fixedPosition;
    private bool hasFixedPosition;
    public bool canPushRight;
    public bool canPushLeft;
    public bool heroMessage;
    public bool glueMessage;
    public SpriteRenderer symbol;
    private bool hasSymbol;
    private float symbolScale;

    void Start()
    {
        if (symbol == null)
        {
            hasSymbol = false;
        }
        else
        {
            symbolScale = symbol.transform.lossyScale.x;
            hasSymbol = true;
            if (breakable)
                symbol.sprite = SpriteManager.Instance.breakable;
            else if (glue)
                symbol.sprite = SpriteManager.Instance.glue;
            else if (positionFixed)
                symbol.sprite = SpriteManager.Instance.positionFixed;
            else if (!pushable)
                symbol.sprite = SpriteManager.Instance.banPush;
            else
            {
                Destroy(symbol.gameObject);
                hasSymbol = false;
            }
        }
        ManageSymbol();
        if (!positionFixed)
            rigidBody = GetComponent<Rigidbody2D>();
        if (!pushable)
        {
            canPushLeft = false;
            canPushRight = false;
        }
        hasFixedPosition = fixedPosition != null;
        foreach (HitBox hitbox in hitBoxes)
        {
            hitbox.Inflate();
            hitbox.ManageColor(colorNumber, inflateDirection, inflating);
            hitbox.InitializeColor(colorNumber);
        }
    }

    void FixedUpdate()
    {
        if (!paused && !GameManager.paused)
        {
            if (!positionFixed)
                Fall();
            else if (hasFixedPosition)
                transform.position = fixedPosition.position;
            if(inflateDirection != InflateDirection.O || breakable)
                CheckPressure();
            CheckPush();
            if(pressureTime > 0.2f && breakable)
            {
                Damage();
            }
            ManagePlayerMessage();
            ManageGlueMessage();
            if (inflating)
            {
                Inflate();
                ManageSymbol();
            }
            CheckDestroy();
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

    private void ManageSymbol()
    {
        if (hasSymbol)
        {
            symbol.transform.localScale = new Vector3(symbolScale / transform.localScale.x, symbolScale / transform.localScale.y, 1);
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
        if (inflateDirection != InflateDirection.O && !cannotInflate)
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
                    if (!hitBox.canPush)
                    {
                        rigidBody.velocity = new Vector2(0,rigidBody.velocity.y);
                        canPushLeft = false;
                    }
                }
                else if (hitBox.direction == Direction.Right)
                {
                    hitBox.CheckPush();
                    if (!hitBox.canPush)
                    {
                        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                        canPushRight = false;
                    }
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
        leftTouchPosition = 1024;
        rightTouchPosition = -1024;
        upTouchPosition = -1024;
        downTouchPosition = 1024;
        foreach (HitBox hitbox in hitBoxes)
        {
            hitbox.ManageColor(colorNumber,inflateDirection,inflating);
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
            if (hitbox.touch)
            {
                if (hitbox.direction == Direction.Up)
                {
                    upTouchPosition = Mathf.Max(hitbox.edgePosition, upForcePosition);
                }
                if (hitbox.direction == Direction.Down)
                {
                    downTouchPosition = Mathf.Min(hitbox.edgePosition, downForcePosition);
                }
                if (hitbox.direction == Direction.Left)
                {
                    leftTouchPosition = Mathf.Min(hitbox.edgePosition, leftForcePosition);
                }
                if (hitbox.direction == Direction.Right)
                {
                    rightTouchPosition = Mathf.Max(hitbox.edgePosition, rightForcePosition);
                }
            }
        }
        if (positionFixed)
        {
            if (inflateDirection == InflateDirection.X)
                cannotInflate = leftForcePosition < 0 || rightForcePosition > 0;
            else if (inflateDirection == InflateDirection.Y)
                cannotInflate = downForcePosition < 0 || upForcePosition > 0;
            else if (inflateDirection == InflateDirection.XY)
                cannotInflate = leftForcePosition < 0 || rightForcePosition > 0 || downForcePosition < 0 || upForcePosition > 0;
            pressure = leftForcePosition < 1000 || rightForcePosition > -1000 || downForcePosition < 1000 || upForcePosition > -1000;
        }
        else
        {
            if (inflateDirection == InflateDirection.X)
                cannotInflate = leftForcePosition < rightForcePosition;
            else if (inflateDirection == InflateDirection.Y)
                cannotInflate = upForcePosition > downForcePosition;
            else if (inflateDirection == InflateDirection.XY)
                cannotInflate = leftForcePosition < rightForcePosition || upForcePosition > downForcePosition;
            pressure = (leftForcePosition < 1000 && rightTouchPosition > -1000) || (leftTouchPosition < 1000 && rightForcePosition > -1000) || (downTouchPosition < 1000 && upForcePosition > -1000) || (downForcePosition < 1000 && upTouchPosition > -1000);
        }
        if (pressure)
            pressureTime += Time.fixedDeltaTime;
        else
            pressureTime = 0;
    }

    private void Damage()
    {
        Destroy(gameObject);
    }

    private void CheckDestroy()
    {
        if (transform.position.y < CameraManager.down - 15f)
            Destroy(gameObject);
    }
}
