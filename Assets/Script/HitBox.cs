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
    public bool hit;
    public bool touch;
    public bool canPush;
    public Direction direction;
    public float lenth;
    public float edgePosition;
    private float pressureDepth;
    private int targetLayerMask;
    private SpriteRenderer spriteRenderer;
    private Collider2D hitCollider;
    private Collider2D[] hitColliders;
    private InflateObject colliderComponent;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (direction == Direction.Left)
        {
            lenth = transform.localScale.y;
            gameObject.layer = MyLayer.Left;
            targetLayerMask = MyLayerMask.Right;
            edgePosition = transform.position.x - transform.parent.parent.position.x;
        }
        else if (direction == Direction.Right)
        {
            lenth = transform.localScale.y;
            gameObject.layer = MyLayer.Right;
            targetLayerMask = MyLayerMask.Left;
            edgePosition = transform.position.x - transform.parent.parent.position.x;
        }
        else if (direction == Direction.Up)
        {
            lenth = transform.localScale.x;
            gameObject.layer = MyLayer.Up;
            targetLayerMask = MyLayerMask.Down;
            edgePosition = transform.position.y - transform.parent.parent.position.y;
        }
        else if (direction == Direction.Down)
        {
            lenth = transform.localScale.x;
            gameObject.layer = MyLayer.Down;
            targetLayerMask = MyLayerMask.Up;
            edgePosition = transform.position.y - transform.parent.parent.position.y;
        }
        pressureDepth = 0.06f;
    }

    public void Inflate()
    {
        if (direction == Direction.Left || direction == Direction.Right)
        {
            transform.localScale = new Vector3(0.4f / transform.parent.lossyScale.x,lenth - 0.1f / transform.parent.lossyScale.y, 1);
        }
        if (direction == Direction.Up || direction == Direction.Down)
        {
            transform.localScale = new Vector3(lenth - 0.1f / transform.parent.lossyScale.x, 0.4f / transform.parent.lossyScale.y, 1);
        }
    }

    public void ManageColor(Color inflateColor, Color edgeColor, Color inflateEdgeColor, InflateDirection inflateDirection, bool inflating)
    {
        if (((inflateDirection == InflateDirection.X || inflateDirection == InflateDirection.XY) && (direction == Direction.Left || direction == Direction.Right)) || ((inflateDirection == InflateDirection.Y || inflateDirection == InflateDirection.XY) && (direction == Direction.Up || direction == Direction.Down)))
        {
            if (inflating)
                spriteRenderer.color = inflateColor;
            else
                spriteRenderer.color = inflateEdgeColor;
        }
        else
        {
            spriteRenderer.color = edgeColor;
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
                colliderComponent = collider.transform.parent.parent.GetComponent<InflateObject>();
                if (colliderComponent != null)
                    canPush &= colliderComponent.canPushLeft;
            }
        }
        else if (direction == Direction.Right)
        {
            hitColliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x - 0.2f, transform.position.y), new Vector2(0.41f, transform.lossyScale.y - 0.1f), 0, targetLayerMask);
            foreach (Collider2D collider in hitColliders)
            {
                colliderComponent = collider.transform.parent.parent.GetComponent<InflateObject>();
                if (colliderComponent != null)
                    canPush &= colliderComponent.canPushRight;
            }
        }
    }
}
