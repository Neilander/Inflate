using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class InflateObject : MonoBehaviour
{
    public bool inflating;
    public float inflateSpeed;
    public List<Box> boxes;

    public bool touched
    {
        get
        {
            if (Hero.touched == null)
                return false;
            else
                return Hero.touched.transform.parent == transform;
        }
    }

    public float FIXINGFLOAT;

    private Rigidbody2D rigidBody;
    private static float maxFallingSpeed = 15f;
    private static float gravity = 30f;
    private float needFeedBack;
    private bool canInflate;
    private float scale;
    private float maxScale;
    private Vector2 velocityValue;
    private bool paused;


    void Start()
    {
        scale = transform.localScale.x;
        rigidBody = GetComponent<Rigidbody2D>();
        needFeedBack = 0.1f;
    }

    void FixedUpdate()
    {
        if (!paused && !MyInput.paused)
        {
            Fall();
            Inflate();
            Test();
            FeedBack();
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

    private void Fall()
    {
        if (rigidBody.velocity.y > - maxFallingSpeed)
        {
            rigidBody.velocity = rigidBody.velocity - new Vector2(0, gravity * Time.fixedDeltaTime);
        }
    }

    public void Inflate()
    {
        inflating = touched;
        if (inflating)
        {
            scale += inflateSpeed * Time.fixedDeltaTime;
            transform.localScale = new Vector3(scale, scale, scale);
            if (scale > maxScale)
                maxScale = scale;
            needFeedBack = 0.1f;
        }
    }

    public void Test()
    {
        canInflate = true;
        foreach (Box box in boxes)
        {
            canInflate &= box.TestCanInflate();
        }
    }

    public void FeedBack()
    {
        if (needFeedBack > 0)
        {
            needFeedBack -= Time.fixedDeltaTime;
            if (!canInflate && scale >= (1 - FIXINGFLOAT) * maxScale)
            {
                scale -= inflateSpeed * Time.fixedDeltaTime;
                transform.localScale = new Vector3(scale, scale, scale);
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
