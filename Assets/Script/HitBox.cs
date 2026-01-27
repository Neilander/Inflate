using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Direction { Up, Down, Left, Right };

public struct MyLayerMask
{
    public static int Left = 1 << 7;
    public static int Right = 1 << 8;
    public static int Up = 1 << 9;
    public static int Down = 1 << 10;
}

public struct MyLayer
{
    public static int Left = 7;
    public static int Right = 8;
    public static int Up = 9;
    public static int Down = 10;
}

public class HitBox : MonoBehaviour
{
    public bool weak;
    public bool hit;
    public bool touch;
    public bool canPush;
    public Direction direction;
    public float edgePosition;
    private float depth;
    private float pressureDepth;
    private int targetLayerMask;
    private Collider2D hitCollider;
    private Collider2D[] hitColliders;
    private InflateObject colliderComponent;

    void Start()
    {
        if (direction == Direction.Left)
        {
            gameObject.layer = MyLayer.Left;
            depth = 0.4f / transform.parent.localScale.x;
            targetLayerMask = MyLayerMask.Right;
            edgePosition = transform.localPosition.x;
        }
        else if (direction == Direction.Right)
        {
            gameObject.layer = MyLayer.Right;
            depth = 0.4f / transform.parent.localScale.x;
            targetLayerMask = MyLayerMask.Left;
            edgePosition = transform.localPosition.x;
        }
        else if (direction == Direction.Up)
        {
            gameObject.layer = MyLayer.Up;
            depth = 0.4f / transform.parent.localScale.y;
            targetLayerMask = MyLayerMask.Down;
            edgePosition = transform.localPosition.y;
        }
        else if (direction == Direction.Down)
        {
            gameObject.layer = MyLayer.Down;
            depth = 0.4f / transform.parent.localScale.y;
            targetLayerMask = MyLayerMask.Up;
            edgePosition = transform.localPosition.y;
        }
        if (weak)
            pressureDepth = 0.05f;
        else
            pressureDepth = 0.1f;
    }

    public void Inflate()
    {
        if (direction == Direction.Left || direction == Direction.Right)
        {
            transform.localScale = new Vector3(depth / transform.parent.localScale.x, transform.localScale.y, 1);
        }
        if (direction == Direction.Up || direction == Direction.Down)
        {
            transform.localScale = new Vector3(transform.localScale.x, depth / transform.parent.localScale.y, 1);
        }
    }

    public void CheckPressure()
    {
        if (direction == Direction.Left)
            hitCollider = Physics2D.OverlapBox(new Vector2(transform.position.x + 0.2f, transform.position.y), new Vector2(0.4f, transform.lossyScale.y), 0, targetLayerMask);
        else if (direction == Direction.Right)
            hitCollider = Physics2D.OverlapBox(new Vector2(transform.position.x - 0.2f, transform.position.y), new Vector2(0.4f, transform.lossyScale.y), 0, targetLayerMask);
        else if (direction == Direction.Up)
            hitCollider = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 0.2f), new Vector2(transform.lossyScale.x, 0.4f), 0, targetLayerMask);
        else if (direction == Direction.Down)
            hitCollider = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y + 0.2f), new Vector2(transform.lossyScale.x, 0.4f), 0, targetLayerMask);
        touch = hitCollider != null;
        if (direction == Direction.Left)
            hitCollider = Physics2D.OverlapBox(new Vector2(transform.position.x + 0.2f, transform.position.y), new Vector2(0.4f - pressureDepth, transform.lossyScale.y - pressureDepth), 0, targetLayerMask);
        else if (direction == Direction.Right)
            hitCollider = Physics2D.OverlapBox(new Vector2(transform.position.x - 0.2f, transform.position.y), new Vector2(0.4f - pressureDepth, transform.lossyScale.y - pressureDepth), 0, targetLayerMask);
        else if (direction == Direction.Up)
            hitCollider = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 0.2f), new Vector2(transform.lossyScale.x - pressureDepth, 0.4f - pressureDepth), 0, targetLayerMask);
        else if (direction == Direction.Down)
            hitCollider = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y + 0.2f), new Vector2(transform.lossyScale.x - pressureDepth, 0.4f - pressureDepth), 0, targetLayerMask);
        hit = hitCollider != null;
    }

    public void CheckPush()
    {
        canPush = true;
        if (direction == Direction.Left)
        {
            hitColliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + 0.2f, transform.position.y), new Vector2(0.41f, transform.lossyScale.y - 0.1f), 0, targetLayerMask);
            foreach (Collider2D collider in hitColliders)
            {
                colliderComponent = collider.transform.parent.GetComponent<InflateObject>();
                if (colliderComponent != null)
                    canPush &= colliderComponent.canPushLeft;
            }
        }
        else if (direction == Direction.Right)
        {
            hitColliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x - 0.2f, transform.position.y), new Vector2(0.41f, transform.lossyScale.y - 0.1f), 0, targetLayerMask);
            foreach (Collider2D collider in hitColliders)
            {
                colliderComponent = collider.transform.parent.GetComponent<InflateObject>();
                if (colliderComponent != null)
                    canPush &= colliderComponent.canPushRight;
            }
        }
    }
}
